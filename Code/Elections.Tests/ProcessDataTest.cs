using System.IO;
using System.Linq;
using Elections.Utility;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    public class ProcessDataTest
    {
        [Test]
        public void TestIrregularity()
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var data = ProcessData.GetElectionDataWithNormalizedPlace(dir + @"..\..\..\..\Elections\ElectionInfoDuma\ElectionInfoDuma2011.xml", 2011);
            Assert.AreEqual(39, 14, data["город Москва, район Гольяново"].GetFoo("ER").Irregularity);
        }

        [Test]
        public void TestNumbersOfVotedFor()
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var electionsLast = ProcessData.GetElectionDataWithNormalizedPlace(dir + @"..\..\..\..\Elections\ElectionInfoPresident\ElectionInfoPresident2012.xml", 2012);
            var electionsByRegion = electionsLast
                  .GroupBy(kvp => kvp.Value.Region, kvp => kvp.Value)
                  .ToDictionary(g => g.Key, g => g.ToList());
            var foo = "Putin";
            Assert.AreEqual(1994310, SortByDelta.NumberVotedFor(electionsByRegion, "Город Москва", foo));

            Assert.AreEqual(7309869, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Город Москва", foo));
            Assert.AreEqual(5779495, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Московская область", foo));
            Assert.AreEqual(3849426, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Город Санкт-Петербург", foo));
            Assert.AreEqual(3803307, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Краснодарский край", foo));
            Assert.AreEqual(3527808, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Свердловская область", foo));
            Assert.AreEqual(3315673, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Ростовская область", foo));
            Assert.AreEqual(3014076, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Республика Башкортостан", foo));
            Assert.AreEqual(2866307, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Республика Татарстан (Татарстан)", foo));
            Assert.AreEqual(2777766, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Нижегородская область", foo));
            Assert.AreEqual(2757879, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Челябинская область", foo));
        }
    }
}
