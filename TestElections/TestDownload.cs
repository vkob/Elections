using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Elections;
using Elections.Utility;
using NUnit.Framework;

namespace TestElections
{
   [TestFixture]
   public class TestDownload
   {
      private readonly Download download = new Download();

      [Test]
      public void Test()
      {
         using (var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("TestElections.TestXlsLoad.txt"))
         using (var reader = new StreamReader(stream))
         {
            var text1 = reader.ReadToEnd();

            var text2 =
               "window.location.assign(\"http://www.adygei.vybory.izbirkom.ru/servlet/ExcelReportVersion?\"+" +
               "\"region=1&\"+" +
               "\"sub_region=1&\"+" +
               "\"root=12000001&\"+" +
               "						" +
               "\"global=true&\"+" +
               "\"vrn=100100021960181&\"+" +
               "\"tvd=2012000128014&\"+" +
               "\"type=233&\"+" +
               "\"vibid=2012000128014&\"+" +
               "\"condition=&\"+" +
               "\"action=show&\"+" +
               "\"version=null&\"+" +
               "\"prver=0&\"+" +
               "\"sortorder=\"+sortorder);";

            Trace.WriteLine(Elections.Download.FindRegionXlsHRef(text2));
            Assert.AreEqual(
               "http://www.altai_terr.vybory.izbirkom.ru/servlet/ExcelReportVersion?region=22&sub_region=22&root=222000041&global=true&vrn=100100028713299&tvd=2222000423902&type=233&vibid=2222000423902&condition=&action=show&version=null&prver=0&sortorder=0",
               Elections.Download.FindRegionXlsHRef(text1));
            Assert.AreEqual(
               "http://www.adygei.vybory.izbirkom.ru/servlet/ExcelReportVersion?region=1&sub_region=1&root=12000001&global=true&vrn=100100021960181&tvd=2012000128014&type=233&vibid=2012000128014&condition=&action=show&version=null&prver=0&sortorder=0",
               Elections.Download.FindRegionXlsHRef(text2));
         }

      }

      [Test]
      public void TestGetName1()
      {
         //№
         Assert.AreEqual("Территориальная избирательная комиссия №4",
                         download.GetName1(
                            "<nobr><a style=\"TEXT-DECORATION: none\" href=\"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&amp;tvd=100100028713469&amp;vrn=100100028713299&amp;region=0&amp;global=1&amp;sub_region=0&amp;prver=0&amp;pronetvd=null&vibid=2782000259679&type=233\">Территориальная избирательная комиссия №4</a></nobr>"));
         //()
         Assert.AreEqual("Республика Адыгея (Адыгея)",
                         download.GetName1(
                            "<a style=\"TEXT-DECORATION: none\" href=\"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&amp;tvd=100100028713304&amp;vrn=100100028713299&amp;region=0&amp;global=1&amp;sub_region=0&amp;prver=0&amp;pronetvd=null&vibid=100100028713305&type=233\">Республика Адыгея (Адыгея)</a>"));
         //N
         Assert.AreEqual("Невская N5",
                         download.GetName1(
                            "<nobr><a style=\"TEXT-DECORATION: none\" href=\"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&amp;tvd=100100021960349&amp;vrn=100100021960181&amp;region=0&amp;global=1&amp;sub_region=0&amp;prver=0&amp;pronetvd=null&vibid=2782000201532&type=233\">Невская N5</a></nobr>"));
         //,
         Assert.AreEqual("Волгоград, Ворошиловская",
                         download.GetName1(
                            "<nobr><a style=\"TEXT-DECORATION: none\" href=\"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&amp;tvd=100100028713367&amp;vrn=100100028713299&amp;region=0&amp;global=1&amp;sub_region=0&amp;prver=0&amp;pronetvd=null&vibid=2342000459622&type=233\">Волгоград, Ворошиловская</a></nobr>"));
         //.
         Assert.AreEqual("р.п. Кольцово",
                         download.GetName1(
                            "<nobr><a style=\"TEXT-DECORATION: none\" href=\"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&amp;tvd=100100028713407&amp;vrn=100100028713299&amp;region=0&amp;global=1&amp;sub_region=0&amp;prver=0&amp;pronetvd=null&vibid=2542000384547&type=233\">р.п. Кольцово </a></nobr>"));

         //Астрахань
         Assert.AreEqual("Астрахань, Кировская",
                         download.GetName1(
                            "<a style=\"TEXT-DECORATION: none\" href=\"http://www.astrakhan.vybory.izbirkom.ru/region/region/astrakhan?action=show&amp;tvd=4304220103086&amp;vrn=4304220103082&amp;region=30&amp;global=&amp;sub_region=0&amp;prver=0&amp;pronetvd=null&vibid=4304220103176&type=222\">Астрахань, Кировская</a></nobr>"));
      }

      [Test]
      public void TestGetName2()
      {
         Assert.AreEqual("сайт избирательной комиссии субъекта Российской Федерации",
                         download.GetName2(
                            "<a href=\"http://www.vybory.izbirkom.ru/region/izbirkom?action=show&amp;global=true&amp;root=12000001&amp;tvd=2012000183551&amp;vrn=100100028713299&amp;prver=0&amp;pronetvd=null&amp;region=1&amp;sub_region=1&amp;type=233&amp;vibid=2012000183551\">сайт избирательной комиссии субъекта Российской Федерации</a>"));

         Assert.AreEqual("сайт избирательной комиссии субъекта Российской Федерации",
                         download.GetName2(
                            "<a href=\"http://www.vybory.izbirkom.ru/region/izbirkom?action=show&amp;global=true&amp;root=12000001&amp;tvd=201200072303&amp;vrn=100100095619&amp;prver=0&amp;pronetvd=0&amp;region=1&amp;sub_region=1&amp;type=233&amp;vibid=201200072303\">сайт избирательной комиссии субъекта Российской Федерации</a>"));

      }

      [Test]
      public void TestGetName3()
      {
         Assert.AreEqual("191 Автозаводский", download.GetName3("<option value=\"http://www.vybory.izbirkom.ru/region/izbirkom?action=show&amp;global=true&amp;root=1000069&amp;tvd=100100095690&amp;vrn=100100095619&amp;prver=0&amp;pronetvd=0&amp;region=0&amp;sub_region=0&amp;type=233&amp;vibid=100100095690\">191 Автозаводский</option>"));
      }
      
      [Test]
      public void GetSpecialName()
      {
         Assert.AreEqual("Всеволожский", download.GetSpecialName("100 Всеволожский", Consts.ElectionYear2003.Year));
         Assert.AreEqual("Шебалинская", download.GetSpecialName("11 Шебалинская", Consts.ElectionYear2003.Year));
         Assert.AreEqual("Горно-Алтайская городская", download.GetSpecialName("1 Горно-Алтайская городская", Consts.ElectionYear2003.Year));
      }
      
      [Test]
      public void TestGetLocation()
      {
         Assert.AreEqual("Агинский Бурятский автономный округ, Агинская",
                         TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2003,
                                                                     @"W:\VS2010\duma\Elections\Results\ResultsDuma\Агинский Бурятский автономный округ\Агинский Бурятский\Агинская\СИЗКСРФ\Агинская 2003.xls"));

         Assert.AreEqual("Камчатский край, Петропавловск-Камчатская городская (судовая)",
                         TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2007,
                                                                     @"W:\VS2010\duma\Elections\ResultsDuma\Камчатский край\Петропавловск-Камчатская городская (судовая)\СИЗКСРФ\петропавловск-камчатская городская (судовая) 2007.xls"));
         Assert.AreEqual("Республика Северная Осетия - Алания, ТИК Пригородного района",
                         TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2011,
                                                                     @"W:\VS2010\duma\Elections\ResultsDuma\Республика Северная Осетия - Алания\ТИК Пригородного района\СИЗКСРФ\ТИК Пригородного района 2011.xls"));

         Assert.AreEqual("Территория за пределами РФ",
                         TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2007,
                                                                     @"W:\VS2010\duma\Elections\ResultsDuma\Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ 2007.xls"));
         Assert.AreEqual("Территория за пределами РФ",
                         TextProcessFunctions.GetElectionCommitteeName(Consts.ElectionYear2007,
                                                                     @"W:\VS2010\duma\Elections\ResultsDuma\Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ 2007.xls"));
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
      public void TestIrregularity()
      {
         var data = ProcessData.GetElectionDataWithNormalizedPlace("..\\..\\..\\\\Elections\\ElectionInfoDuma\\ElectionInfoDuma2011.xml", 2011, "");
         Assert.AreEqual(39,14, data["город Москва, район Гольяново"].GetFoo("ER").Irregularity);
      }
      
      [Test]
      public void TestGetUikNumber()
      {
         Assert.AreEqual("517", TextProcessFunctions.GetUikNumber("УИК №517"));
         Assert.AreEqual("517", TextProcessFunctions.GetUikNumber("УИК 517"));
         Assert.AreEqual("517", TextProcessFunctions.GetUikNumber("УИК517"));
      }

      [Test]
      public void TestNumbersOfVotedFor()
      {
         var electionsLast = ProcessData.GetElectionDataWithNormalizedPlace("..\\..\\..\\\\Elections\\ElectionInfoPresident\\ElectionInfoPresident2012.xml", 2012, "");
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
