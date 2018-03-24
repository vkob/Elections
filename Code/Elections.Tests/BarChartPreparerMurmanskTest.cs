using System.IO;
using System.Text;
using Elections.Diagrams.BarChart;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    public class BarChartPreparerMurmanskTest
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
            CreateFile(Consts.ElectionYear2018, @"W:\VS\Reps\GitHub\Elections\Results\ResultsPresident\Мурманская область\Мурманская\СИЗКСРФ\Мурманская {0}.txt", 743886);
        }

        [Test]
        public void HtmlTest()
        {
            sb.AppendLine("</html>");

            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            var dest = Path.Combine(dir + @"\..\" + Data.Core.Consts.TopPath, "BarChartTest") + @"\Murmansk.html";
            using (var sw = new StreamWriter(dest))
            {
                sw.WriteLine(sb.ToString());
            }

            Assert.AreEqual(70, new FileInfo(dest).Length);
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
