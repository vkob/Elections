using System.IO;
using Data;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2003
{
    [TestFixture]
    public class TikCreatorTest
    {
        TikCreator reader = new TikCreator();
        string dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);

        [Test]
        public void TikTest1()
        {
            var tik = reader.Create(dir + @"\..\..\..\..\Results\ResultsDuma\Город Москва\Автозаводский\Район Лефортово\СИЗКСРФ\Район Лефортово 2003.txt");
            Assert.AreEqual(53078, tik.NumberOfVoters);
        }

        [Test]
        public void TikTest2()
        {
            var tik = reader.Create(dir + @"\..\..\..\..\Results\ResultsDuma\Город Москва\Автозаводский\Даниловский район\СИЗКСРФ\Даниловский район 2003.txt");
            Assert.AreEqual(60356, tik.NumberOfVoters);
        }
    }
}
