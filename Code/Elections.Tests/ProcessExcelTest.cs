using System;
using System.CodeDom;
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
        private ProcessExcel processExcel;

        [OneTimeSetUp]
        public void Init()
        {
            processExcel = new ProcessExcel();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            processExcel.Dispose();
        }

        [Test]
        public void DrawDiagramForTxtDataTest2003()
        {
            CreateFile(Consts.ElectionYear2003, @"Архангельская область\Архангельский\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 102800);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2007()
        {
            CreateFile(Consts.ElectionYear2007, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 112830);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2011()
        {
            CreateFile(Consts.ElectionYear2011, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 112006);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2016()
        {
            CreateFile(Consts.ElectionYear2016, @"ОИК №72\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 92602);
        }

        public void CreateFile(ElectionYear electionYear, string path, int fileLength)
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var dest = Path.Combine(dir + @"\..\" + Data.Core.Consts.TopPath, "ProcessExcelTest");
            Directory.CreateDirectory(dest);
            var dirInfo = new DirectoryInfo(dest);
            var fileNameSource = Common(processExcel, electionYear, path);

            var fileInfo = new FileInfo(fileNameSource);

            var destFileName = Path.Combine(Path.GetFullPath(dirInfo.FullName), Path.GetFileName(fileNameSource));

            if (File.Exists(destFileName)) File.Delete(destFileName);

            fileInfo.CopyTo(destFileName);

            Assert.AreEqual(fileLength, new FileInfo(fileNameSource).Length);
        }

        private string Common(ProcessExcel processExcel, ElectionYear electionYear, string path)
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            string fileName = Path.Combine(Path.Combine(dir, electionYear.FullPath), string.Format(path, electionYear.Year));
            return processExcel.DrawDiagramForTxtData(new FileInfo(fileName), electionYear, true);
        }
    }
}
