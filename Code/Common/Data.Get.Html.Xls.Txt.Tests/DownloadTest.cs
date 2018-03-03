using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Data.Get.Html.Xls.Txt.Tests
{
   [TestFixture]
   public class TestDownload
   {
      private readonly Download download = new Download();

      [Test]
      public void Test()
      {
         using (var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("Data.Get.Html.Xls.Txt.Tests.TestXlsLoad.txt"))
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

            Trace.WriteLine(Download.FindRegionXlsHRef(text2));
            Assert.AreEqual(
               "http://www.altai_terr.vybory.izbirkom.ru/servlet/ExcelReportVersion?region=22&sub_region=22&root=222000041&global=true&vrn=100100028713299&tvd=2222000423902&type=233&vibid=2222000423902&condition=&action=show&version=null&prver=0&sortorder=0",
               Download.FindRegionXlsHRef(text1));
            Assert.AreEqual(
               "http://www.adygei.vybory.izbirkom.ru/servlet/ExcelReportVersion?region=1&sub_region=1&root=12000001&global=true&vrn=100100021960181&tvd=2012000128014&type=233&vibid=2012000128014&condition=&action=show&version=null&prver=0&sortorder=0",
               Download.FindRegionXlsHRef(text2));
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

         //_
         Assert.AreEqual("Тамбовская_177", 
                        download.GetName1(
                                  "<a style=\"text-decoration: none\" href=\"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&amp;tvd=100100067796087&amp;vrn=100100067795849&amp;region=0&amp;global=1&amp;sub_region=0&amp;prver=0&amp;pronetvd=0&amp;vibid=2682000507746&amp;type=233\">Тамбовская_177</a></nobr>"));
         //ё
         Assert.AreEqual("Звёздная городская",
              download.GetName1(
                  "<a style=\"text-decoration: none\" href=\"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&amp;tvd=100100067796150&amp;vrn=100100067795849&amp;region=0&amp;global=1&amp;sub_region=0&amp;prver=0&amp;pronetvd=0&amp;vibid=25920001184764&amp;type=233\">Звёздная городская</a></nobr>"));


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
         Assert.AreEqual("Всеволожский", download.GetSpecialName("100 Всеволожский", Download.ElectionYear2003));
         Assert.AreEqual("Шебалинская", download.GetSpecialName("11 Шебалинская", Download.ElectionYear2003));
         Assert.AreEqual("Горно-Алтайская городская", download.GetSpecialName("1 Горно-Алтайская городская", Download.ElectionYear2003));
      }
   }
}
