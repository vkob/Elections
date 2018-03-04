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
        public void DrawDiagramForTxtDataTest()
        {
            using (var processExcel = new ProcessExcel())
            {
                Common(processExcel, Consts.ElectionYear2007);
                Common(processExcel, Consts.ElectionYear2011);
            }
        }

        private void Common(ProcessExcel processExcel, ElectionYear electionYear)
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string fileName = Path.Combine(Path.Combine(dir, electionYear.FullPath),
                $@"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {electionYear.Year}.txt");
            processExcel.DrawDiagramForTxtData(new FileInfo(fileName), electionYear, true);
        }
    }
}
