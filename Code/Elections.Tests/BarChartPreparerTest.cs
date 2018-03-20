﻿using System.IO;
using System.Text;
using Elections.Diagrams.BarChart;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    public class BarChartTest
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
        public void DrawDiagramForTxtDataTest2003()
        {
            CreateFile(Consts.ElectionYear2003, @"Архангельская область\Архангельский\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 102800);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2004()
        {
            CreateFile(Consts.ElectionYear2004, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 113838);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2007()
        {
            CreateFile(Consts.ElectionYear2007, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 109053);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2008()
        {
            CreateFile(Consts.ElectionYear2008, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 111890);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2011()
        {
            CreateFile(Consts.ElectionYear2011, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 105361);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2012()
        {
            CreateFile(Consts.ElectionYear2012, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 116228);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2016()
        {
            CreateFile(Consts.ElectionYear2016, @"ОИК №72\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 101723);
        }

        [Test]
        public void DrawDiagramForTxtDataTest2018()
        {
            CreateFile(Consts.ElectionYear2018, @"Архангельская область\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская {0}.txt", 108234);
        }

        [Test]
        public void HtmlTest()
        {
            sb.AppendLine("</html>");

            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            var dest = Path.Combine(dir + @"\..\" + Data.Core.Consts.TopPath, "BarChartTest") + @"\All.html";
            using (var sw = new StreamWriter(dest))
            {
                sw.WriteLine(sb.ToString());
            }

            Assert.AreEqual(553, new FileInfo(dest).Length);
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