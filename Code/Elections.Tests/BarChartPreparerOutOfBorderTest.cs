using System.IO;
using System.Text;
using Elections.Diagrams.BarChart;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    public class BarChartPreparerOutOfBorderTest
    {
        private BarChartPreparer _barChartPreparer;
        private StringBuilder sb = new StringBuilder("<html>");

        [OneTimeSetUp]
        public void Init()
        {
            _barChartPreparer = new BarChartPreparer(true);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _barChartPreparer.Dispose();
        }
      

        [Test]
        public void DrawDiagramForTxtDataTest2018()
        {
            CreateFile(Consts.ElectionYear2018, @"Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ {0}.txt", 1009902);
        }

        [Test]
        public void HtmlTest()
        {
            sb.AppendLine("</html>");

            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            var dest = Path.Combine(dir + @"\..\" + Data.Core.Consts.TopPath, "BarChartTest") + @"\OutOfBorder.html";
            using (var sw = new StreamWriter(dest))
            {
                sw.WriteLine(sb.ToString());
            }

            Assert.AreEqual(67, new FileInfo(dest).Length);
        }
        
        public void CreateFile(ElectionYear electionYear, string path, int fileLength)
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var dest = Path.Combine(dir + @"\..\" + Data.Core.Consts.TopPath, "BarChartTest");
            Directory.CreateDirectory(dest);
            var dirInfo = new DirectoryInfo(dest);

            string fileName = Path.Combine(Path.Combine(dir, electionYear.FullPath), string.Format(path, electionYear.Year));
            var fileNameSource =  _barChartPreparer.CreateDiagram(new FileInfo(fileName), electionYear.CaptionDiagram, true);

            var fileInfo = new FileInfo(fileNameSource);

            var destFileName = Path.Combine(Path.GetFullPath(dirInfo.FullName), Path.GetFileName(fileNameSource));

            sb.AppendLine($"<img src=\"{Path.GetFileName(fileNameSource)}\"/>");

            if (File.Exists(destFileName)) File.Delete(destFileName);

            fileInfo.CopyTo(destFileName);

            Assert.AreEqual(fileLength, new FileInfo(fileNameSource).Length);
        }
    }
}
