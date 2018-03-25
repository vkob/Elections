using System.IO;
using System.Text;
using NUnit.Framework;

namespace Elections.Tests.BarChartPreparer
{
    [TestFixture]
    public class BarChartPreparerBaseTest 
    {
        private Diagrams.BarChart.BarChartPreparer _barChartPreparer;
        private StringBuilder sb = new StringBuilder("<html>");

        [OneTimeSetUp]
        public void Init()
        {
            _barChartPreparer = new Diagrams.BarChart.BarChartPreparer(true);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _barChartPreparer.Dispose();
        }

        protected long HtmlSize(string name)
        {
            sb.AppendLine("</html>");

            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            var dest = Path.Combine(dir + @"\..\" + Data.Core.Consts.TopPath, "BarChartTest") + $@"\{name}.html";
            using (var sw = new StreamWriter(dest))
            {
                sw.WriteLine(sb.ToString());
            }

            return new FileInfo(dest).Length;
        }

        protected void CreateFile(ElectionYear electionYear, string path, int fileLength)
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var dest = Path.Combine(dir + @"\..\" + Data.Core.Consts.TopPath, "BarChartTest");
            Directory.CreateDirectory(dest);
            var dirInfo = new DirectoryInfo(dest);

            string fileName = Path.Combine(Path.Combine(dir, electionYear.FullPath), string.Format(path, electionYear.Year));
            var fileNameSource = _barChartPreparer.CreateDiagram(new FileInfo(fileName), electionYear.CaptionDiagram, true);

            var fileInfo = new FileInfo(fileNameSource);

            var destFileName = Path.Combine(Path.GetFullPath(dirInfo.FullName), Path.GetFileName(fileNameSource));

            sb.AppendLine($"<img src=\"{Path.GetFileName(fileNameSource)}\"/>");

            if (File.Exists(destFileName)) File.Delete(destFileName);

            fileInfo.CopyTo(destFileName);

            Assert.AreEqual(fileLength, new FileInfo(fileNameSource).Length);
        }
    }
}
