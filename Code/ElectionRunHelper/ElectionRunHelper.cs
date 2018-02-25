using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using Elections;
using Elections.Utility;
using Elections.XmlProcessing;

namespace ElectionRunHelper
{
    class ElectionRunHelper
    {
        private static void FindMinMaxData()
        {
            var processTxt = new ProcessTxt();
            //processTxt.Start(Consts.ElectionYear2003);
            //processTxt.Start(Consts.ElectionYear2007);
            //processTxt.Start(Consts.ElectionYear2011);

            //processTxt.Start(Consts.ElectionYear2004);
            //processTxt.Start(Consts.ElectionYear2008);
            //processTxt.Start(Consts.ElectionYear2012);

            //processTxt.Start(Consts.ElectionAstrahan2009);
            processTxt.Start(Consts.ElectionAstrahan2012);
        }

        private static void SortByDeltaFunc()
        {
            var sortByDelta = new SortByDelta();
            sortByDelta.Main();
        }

        private static void FindLocalLeader()
        {
            var what = Consts.ElectionYear2011.DirElectionInfo;
            var year = Consts.ElectionYear2011.Year;
            var mainFooName = "ER";

            var resultsPrev = Consts.LocalPath + @"\" + Consts.ElectionsDir + @"\" + what + @"\" + what + year + ".xml";

            var xmlSerializer = new XmlSerializer(typeof(List<Election>));

            Func<string, Dictionary<string, Election>> getElectionData = (fileName) =>
            {
                using (var sr = new StreamReader(fileName))
                {
                    return ((List<Election>)xmlSerializer.Deserialize(sr)).ToDictionary(
                        election => election.ElectionCommittee, election => election);
                }
            };

            var electionsPrev = getElectionData(resultsPrev);

            foreach (var kvp in electionsPrev)
            {
                double maxValue = -1;
                kvp.Value.Foos.ForEach(foo =>
                {
                    if (foo.Value > maxValue) maxValue = foo.Value;
                });
                var foos = kvp.Value.Foos.Where(foo => Math.Abs(foo.Value - maxValue) < 0.001);
                if (foos.Count() > 1 || foos.First().Name != mainFooName)
                {
                    foos.ForEach(
                        foo =>
                            Trace.WriteLine(string.Format("{0}: {1} {2}", foo.Name, kvp.Value.ElectionCommittee,
                                foo.Value)));
                }
            }
        }

        private static void GeneratePresenceDiagram()
        {
            ProcessExcel.GenerateGraphic(Consts.ElectionYear2011, new[] { "ER", "KPRF", "SR", "LDPR" }, AxisYType.People,
                DiagramType.Presence);
            ProcessExcel.GenerateGraphic(Consts.ElectionYear2011, new[] { "ER", "KPRF", "SR", "LDPR" }, AxisYType.UIK,
                DiagramType.Presence);
        }

        private static void GenerateFooResultsDiagram()
        {
            ProcessExcel.GenerateGraphic(Consts.ElectionYear2011, new[] { "ER", "KPRF", "SR", "LDPR", "YA" },
                AxisYType.People, DiagramType.Results);
            ProcessExcel.GenerateGraphic(Consts.ElectionYear2011, new[] { "ER", "KPRF", "SR", "LDPR", "YA" },
                AxisYType.UIK, DiagramType.Results);
        }

        private static void GenerateDiagrams()
        {
            var processExcel = new ProcessExcel();
            processExcel.PrepareDrawAllDiagrams(new[]
            {
                Consts.ElectionYear2003,
                Consts.ElectionYear2007,
                Consts.ElectionYear2011,
            });
            processExcel.PrepareDrawAllDiagrams(new[]
            {
                Consts.ElectionYear2004,
                Consts.ElectionYear2008,
                Consts.ElectionYear2012,
            });
            processExcel.PrepareDrawAllDiagrams(new[]
            {
                Consts.ElectionAstrahan2009,
                Consts.ElectionAstrahan2012
            });
        }

        private static void DrawOneDiagram()
        {
            var processExcel = new ProcessExcel();
            string fileName;
            //fileName = @"W:\VS2010\duma\Elections\ResultsDuma\Город Москва - Восточная\район Гольяново\СИЗКСРФ\район гольяново 2011.txt";
            //processExcel.DrawDiagramForTxtData(new FileInfo(fileName), ElectionType.Duma, true);

            //fileName = @"W:\VS2010\duma\Elections\ResultsPresident\Город Москва\район Гольяново\СИЗКСРФ\район гольяново 2012.txt";
            //processExcel.DrawDiagramForTxtData(new FileInfo(fileName), ElectionType.President, true);

            //fileName = @"W:\VS2010\duma\Elections\ResultsDuma\Чеченская Республика\Наурская\СИЗКСРФ\наурская 2011.txt";
            //processExcel.DrawDiagramForTxtData(new FileInfo(fileName), ElectionType.Duma, true);

            fileName = @"W:\VS2010\duma\Elections\ResultsDuma\Алтайский край\Табунская\СИЗКСРФ\Табунская 2011.txt";
            processExcel.DrawDiagramForTxtData(new FileInfo(fileName), Consts.ElectionYear2007, true);

        }

        private static void DownloadHtml(string year)
        {
            var download = new Download();

            switch (year)
            {
                case "2003":
                    download.Start(Consts.ElectionYear2003);
                    break;
                case "2004":
                    download.Start(Consts.ElectionYear2004);
                    break;
                case "2007":
                    download.Start(Consts.ElectionYear2007);
                    break;
                case "2008":
                    download.Start(Consts.ElectionYear2008);
                    break;
                case "2009":
                    download.Start(Consts.ElectionAstrahan2009);
                    download.Start(Consts.ElectionAstrahan2012);
                    break;
                case "2011":
                    download.Start(Consts.ElectionYear2011);
                    break;
                case "2012":
                    download.Start(Consts.ElectionYear2012);
                    break;
                case "2016":
                    download.Start(Consts.ElectionYear2016);
                    break;

            }
        }

        private static void DownloadXls(string type, string year)
        {
            Download.FindFileForXlsExtraction(GetResultPath(type), year);
        }

        private static string GetResultPath(string type)
        {
            switch (type.ToLower())
            {
                case "duma":
                    return Consts.LocalPathDumaResults;
                case "astrahan":
                    return Consts.LocalPathAstrahanResults;
                case "president":
                    return Consts.LocalPathPresidentResults;
                default:
                    throw new Exception("Unknown type");
            }
        }

        private static void ExtraxtXlsToTxt(string type, string years)
        {
            var processExcel = new ProcessExcel();
            processExcel.ExportXls(GetResultPath(type), years);
        }

        private static void GenerateAll()
        {
            var sortByDelta = new SortByDelta();
            sortByDelta.Main();
        }

        private static void MainSum()
        {
            var sortByDelta = new SortByDelta();
            sortByDelta.MainSum();
        }

        private static void MainDominant()
        {
            var sortByDelta = new SortByDelta();
            sortByDelta.DominantForIks();
            sortByDelta.DominantForRegions();
        }

        public static void CopyFiles()
        {
            var zipFiles = new ZipFiles();
            zipFiles.StartCopying(new[]
            {
                //Consts.ElectionAstrahan2009, 
                //Consts.ElectionAstrahan2012, 
                //Consts.ElectionYear2003, 
                //Consts.ElectionYear2004,
                //Consts.ElectionYear2007,
                //Consts.ElectionYear2008,
                //Consts.ElectionYear2011,
                Consts.ElectionYear2012
            });
        }

        public static void ZipFiles()
        {
            var zipFiles = new ZipFiles();
            zipFiles.StartZipping(new[]
            {
                Consts.ElectionAstrahan2009,
                Consts.ElectionAstrahan2012,
                Consts.ElectionYear2003,
                Consts.ElectionYear2004,
                Consts.ElectionYear2007,
                Consts.ElectionYear2008,
                Consts.ElectionYear2011,
                Consts.ElectionYear2012
            });
        }

        public static void CopyZipFiles()
        {
            var zipFiles = new ZipFiles();
            zipFiles.CopyZipFiles();
        }

        private static void Main(string[] args)
        {
            switch (args[0])
            {
                case "1":
                    DownloadHtml(args[1]);//1 2016
                    break;
                case "2":
                    DownloadXls(args[1], args[2]);//2 duma 2016 
                    break;
                case "3":
                    ExtraxtXlsToTxt(args[1], args[2]);//3 duma 2016
                    break;
                case "4":
                    FindMinMaxData();
                    break;
                case "5":
                    GenerateDiagrams();
                    break;
                case "6":
                    GenerateAll();
                    break;
                case "7":
                    MainSum();
                    break;
                case "8":
                    MainDominant();
                    break;
                case "9":
                    CopyFiles();
                    break;
                case "10":
                    ZipFiles();
                    break;
                case "11":
                    CopyZipFiles();
                    break;
                case "12":
                    SortByDeltaFunc();
                    break;
                case "13":
                    FindLocalLeader();
                    break;
                case "14":
                    GenerateDiagrams();
                    break;
                case "15":
                    DrawOneDiagram();
                    break;
                case "16":
                    GeneratePresenceDiagram();
                    break;
                case "17":
                    GenerateFooResultsDiagram();
                    break;
            }
        }
    }
}
