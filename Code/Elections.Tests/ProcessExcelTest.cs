using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    public class ProcessExcelTest
    {
        [Test]
        public void Te()
        {
            //var processExcel = new ProcessExcel();
            //string fileName;
            //fileName = @"W:\VS2010\duma\Elections\ResultsDuma\Алтайский край\Табунская\СИЗКСРФ\Табунская 2011.txt";
            //processExcel.DrawDiagramForTxtData(new FileInfo(fileName), Consts.ElectionYear2011, true);
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
    }
}
