using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Data.Core;
using Elections.Utility;
using Microsoft.Office.Interop.Excel;

namespace Elections.Diagrams
{
    public class BarChartPreparer : IDisposable
    {
        private readonly bool _overwrite;
        private readonly BarChartDrawer _barChartDrawer;

        public static Dictionary<string, int> PartiesOrder2003 = new[]
           {
            new KeyValuePair<string, int>("Единая Россия", 1),  //37.56%
            new KeyValuePair<string, int>("КПРФ", 2),           //12.61%
            new KeyValuePair<string, int>("ЛДПР", 3),           //11.45%
            new KeyValuePair<string, int>("Родина", 4),         //9.02%
            new KeyValuePair<string, int>("Яблоко", 5),         //4.30%
        }.ToDictionary(k => k.Key, k => k.Value);

        public static Dictionary<string, int> PartiesOrder2007 = new[]
        {
            new KeyValuePair<string, int>("Единая Россия", 1),      //64.30%
            new KeyValuePair<string, int>("КПРФ", 2),               //11.57%
            new KeyValuePair<string, int>("ЛДПР", 3),               //8.14%
            new KeyValuePair<string, int>("Справедливая Россия", 4),//7.74%
            new KeyValuePair<string, int>("Яблоко", 5),             //1.59%
        }.ToDictionary(k => k.Key, k => k.Value);

        public static Dictionary<string, int> PartiesOrder2011 = new[]
        {
            new KeyValuePair<string, int>("Единая Россия", 1),      //49.32%
            new KeyValuePair<string, int>("КПРФ", 2),               //19.19%
            new KeyValuePair<string, int>("ЛДПР", 3),               //11.67%
            new KeyValuePair<string, int>("Справедливая Россия", 4),//13.24%
            new KeyValuePair<string, int>("Яблоко", 5),             //3.43%
        }.ToDictionary(k => k.Key, k => k.Value);

        public static Dictionary<string, int> PartiesOrder2016 = new[]
        {
            new KeyValuePair<string, int>("Единая Россия", 1),      //54.20%
            new KeyValuePair<string, int>("КПРФ", 2),               //13.34%
            new KeyValuePair<string, int>("ЛДПР", 3),               //13.14%
            new KeyValuePair<string, int>("Справедливая Россия", 4),//6.22%
            new KeyValuePair<string, int>("Яблоко", 5),             //1.99%
        }.ToDictionary(k => k.Key, k => k.Value);

        public static Dictionary<int, Dictionary<string, int>> PartiesOrders = new Dictionary<int, Dictionary<string, int>>{
            {2003, PartiesOrder2003},
            {2007, PartiesOrder2007},
            {2011, PartiesOrder2011},
            {2016, PartiesOrder2016},};

        public Dictionary<string, int> PresidentOrder = new[]
        {
         new KeyValuePair<string, int>("Путин",1),
         new KeyValuePair<string,int>("Медведев",1),

         new KeyValuePair<string,int>("Харитонов",2),
         new KeyValuePair<string,int>("Зюганов",2),

         new KeyValuePair<string,int>("Глазьев",3),
         new KeyValuePair<string,int>("Жириновский",3),

         new KeyValuePair<string,int>("Хакамада",4),
         new KeyValuePair<string,int>("Богданов",4),
         new KeyValuePair<string,int>("Прохоров",4),

         new KeyValuePair<string,int>("Миронов",5),
      }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        public BarChartPreparer(bool overwrite)
        {
            _overwrite = overwrite;
            _barChartDrawer = new BarChartDrawer();
        }

        public void PrepareDrawAllDiagrams(ElectionYear[] electionYears)
        {
            Debug.Assert(electionYears.Select(ey => ey.ElectionType).Distinct().Count() == 1, "Wrong type in election years array");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            string path = Path.Combine(Data.Core.Consts.ResultsPath, electionYears[0].Result);

            var directoryInfo = new DirectoryInfo(path);
            var directoryInfos = directoryInfo.GetDirectories(); //Enumerable.Range(0, 10).Select(i => directoryInfo.GetDirectories()[i]).ToArray();

            var count = directoryInfos.Length / Environment.ProcessorCount;

            var threads = new Thread[Environment.ProcessorCount];
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var start = i * count;
                int length = (i < Environment.ProcessorCount - 1) ? count : directoryInfos.Length - start;
                var directoryInfosNew = new DirectoryInfo[length];
                Array.Copy(directoryInfos, start, directoryInfosNew, 0, length);
                threads[i] = new Thread(() => FindDataFiles(directoryInfosNew, electionYears));
            }

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            stopWatch.Stop();
            Trace.WriteLine(stopWatch.Elapsed);
        }

        public void FindDataFiles(DirectoryInfo[] directoryInfos, ElectionYear[] electionYears)
        {
            foreach (var di in directoryInfos)
            {
                if (di.FullName.EndsWith(Data.Core.Consts.LocalCommittee))
                {
                    foreach (var electionYear in electionYears)
                    {
                        ProcessFiles(di, electionYear);
                    }
                }

                FindDataFiles(di.GetDirectories(), electionYears);
            }
        }

        private void ProcessFiles(DirectoryInfo di, ElectionYear electionYear)
        {
            foreach (var fi in di.GetFiles(electionYear.PatternExt))
            {
                CreateDiagram(fi, electionYear, _overwrite);
            }
        }

        public string CreateDiagram(FileInfo fi, ElectionYear electionYear, bool overWrite)
        {
            var year = Convert.ToInt32(fi.FullName.Substring(fi.FullName.Length - 8, 4));
            var location = TextProcessFunctions.GetElectionCommitteeName(electionYear, fi.FullName, TextProcessFunctions.GetMapping());
            var picName = $@"{fi.DirectoryName}\{TextProcessFunctions.Translit(location)}{year}.jpg";

            if (File.Exists(picName) && !overWrite) return picName;

            var partiesOrders = (electionYear.ElectionType == ElectionType.Duma)
                ? PartiesOrders[electionYear.Year]
                : PresidentOrder;

            return _barChartDrawer.DrawDiagramForTxtData(DiagramDataCreator.Create(fi.FullName, picName, string.Format(electionYear.CaptionDiagram, year, location), partiesOrders));
        }

        public void Dispose()
        {
            _barChartDrawer.Dispose();
        }
    }
}
