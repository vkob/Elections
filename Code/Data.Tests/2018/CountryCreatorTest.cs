using System.Diagnostics;
using System.IO;
using System.Linq;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2018
{
    [TestFixture]
    public class CountryCreatorTest
    {
        [Test]
        public void CountryTest2018()
        {
            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);

            var actualCountry = new CountryCreator().Create(dir + @"\..\..\..\..\Results\ResultsPresident\", "*2018.txt");

            var country = new Country()
            {
                NumberOfVoters = 109008428,
                NumberOfEarlier = 219648,
                NumberOfInside = 68587926,
                NumberOfOutside = 4822007,
                Portable = 5039911,
                Stationary = 68539081,
                Valid = 72787734,
                InValid = 791258,
            };

            Common.CheckData(actualCountry, country);
        }
    }
}
