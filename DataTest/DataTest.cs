using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Data;
using NUnit.Framework;

namespace DataTest
{
    [TestFixture]
    public class DataTest
    {
        [Test]
        public void TxtFileDataExtractTest()
        {
            var reader = new Reader();

            var dir = Path.GetDirectoryName(typeof(DataTest).Assembly.Location);
            
            var tik = reader.Generate(dir + @"\..\..\..\..\Results\ResultsPresident\Алтайский край\Алейская\СИЗКСРФ\Алейская 2012.txt");
            Assert.IsNotNull(tik);

            Assert.AreEqual(37, tik.Uiks.Count);

            Test1(tik,                          "Алейская", 14137, 0, 8934, 996, 8934, 996, 9850, 80);
            Test1(tik.Uiks[0],                  "УИК №517", 577, 0, 344, 26, 344, 26, 368, 2);
            Test1(tik.Uiks[tik.Uiks.Count - 1], "УИК №553", 469, 0, 320, 35, 320, 35, 350, 5);
        }

        private void Test1(IElectItem item, string name,
            int numberOfVoters, int numberOfEarlier, int numberOfInside, int numberOfOutside, int stationary, int portable, int valid, int inValid)
        {
            Assert.AreEqual(numberOfVoters, item.NumberOfVoters);

            Assert.AreEqual(name, item.Name);

            Assert.AreEqual(numberOfEarlier, item.NumberOfEarlier);
            Assert.AreEqual(numberOfInside, item.NumberOfInside);
            Assert.AreEqual(numberOfOutside, item.NumberOfOutside);

            Assert.AreEqual(stationary, item.Stationary);
            Assert.AreEqual(portable, item.Portable);

            Assert.AreEqual(valid, item.Valid);
            Assert.AreEqual(inValid, item.InValid);
        }
    }
}
