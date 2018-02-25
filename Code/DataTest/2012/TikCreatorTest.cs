using System.IO;
using Data;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2012
{
    [TestFixture]
    public class TikCreatorTest
    {
        [Test]
        public void TikTest()
        {
            var reader = new TikCreator();

            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);
            
            var tik = reader.Create(dir + @"\..\..\..\..\Results\ResultsPresident\Алтайский край\Алейская\СИЗКСРФ\Алейская 2012.txt");
            Assert.IsNotNull(tik);

            Assert.AreEqual(37, tik.Uiks.Count);

            Common.CheckData(tik, new District(){
                Name     = "Алейская",
                NumberOfVoters = 14137,
                NumberOfEarlier = 0,
                NumberOfInside = 8934,
                NumberOfOutside = 996,
                Stationary = 8934,
                Portable = 996,
                Valid = 9850,
                InValid = 80});

            Common.CheckData(tik.Uiks[0], new District()
            {
                Name = "УИК №517",
                NumberOfVoters = 577,
                NumberOfEarlier = 0,
                NumberOfInside = 344,
                NumberOfOutside = 26,
                Stationary = 344,
                Portable = 26,
                Valid = 368,
                InValid = 2
            });

            Common.CheckData(tik.Uiks[tik.Uiks.Count - 1], new District()
            {
                Name = "УИК №553",
                NumberOfVoters = 469,
                NumberOfEarlier = 0,
                NumberOfInside = 320,
                NumberOfOutside = 35,
                Stationary = 320,
                Portable = 35,
                Valid = 350,
                InValid = 5
            });

            tik.Check();
        }
    }
}
