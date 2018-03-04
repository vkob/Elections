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
            Assert.AreEqual("Агинский Бурятский автономный округ, Агинская",
                TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2003,
                    @"ResultsDuma\Агинский Бурятский автономный округ\Агинский Бурятский\Агинская\СИЗКСРФ\Агинская 2003.xls"));

            Assert.AreEqual("Архангельская область, Архангельск, Октябрьская",
                            TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2003,
                    @"ResultsDuma\Архангельская область\Архангельский\Архангельск, Октябрьская\СИЗКСРФ\Архангельск, Октябрьская 2003.xls"));

            /////////////////////
            
            Assert.AreEqual("Камчатский край, Петропавловск-Камчатская городская (судовая)",
                            TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2007,
                                                                        @"ResultsDuma\Камчатский край\Петропавловск-Камчатская городская (судовая)\СИЗКСРФ\петропавловск-камчатская городская (судовая) 2007.xls"));
            Assert.AreEqual("Территория за пределами РФ",
                            TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2007,
                                @"ResultsDuma\Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ 2007.xls"));

            ////////////////////
            
            Assert.AreEqual("Республика Северная Осетия - Алания, ТИК Пригородного района",
                TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2011,
                    @"ResultsDuma\Республика Северная Осетия - Алания\ТИК Пригородного района\СИЗКСРФ\ТИК Пригородного района 2011.xls"));
        }

        [Test]
        public void TestGetLocationGlobalLocal()
        {
            Assert.AreEqual("Ханты-Мансийский автономный округ, Сургутская",
                            TextProcessFunctions.GetElectionCommitteeName(@"Ханты-Мансийский автономный округ\Ханты-Мансийский\Сургутская\сургутская 2003.html", 2003));
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
    }
}
