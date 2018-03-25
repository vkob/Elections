using System.IO;
using System.Text;
using NUnit.Framework;

namespace Elections.Tests.BarChartPreparer
{
    [TestFixture]
    public class BarChartTest : BarChartPreparerBaseTest
    {
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
            Assert.AreEqual(553, HtmlSize("Arhangelsk"));
        }
    }
}
