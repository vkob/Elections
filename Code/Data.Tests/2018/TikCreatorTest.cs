using System.Diagnostics;
using System.IO;
using System.Linq;
using Data;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2018
{
    [TestFixture]
    public class TikCreatorTest
    {
        [Test]
        public void Мурманская()
        {
            var reader = new TikCreator();

            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);
            
            var tik = reader.Create(dir + @"\..\..\..\..\Results\ResultsPresident\Мурманская область\Мурманская\СИЗКСРФ\Мурманская 2018.txt");
            Assert.IsNotNull(tik);

            tik.Check();
        }

        [Test]
        public void Территория_за_пределами_РФ()
        {
            var reader = new TikCreator();

            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);

            var tik = reader.Create(dir + @"\..\..\..\..\Results\ResultsPresident\Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ 2018.txt");
            Assert.IsNotNull(tik);

            tik.Check();
        }
    }
}
