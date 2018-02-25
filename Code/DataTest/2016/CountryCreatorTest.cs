using System.IO;
using System.Linq;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2016
{
    [TestFixture]
    public class CountryCreatorTest
    {
        [Test]
        public void CountryTest()
        {
            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);

            var actualCountry = new CountryCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\", "*2016.txt");

            actualCountry.Districts = actualCountry.Districts.OrderBy(d => d.Name).ToList();
            var country = new Country()
            {
                NumberOfVoters = 110061200,
                NumberOfEarlier = 109868,
                NumberOfInside = 49174491,
                NumberOfOutside = 3416633,
                Portable = 3524522,
                Stationary = 49107327,
                Valid = 51649253,
                InValid = 982596,
            };
        
            Common.CheckData(actualCountry, country);
        }
    }
}
