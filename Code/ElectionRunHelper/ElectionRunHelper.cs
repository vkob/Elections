using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Data.Get.Html.Xls.Txt;
using Elections;
using Elections.Diagrams;
using Elections.Diagrams.BarChart;
using Elections.Utility;

namespace ElectionRunHelper
{
    class ElectionRunHelper
    {
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
                    new BarChartPreparer(false).PrepareDrawAllDiagrams(Data.Core.Consts.Duma.Union(Data.Core.Consts.President).ToArray());
                    break;
                case "6":
                {
                    var sbMain = new StringBuilder();
                    var sbGraphics = new StringBuilder();

                    bool needOutput = true;

                    Pair<string, string> res = null;

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    ////2003
                    //res = Start(needOutput, new[] { Consts.ElectionYear2003 }, null, false);
                    //sbMain.Append(res.First);
                    //if (res != null) sbGraphics.Append(res.Second);

                    ////2004
                    //res = Start(needOutput, new[] { Consts.ElectionYear2004 }, null, false);
                    //sbMain.Append(res.First);
                    //if (res != null) sbGraphics.Append(res.Second);

                    ////2007
                    //res = Start(needOutput, new[] { Consts.ElectionYear2007 }, null, false);
                    //sbMain.Append(res.First);
                    //if (res != null) sbGraphics.Append(res.Second);

                    ////2008
                    //res = Start(needOutput, new[] { Consts.ElectionYear2004, Consts.ElectionYear2008 }, null, false);
                    //sbMain.Append(res.First);
                    //if (res != null) sbGraphics.Append(res.Second);

                    ////2011
                    //res = Start(needOutput, new[] { Consts.ElectionYear2007, Consts.ElectionYear2011 }, Consts.ElectionYear2007, true);
                    //sbMain.Append(res.First);
                    //if (res != null) sbGraphics.Append(res.Second);

                    ////2012
                    //res = Start(needOutput, new[] { Consts.ElectionYear2004, Consts.ElectionYear2008, Consts.ElectionYear2012 },
                    //   Consts.ElectionYear2004, true);
                    //sbMain.Append(res.First);
                    //if (res != null) sbGraphics.Append(res.Second);

                    ////2016
                    //res = Start(needOutput, new[] { Consts.ElectionYear2003, Consts.ElectionYear2007, Consts.ElectionYear2011, Consts.ElectionYear2016 },
                    //    Consts.ElectionYear2011, true);

                    res = new SortByDelta().Start(needOutput, new[] { Consts.ElectionYear2016 },
                        null, true);

                    sbMain.Append(res.First);
                    if (res != null) sbGraphics.Append(res.Second);

                    Html.GenerateResult(Consts.UpdatePath, sbMain, sbGraphics);

                    stopwatch.Stop();
                    Trace.WriteLine(String.Format("Generated HTML-s {0}", stopwatch.Elapsed));
                }
                    break;
                case "8":
                {
                    var sortByDelta = new SortByDelta();
                    sortByDelta.StartDominantForIks(new[]
                    {
                        Consts.ElectionYear2016,
                        Consts.ElectionYear2012,
                        Consts.ElectionYear2011,
                        Consts.ElectionYear2008,
                        Consts.ElectionYear2007,
                        Consts.ElectionYear2004,
                        Consts.ElectionYear2003
                    });
                    sortByDelta.StartDominantForRegions(new[]
                    {
                        Consts.ElectionYear2016,
                        Consts.ElectionYear2012,
                        Consts.ElectionYear2011,
                        Consts.ElectionYear2008,
                        Consts.ElectionYear2007,
                        Consts.ElectionYear2004,
                        Consts.ElectionYear2003
                    });
                    break;
                }
            }
        }
    }
}
