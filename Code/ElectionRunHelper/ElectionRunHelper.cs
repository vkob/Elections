using Data.Get.Html.Xls.Txt;
using Elections;
using Elections.Diagrams;

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
                {
                    var processExcel = new BarChartPreparer(false);
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
                    break;
                }
                case "6":
                    new SortByDelta().Main();
                    break;
                case "8":
                {
                    var sortByDelta = new SortByDelta();
                    sortByDelta.DominantForIks();
                    sortByDelta.DominantForRegions();
                    break;
                }
            }
        }
    }
}
