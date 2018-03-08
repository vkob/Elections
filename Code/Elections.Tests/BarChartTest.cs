using System.IO;
using Elections.Diagrams;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    public class BarChartTest
    {
        private BarChartPreparer _barChartPreparer;

        [OneTimeSetUp]
        public void Init()
        {
            _barChartPreparer = new BarChartPreparer();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _barChartPreparer.Dispose();
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

        [Test]
        public void DrawDiagramForTxtDataTest2003()
        {
            CreateFile(Consts.ElectionYear2003, @"Архангельская область\Архангельский\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 102800);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2007()
        {
            CreateFile(Consts.ElectionYear2007, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 109053);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2011()
        {
            CreateFile(Consts.ElectionYear2011, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 105361);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2016()
        {
            CreateFile(Consts.ElectionYear2016, @"ОИК №72\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 101723);
        }

        public void CreateFile(ElectionYear electionYear, string path, int fileLength)
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var dest = Path.Combine(dir + @"\..\" + Data.Core.Consts.TopPath, "BarChartTest");
            Directory.CreateDirectory(dest);
            var dirInfo = new DirectoryInfo(dest);

            string fileName = Path.Combine(Path.Combine(dir, electionYear.FullPath), string.Format(path, electionYear.Year));
            var fileNameSource =  _barChartPreparer.DrawDiagramForTxtData(new FileInfo(fileName), electionYear, true);

            var fileInfo = new FileInfo(fileNameSource);

            var destFileName = Path.Combine(Path.GetFullPath(dirInfo.FullName), Path.GetFileName(fileNameSource));

            if (File.Exists(destFileName)) File.Delete(destFileName);

            fileInfo.CopyTo(destFileName);

            Assert.AreEqual(fileLength, new FileInfo(fileNameSource).Length);
        }
    }
}
