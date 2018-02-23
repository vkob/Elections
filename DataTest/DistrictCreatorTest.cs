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

            var list = new FileList(dir + @"\..\..\..\..\Results\ResultsPresident\Алтайский край\", "*2012.txt").GetList();
            var district = new DistrictCreator().Create(list);

            TikCreatorTest.CheckData(district, "Алтайский край", 1961328, 67, 1102078, 73701, 1101660, 73770, 1163426, 12004);
        }
    }
}
