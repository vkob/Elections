using System.IO;
using Data;
using Data.Places;
using NUnit.Framework;

namespace DataTest._2016
{
    [TestFixture]
    public class TikCreatorTest
    {
        [Test]
        public void TikTest()
        {
            var reader = new TikCreator();

            var dir = Path.GetDirectoryName(typeof(TikCreatorTest).Assembly.Location);
            
            var tik = reader.Create(dir + @"\..\..\..\..\Results\ResultsDuma\ОИК №40\Алейская\СИЗКСРФ\Алейская 2016.txt");
            Assert.IsNotNull(tik);

            Assert.AreEqual(37, tik.Uiks.Count);

            Common.CheckData(tik, new District(){
                Name     = "Алейская",
                NumberOfVoters = 13563,
                NumberOfEarlier = 0,
                NumberOfInside = 6982,
                NumberOfOutside = 782,
                Stationary = 6981,
                Portable = 782,
                Valid = 7612,
                InValid = 151
            });

            Common.CheckData(tik.Uiks[0], new District()
            {
                Name = "УИК №554",
                NumberOfVoters = 622,
                NumberOfEarlier = 0,
                NumberOfInside = 333,
                NumberOfOutside = 27,
                Stationary = 333,
                Portable = 27,
                Valid = 351,
                InValid = 9
            });

            Common.CheckData(tik.Uiks[tik.Uiks.Count - 1], new District()
            {
                Name = "УИК №590",
                NumberOfVoters = 466,
                NumberOfEarlier = 0,
                NumberOfInside = 291,
                NumberOfOutside = 26,
                Stationary = 291,
                Portable = 26,
                Valid = 315,
                InValid = 2
            });

            tik.Check();
        }
    }
}
