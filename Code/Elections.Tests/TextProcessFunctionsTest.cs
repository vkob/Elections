using System.IO;
using Elections.Utility;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    public class TextProcessFunctionsTest
    { 
        [Test]
        public void TestGetLocation()
        {
            Assert.AreEqual("Агинский Бурятский автономный округ, Агинская", TextProcessFunctions.GetElectionCommitteeName
                (@"ResultsDuma\Агинский Бурятский автономный округ\Агинский Бурятский\Агинская\СИЗКСРФ\Агинская 2003.xls"));

            Assert.AreEqual("Архангельская область, Архангельск, Октябрьская", TextProcessFunctions.GetElectionCommitteeName
                (@"ResultsDuma\Архангельская область\Архангельский\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская 2003.xls"));

            /////////////////////
            
            Assert.AreEqual("Камчатский край, Петропавловск-Камчатская городская (судовая)", TextProcessFunctions.GetElectionCommitteeName
                (@"ResultsDuma\Камчатский край\Петропавловск-Камчатская городская (судовая)\СИЗКСРФ\петропавловск-камчатская городская (судовая) 2007.xls"));

            Assert.AreEqual("Территория за пределами РФ", TextProcessFunctions.GetElectionCommitteeName
                (@"ResultsDuma\Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ 2007.xls"));

            ////////////////////
            
            Assert.AreEqual("Республика Северная Осетия - Алания, ТИК Пригородного района", TextProcessFunctions.GetElectionCommitteeName
                (@"ResultsDuma\Республика Северная Осетия - Алания\ТИК Пригородного района\СИЗКСРФ\ТИК Пригородного района 2011.xls"));
        }

        [Test]
        public void TestGetLocation2016()
        {
            Assert.AreEqual("Архангельская область, Архангельск, Октябрьская",
                TextProcessFunctions.GetElectionCommitteeName
                    (@"ResultsDuma\ОИК №72\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская 2016.xls", null, TextProcessFunctions.GetMapping()));
        }


        [Test]
        public void GetElectionCommitteeNameTest()
        {
            Assert.AreEqual("Агинский Бурятский автономный округ, Агинская", TextProcessFunctions.GetElectionCommitteeName
                (@"Агинский Бурятский автономный округ\Агинский Бурятский\Агинская\СИЗКСРФ", 2003));

            Assert.AreEqual("Агинский Бурятский автономный округ, Агинская", TextProcessFunctions.GetElectionCommitteeName
                (@"Агинский Бурятский автономный округ\Агинская\СИЗКСРФ", 2007));
        }

        [Test]
        public void TestGetLocationGlobalLocal()
        {
            Assert.AreEqual("Ханты-Мансийский автономный округ, Сургутская", TextProcessFunctions.GetElectionCommitteeName
                (@"ResultsDuma\Ханты-Мансийский автономный округ\Ханты-Мансийский\Сургутская\сургутская 2003.html"));
        }

        [Test]
        public void TestTrasnlit()
        {
            Assert.AreEqual("PetropavlovskKamchatskaya_gorodskaya_sudovaya_Kamchatskij_kraj_2007",
                            TextProcessFunctions.Translit(
                               "Петропавловск-Камчатская городская (судовая) Камчатский край 2007"));
        }

        [Test]
        public void TestGetRegion()
        {
            Assert.AreEqual("Алтайский край", TextProcessFunctions.GetRegion(@"Алтайский край - Рубцовская\Рубцовская городская\СИЗКСРФ"));
            Assert.AreEqual("Город Москва", TextProcessFunctions.GetRegion(@"Город Москва - Царицынская\район Орехово-Борисово Южное\СИЗКСРФ"));
            Assert.AreEqual("Город Санкт-Петербург", TextProcessFunctions.GetRegion(@"Город Санкт-Петербург - Западная\Ломоносовская N 9\СИЗКСРФ"));
        }

        [Test]
        public void TestNormalizedPlace()
        {
            Assert.AreEqual("Алтайский край, Рубцовская городская", TextProcessFunctions.GetNormalizedPlace("Алтайский край - Рубцовская, Рубцовская городская"));
            Assert.AreEqual("Город Москва, район Орехово-Борисово Южное", TextProcessFunctions.GetNormalizedPlace("Город Москва - Царицынская, район Орехово-Борисово Южное"));
            Assert.AreEqual("Город Санкт-Петербург, Московская №19", TextProcessFunctions.GetNormalizedPlace("Город Санкт-Петербург - Западная, Территориальная избирательная комиссия №19"));
        }

        [Test]
        public void TestGetUikNumber()
        {
            Assert.AreEqual("517", TextProcessFunctions.GetUikNumber("УИК №517"));
            Assert.AreEqual("517", TextProcessFunctions.GetUikNumber("УИК 517"));
            Assert.AreEqual("517", TextProcessFunctions.GetUikNumber("УИК517"));
        }

        [Test]
        public void GetMappingTest()
        {
            var dictionary = TextProcessFunctions.GetMapping();
            Assert.AreEqual("Алтайский край - Бийский", dictionary["ОИК №41"]);
        }

        [Test]
        public void NormalizeElectionCommitteeNameTest()
        {
            var textData = TextProcessFunctions.NormalizeElectionCommitteeName(@"ОИК №1\Адыгейская\СИЗКСРФ", 2016);

            Assert.AreEqual("Республика Адыгея (Адыгея), Адыгейская", textData.ElectionCommitteeName);
            Assert.AreEqual("Республика Адыгея (Адыгея)", textData.Region);
            Assert.AreEqual("<a href=\"../Files2016/Respublika_Adigeya_Adigeya_Adigejskaya.html\">Республика Адыгея (Адыгея), Адыгейская</a>", textData.HrefHtmlFile);
            Assert.AreEqual("Respublika_Adigeya_Adigeya_Adigejskaya", textData.Translit);
        }
    }
}
