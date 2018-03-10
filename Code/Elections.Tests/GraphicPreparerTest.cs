using Elections.Diagrams.Graphic;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    class GraphicPreparerTest
    {
        [Test]
        public void Test()
        {

        }

        private static void GeneratePresenceDiagram()
        {
            GraphicPreparer.GenerateGraphic(Consts.ElectionYear2011, new[] { "ER", "KPRF", "SR", "LDPR" }, AxisYType.People,
                DiagramType.Presence);
            GraphicPreparer.GenerateGraphic(Consts.ElectionYear2011, new[] { "ER", "KPRF", "SR", "LDPR" }, AxisYType.UIK,
                DiagramType.Presence);
        }

        private static void GenerateFooResultsDiagram()
        {
            GraphicPreparer.GenerateGraphic(Consts.ElectionYear2011, new[] { "ER", "KPRF", "SR", "LDPR", "YA" },
                AxisYType.People, DiagramType.Results);
            GraphicPreparer.GenerateGraphic(Consts.ElectionYear2011, new[] { "ER", "KPRF", "SR", "LDPR", "YA" },
                AxisYType.UIK, DiagramType.Results);
        }
    }
}
