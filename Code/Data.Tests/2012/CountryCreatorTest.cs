using System.Diagnostics;
using System.IO;
using System.Linq;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2012
{
    [TestFixture]
    public class CountryCreatorTest
    {
        [Test]
        public void CountryTest()
        {  
            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);

            var actualCountry = new CountryCreator().Create(dir + @"\..\..\..\..\Results\ResultsPresident\", "*2012.txt");

            var country = new Country()
            {
                NumberOfVoters = 109860331,
                NumberOfEarlier = 239569,
                NumberOfInside = 65639398,
                NumberOfOutside = 5901833,
                Portable = 6139277,
                Stationary = 65562388,
                Valid = 70864974,
                InValid = 836691,
            };

            Common.CheckData(actualCountry, country);
        }
    }
}
