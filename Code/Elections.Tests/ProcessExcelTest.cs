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
                var filename1 = Common(processExcel, Consts.ElectionYear2007);
                Assert.AreEqual(112830, new FileInfo(filename1).Length);
                var filename2 = Common(processExcel, Consts.ElectionYear2011);
                Assert.AreEqual(112006, new FileInfo(filename2).Length);
            }
        }

        private string Common(ProcessExcel processExcel, ElectionYear electionYear)
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string fileName = Path.Combine(Path.Combine(dir, electionYear.FullPath),
                $@"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {electionYear.Year}.txt");
            return processExcel.DrawDiagramForTxtData(new FileInfo(fileName), electionYear, true);
        }
    }
}
