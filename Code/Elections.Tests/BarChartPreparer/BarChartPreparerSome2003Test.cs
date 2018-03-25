using NUnit.Framework;

namespace Elections.Tests.BarChartPreparer
{
    class BarChartPreparerSome2003Test : BarChartPreparerBaseTest
    {
        [Test]
        public void DrawDiagramForБалтийская2003()
        {
            CreateFile(Consts.ElectionYear2003, @"Калининградская область\Калининградский\Балтийская\СИЗКСРФ\Балтийская {0}.txt", 71074);
        }

        [Test]
        public void DrawDiagramForВладивостокПервомайская2003()
        {
            CreateFile(Consts.ElectionYear2003, @"Приморский край\Владивостокский\Владивосток, Первомайская\СИЗКСРФ\Владивосток, Первомайская {0}.txt", 272088);
        }

        [Test]
        public void DrawDiagramForВладивостокФрунзенская2003()
        {
            CreateFile(Consts.ElectionYear2003, @"Приморский край\Владивостокский\Владивосток, Фрунзенская\СИЗКСРФ\Владивосток, Фрунзенская {0}.txt", 241629);
        }

        [Test]
        public void DrawDiagramForСибайскаяГородская2003()
        {
            CreateFile(Consts.ElectionYear2003, @"Республика Башкортостан\Кумертауский\Сибайская городская\СИЗКСРФ\Сибайская городская {0}.txt", 84779);
        }

        [Test]
        public void DrawDiagramForНевельскаяCудовая()
        {
            CreateFile(Consts.ElectionYear2003, @"Сахалинская область\Сахалинский\Невельская судовая\СИЗКСРФ\Невельская судовая {0}.txt", 286193);
        }

        [Test]
        public void HtmlTest()
        {
            Assert.AreEqual(333, HtmlSize("Some2003"));
        }
    }
}
