using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using NUnit.Framework;

namespace DataTest
{
    public static class Common
    {
        public static void CheckData(IElectItem actual, IElectItem expected)
        {
            Assert.AreEqual(expected.NumberOfVoters, actual.NumberOfVoters);

            Assert.AreEqual(expected.Name, actual.Name);

            Assert.AreEqual(expected.NumberOfEarlier, actual.NumberOfEarlier);
            Assert.AreEqual(expected.NumberOfInside, actual.NumberOfInside);
            Assert.AreEqual(expected.NumberOfOutside, actual.NumberOfOutside);

            Assert.AreEqual(expected.Stationary, actual.Stationary);
            Assert.AreEqual(expected.Portable, actual.Portable);

            Assert.AreEqual(expected.Valid, actual.Valid);
            Assert.AreEqual(expected.InValid, actual.InValid);
        }
    }
}
