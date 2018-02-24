using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Data;
using Data.Creators;
using Data.Places;
using NUnit.Framework;

namespace DataTest
{
    [TestFixture]
    class DistrictCreatorTest
    {
        [Test]
        public void DistrictTest()
        {
            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);

            var district = new DistrictCreator().Create(dir + @"\..\..\..\..\Results\ResultsPresident\Алтайский край\", "*2012.txt");

            Common.CheckData(district, new District()
            {
                Name = "Алтайский край",
                NumberOfVoters = 1961328,
                NumberOfEarlier = 67,
                NumberOfInside = 1102078,
                NumberOfOutside = 73701,
                Stationary = 1101660,
                Portable = 73770,
                Valid = 1163426,
                InValid = 12004
            });
        }
    }
}
