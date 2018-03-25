using NUnit.Framework;

namespace Elections.Tests.BarChartPreparer
{
    [TestFixture]
    public class BarChartPreparerMurmanskTest : BarChartPreparerBaseTest
    {
        [Test]
        public void DrawDiagramForTxtDataTest2018()
        {
            CreateFile(Consts.ElectionYear2018, @"Мурманская область\Мурманская\СИЗКСРФ\Мурманская {0}.txt", 743886);
        }

        [Test]
        public void HtmlTest()
        {
            Assert.AreEqual(70, HtmlSize("Murmansk"));
        }
    }
}
