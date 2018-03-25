using System.Diagnostics;
using System.IO;
using System.Linq;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2003
{
    [TestFixture]
    public class CountryCreatorTest
    {
        [Test]
        public void CountryTest()
        {  
            var dir = Path.GetDirectoryName(typeof(CountryCreatorTest).Assembly.Location);

            var actualCountry = new CountryCreator().Create(dir + @"\..\..\..\..\Results\ResultsDuma\", "*2003.txt");

            var country = new Country()
            {
                NumberOfVoters = 108906249,
                NumberOfEarlier = 74034,
                NumberOfInside = 57310748,
                NumberOfOutside = 3327519,
                Portable = 3392547,
                Stationary = 57240630,
                Valid = 59684742,
                InValid = 948435,
            };

            Trace.WriteLine(string.Join("\n",actualCountry.Districts.Select(d => d.NumberOfVoters)));

            Common.CheckData(actualCountry, country);
        }
    }
}
