using System.IO;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2016
{
    [TestFixture]
    class DistrictCreatorTest
    {
        [Test]
        public void DistrictTest()
        {
            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);

            var district = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\ОИК №40\", "*2016.txt");

            Common.CheckData(district, new District()
            {
                Name = "ОИК №40",
                NumberOfVoters = 494449,
                NumberOfEarlier = 0,
                NumberOfInside = 188475,
                NumberOfOutside = 10649,
                Stationary = 188337,
                Portable = 10631,
                Valid = 193889,
                InValid = 5079
            });
        }
    }
}
