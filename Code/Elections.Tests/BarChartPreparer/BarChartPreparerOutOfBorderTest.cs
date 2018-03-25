using NUnit.Framework;

namespace Elections.Tests.BarChartPreparer
{
    [TestFixture]
    public class BarChartPreparerOutOfBorderTest : BarChartPreparerBaseTest
    {
        [Test]
        public void DrawDiagramForTxtDataTest2018()
        {
            CreateFile(Consts.ElectionYear2018, @"Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ {0}.txt", 1009902);
        }

        [Test]
        public void HtmlTest()
        {
            Assert.AreEqual(67, HtmlSize("OutOfBorder"));
        }
    }
}
