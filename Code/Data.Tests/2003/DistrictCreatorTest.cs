using System.Diagnostics;
using System.IO;
using System.Linq;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2003
{
    [TestFixture]
    class DistrictCreatorTest
    {
        string dir = Path.GetDirectoryName(typeof(DistrictCreatorTest).Assembly.Location);

        [Test]
        public void DistrictTest1()
        { 
            var district1 = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\Город Москва\Автозаводский\", "*2003.txt");
            Assert.AreEqual(496413, district1.NumberOfVoters);

            var district2 = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\Город Москва\Бабушкинский\", "*2003.txt");
            Assert.AreEqual(459853, district2.NumberOfVoters);

            var district3 = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\Город Москва\Кунцевский\", "*2003.txt");
            Assert.AreEqual(7, district3.Tiks.Count);

            Assert.AreEqual(452133, district3.NumberOfVoters);

            var district4 = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\Город Москва\Ленинградский\", "*2003.txt");
            Assert.AreEqual(499680, district4.NumberOfVoters);
        }

        [Test]
        public void DistrictTest()
        {
            var district = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\Город Москва\", "*2003.txt");

            Assert.AreEqual(7120520, district.NumberOfVoters);
            Assert.AreEqual("Город Москва", district.Name);
        }

        [Test]
        public void DistrictTest2()
        {
            var district1 = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\Архангельская область\Котласский\", "*2003.txt");

            Trace.WriteLine(string.Join(" ", district1.Tiks.Select(t => t.Name).OrderBy(r => r)));
            Assert.AreEqual(471381, district1.NumberOfVoters);

            var district2 = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\Архангельская область\Архангельский", "*2003.txt");
            Assert.AreEqual(541462, district2.NumberOfVoters);

            var district = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\Архангельская область\", "*2003.txt");
            Assert.AreEqual(1012843, district.NumberOfVoters);
        }
    }
}
