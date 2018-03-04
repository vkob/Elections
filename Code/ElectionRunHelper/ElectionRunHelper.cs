﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Data.Get.Html.Xls.Txt;
using Elections;
using Elections.Utility;
using Extensions = Data.Core.Extensions;

namespace ElectionRunHelper
{
    class ElectionRunHelper
    {
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

            var electionsPrev = ProcessData.ReadSavedData(resultsPrev).ToDictionary(election => election.ElectionCommittee, election => election);

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
                    Extensions.ForEach(foos, foo =>
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
                Consts.ElectionYear2016,
            });
            processExcel.PrepareDrawAllDiagrams(new[]
            {
                Consts.ElectionYear2004,
                Consts.ElectionYear2008,
                Consts.ElectionYear2012,
            });
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

        private static void Main(string[] args)
        {
            switch (args[0])
            {
                case "1":
                    new Download().GetHtmlFiles(args[1]);
                    break;
                case "2":
                    new Download().GetXlsFiles(args[1]);//2 2016 
                    break;
                case "3"://3 2016
                    using (var excellExtracter = new ExcellExtracter())
                        excellExtracter.ExportXls(args[1]);
                    break;
                case "4":
                    new FinalXmlCreator().Start(args[1]);//4 2016
                    break;
                case "5":
                    GenerateDiagrams();
                    break;
                case "6":
                    new SortByDelta().Main();
                    break;
                case "7":
                    MainSum();
                    break;
                case "8":
                    MainDominant();//Сводная таблица результатов выборов Партии Власти и Президента за все года
                    break;
                case "12":
                    SortByDeltaFunc();
                    break;
                case "13":
                    FindLocalLeader();
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
