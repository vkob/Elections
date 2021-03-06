﻿using System.Diagnostics;
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

            var data = ProcessData.GetElectionDataWithNormalizedPlace(dir + @"..\..\..\..\Elections\ElectionInfoDuma\ElectionInfoDuma2011.xml");
            Assert.AreEqual(39, 14, data["город Москва, район Гольяново"].GetFoo("ER").Irregularity);
        }

        [Test]
        public void NumberOfElectorsInList2012()
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var electionsLast = ProcessData.GetElectionDataWithNormalizedPlace(dir + @"..\..\..\..\Elections\ElectionInfoPresident\ElectionInfoPresident2012.xml");
            var electionsByRegion = electionsLast
                  .GroupBy(kvp => kvp.Value.TextData.Region, kvp => kvp.Value)
                  .ToDictionary(g => g.Key, g => g.ToList());
            var foo = "Putin";
            Assert.AreEqual(1994310, SortByDelta.NumberVotedFor(electionsByRegion, "Город Москва", foo));

            Assert.AreEqual(7309869, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Город Москва"));
            Assert.AreEqual(5779495, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Московская область"));
            Assert.AreEqual(3849426, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Город Санкт-Петербург"));
            Assert.AreEqual(3803307, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Краснодарский край"));
            Assert.AreEqual(3527808, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Свердловская область"));
            Assert.AreEqual(3315673, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Ростовская область"));
            Assert.AreEqual(3014076, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Республика Башкортостан"));
            Assert.AreEqual(2866307, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Республика Татарстан (Татарстан)"));
            Assert.AreEqual(2777766, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Нижегородская область"));
            Assert.AreEqual(2757879, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Челябинская область"));

            foreach (var regionPair in electionsByRegion)
            {
                //Trace.WriteLine(regionPair.Key);
            }

            Assert.AreEqual(85, electionsByRegion.Count);
        }

        [Test]
        public void NumberOfElectorsInList2016()
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var electionsLast = ProcessData.GetElectionDataWithNormalizedPlace(dir + @"..\..\..\..\Elections\ElectionInfoDuma\ElectionInfoDuma2016.xml");
            var electionsByRegion = electionsLast
                  .GroupBy(kvp => kvp.Value.TextData.Region, kvp => kvp.Value)
                  .ToDictionary(g => g.Key, g => g.ToList());

            Assert.AreEqual(7452834, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Город Москва"));
            Assert.AreEqual(5649321, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Московская область"));
            Assert.AreEqual(3835562, SortByDelta.NumberOfElectorsInList(electionsByRegion, "Город Санкт-Петербург"));

            foreach (var regionPair in electionsByRegion)
            {
                //Trace.WriteLine(regionPair.Key);
            }

            Assert.AreEqual(85, electionsByRegion.Count);
        }
    }
}
