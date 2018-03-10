using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Data.Core;
using Elections.Utility;

namespace Elections.Diagrams.BarChart
{
    public class BarChartPreparer : IDisposable
    {
        private readonly bool _overwrite;
        private readonly BarChartDrawer _barChartDrawer;

        public static Dictionary<string, int> PartiesOrder = new[]
           {
            new KeyValuePair<string, int>("Единая Россия", 1),
            new KeyValuePair<string, int>("Путин",1),
            new KeyValuePair<string, int>("Медведев",1),

            new KeyValuePair<string, int>("КПРФ", 2),
            new KeyValuePair<string, int>("Харитонов",2),
            new KeyValuePair<string, int>("Зюганов",2),
               
            new KeyValuePair<string, int>("ЛДПР", 3),
            new KeyValuePair<string, int>("Глазьев",3),
            new KeyValuePair<string, int>("Жириновский",3),
               
            new KeyValuePair<string, int>("Родина", 4), //2003         
            new KeyValuePair<string, int>("Справедливая Россия", 4),
            new KeyValuePair<string, int>("Хакамада",4),
            new KeyValuePair<string, int>("Богданов",4),
            new KeyValuePair<string, int>("Прохоров",4),

            new KeyValuePair<string, int>("Яблоко", 5),
            new KeyValuePair<string, int>("Миронов",5),
        }.ToDictionary(k => k.Key, k => k.Value);

        public BarChartPreparer(bool overwrite)
        {
            _overwrite = overwrite;
            _barChartDrawer = new BarChartDrawer();
        }

        public void PrepareDrawAllDiagrams(int[] years)
        {
            var dumaYears = years.Where(y => Data.Core.Consts.Duma.Contains(y)).ToArray();
            var presidentYears = years.Where(y => Data.Core.Consts.President.Contains(y)).ToArray();

            PrepareDrawAllDiagrams(dumaYears, Path.Combine(Data.Core.Consts.ResultsPath, Data.Core.Consts.ResultsDuma), ElectionYear.CaptionDiagramDuma);
            PrepareDrawAllDiagrams(presidentYears, Path.Combine(Data.Core.Consts.ResultsPath, Data.Core.Consts.ResultsPresident), ElectionYear.CaptionDiagramPresident);
        }

        public void PrepareDrawAllDiagrams(int[] years, string path, string captionDiagram)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var directoryInfo = new DirectoryInfo(path);
            var directoryInfos = directoryInfo.GetDirectories();

            var count = directoryInfos.Length / Environment.ProcessorCount;

            var threads = new Thread[Environment.ProcessorCount];
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var start = i * count;
                int length = (i < Environment.ProcessorCount - 1) ? count : directoryInfos.Length - start;
                var directoryInfosNew = new DirectoryInfo[length];
                Array.Copy(directoryInfos, start, directoryInfosNew, 0, length);
                threads[i] = new Thread(() => FindDataFiles(directoryInfosNew, years.Select(y => $"*{y}.txt").ToArray(), captionDiagram));
            }

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            stopWatch.Stop();
            Trace.WriteLine(stopWatch.Elapsed);
        }

        public void FindDataFiles(DirectoryInfo[] directoryInfos, string[] patterns, string captionDiagram)
        {
            foreach (var di in directoryInfos)
            {
                if (di.FullName.EndsWith(Data.Core.Consts.LocalCommittee))
                {
                    foreach (var pattern in patterns)
                    {
                        ProcessFiles(di, pattern, captionDiagram);
                    }
                }

                FindDataFiles(di.GetDirectories(), patterns, captionDiagram);
            }
        }

        private void ProcessFiles(DirectoryInfo di, string pattern, string captionDiagram)
        {
            foreach (var fi in di.GetFiles(pattern))
            {
                CreateDiagram(fi, captionDiagram, _overwrite);
            }
        }

        public string CreateDiagram(FileInfo fi, string captionDiagram, bool overWrite)
        {
            var year = TextProcessFunctions.GetYear(fi.Name);
            var location = TextProcessFunctions.GetElectionCommitteeName(fi.FullName, null, TextProcessFunctions.GetMapping());
            var picName = $@"{fi.DirectoryName}\{TextProcessFunctions.Translit(location)}{year}.jpg";

            if (File.Exists(picName) && !overWrite) return picName;

            return _barChartDrawer.DrawDiagramForTxtData(DiagramDataCreator.Create(fi.FullName, picName, string.Format(captionDiagram, year, location), PartiesOrder));
        }

        public void Dispose()
        {
            _barChartDrawer.Dispose();
        }
    }
}
