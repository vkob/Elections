using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Elections
{
   public class Consts
   {
      #region External

      public const string LocalPath = @"..\..\..\";
      public const string TopPath = @"..\..\..\..\";

      public const string Regions = "Regions";
      public const string Upload = "Upload";
      public const string Graphics = "Graphics";
      public const string Uiks = "Uiks";
      public const string Iks = "IKs";
      public const string AllIks = "AllIks";
      public const string AllRegions = "AllRegions";
      public const string Files = "Files";
      public const string Images = "Images";
      public const string Zip = "Zip";

      public const string JpgZip = "Images{0}{1}";
      public const string JpgZipIrr = "Images{0}{1}Irr";
      public const string TxtZip = "Txt{0}{1}";
      public const string XlsZip = "Xls{0}{1}";

      public static string ResultsPath = Path.Combine(TopPath, "Results");
      public static string UpdatePath = Path.Combine(TopPath, Upload);
      public static string ToZipPath = Path.Combine(TopPath, "ToZip");

      public static string GraphicsPath = Path.Combine(UpdatePath, Graphics);
      public static string AllIksPath = Path.Combine(UpdatePath, AllIks);
      public static string AllRegionsPath = Path.Combine(UpdatePath, AllRegions);
      public static string ZipPath = Path.Combine(UpdatePath, Zip);

      public static string LocalPathDumaResults = Path.Combine(ResultsPath, Consts.ResultsDuma);
      public static string LocalPathPresidentResults = Path.Combine(ResultsPath, Consts.ResultsPresident);
      public static string LocalPathAstrahanResults = Path.Combine(ResultsPath, Consts.ResultsAstrahan);

      #endregion

      public const string LocalCommitteeLong = "сайт избирательной комиссии субъекта Российской Федерации";
      public const string LocalCommittee = @"СИЗКСРФ";

      public const string ResultsDuma = "ResultsDuma";
      public const string ResultsPresident = "ResultsPresident";
      public const string ResultsAstrahan = "ResultsAstrahan";

      public const string MainHtmlFileName = "all";

      public const string ExtHtml = ".html";
      public const string PatthernExtHtml = "*.html";

      public const string PatternExtJpg = "*{0}.jpg";
      public const string PatternExtTxt = "*{0}.txt";
      public const string PatternExtXls = "*{0}.xls";
      public const string PatternExtCommonHtml = "*{0}.html";

      public const string PatternExt2003Xls = "*2003.xls";
      public const string PatternExt2007Xls = "*2007.xls";
      public const string PatternExt2011Xls = "*2011.xls";

      public const string PatternExt2004Xls = "*2004.xls";
      public const string PatternExt2008Xls = "*2008.xls";
      public const string PatternExt2009Xls = "*2009.xls";
      public const string PatternExt2012Xls = "*2012.xls";

      public const string PatternExt2007Jpg = "*2007.jpg";
      public const string PatternExt2011Jpg = "*2011.jpg";

      public const string DumaExtremeResults = "Duma{0}.txt";
      public const string PresidentExtremeResults = "President{0}.txt";

      public const string Ending2003Html = "2003.html";
      public const string Ending2007Html = "2007.html";
      public const string Ending2011Html = "2011.html";

      public const string Ending2004Html = "2004.html";
      public const string Ending2008Html = "2008.html";
      public const string Ending2012Html = "2012.html";

      public const string Ending2003Xls = "2003.xls";
      public const string Ending2007Xls = "2007.xls";
      public const string Ending2011Xls = "2011.xls";

      public const string Ending2004Xls = "2004.xls";
      public const string Ending2008Xls = "2008.xls";
      public const string Ending2012Xls = "2012.xls";

       #region FooData Constants

      public static FooData[] DumaFooData2003 = new[]
                          {
                             new FooData() { RussianShort = "ЕР", Color = "white", IsMain = true, RussianLong = "Единая Россия" , Result =37.56},
                             new FooData() { RussianShort = "КПРФ", Color = "red", IsMain = false, RussianLong = "КПРФ" , Result =12.61},
                             new FooData() { RussianShort = "ЛДПР", Color = "blue", IsMain = false, RussianLong = "ЛДПР", Result = 11.45},
                             new FooData() { RussianShort = "Родина", Color = "yellow", IsMain = false, RussianLong = "Родина" , Result =9.02},
                             new FooData() { RussianShort = "Я", Color = "green", IsMain = false, RussianLong = "Яблоко" , Result =0},
                             //////
                             new FooData() { RussianShort = "АПР", Color = "green", IsMain = false, IsHiddenForIks = true, RussianLong = "АПР" , Result = 3.63},
                             new FooData() { RussianShort = "РППиПСС", Color = "white", IsMain = false, IsHiddenForIks = true, RussianLong = "РППиПСС" , Result =0},
                             new FooData() { RussianShort = "СПС", Color = "white", IsMain = false, IsHiddenForIks = true, RussianLong = "СПС" , Result =0},
                          };

      public static FooData[] DumaFooData2007 = new[]
                          {
                             new FooData() { RussianShort = "ЕР", Color = "white", IsMain = true, RussianLong = "Единая Россия" , Result =64.30},
                             new FooData() { RussianShort = "КПРФ", Color = "red", IsMain = false, RussianLong = "КПРФ" , Result =11.57},
                             new FooData() { RussianShort = "СР", Color = "yellow", IsMain = false, RussianLong = "СР" , Result =7.74},
                             new FooData() { RussianShort = "ЛДПР", Color = "blue", IsMain = false, RussianLong = "ЛДПР", Result = 8.14},
                             new FooData() { RussianShort = "Я", Color = "green", IsMain = false, RussianLong = "Яблоко", Result = 1.59},
                          };

      public static FooData[] DumaFooData2011 = new[]
                          {
                             new FooData() { RussianShort = "ЕР", Color = "white", IsMain = true, RussianLong = "Единая Россия" , Result =49.32},
                             new FooData() { RussianShort = "КПРФ", Color = "red", IsMain = false, RussianLong = "КПРФ" , Result =19.19},
                             new FooData() { RussianShort = "СР", Color = "yellow", IsMain = false, RussianLong = "СР" , Result =13.24},
                             new FooData() { RussianShort = "ЛДПР", Color = "blue", IsMain = false, RussianLong = "ЛДПР", Result = 11.67},
                             new FooData() { RussianShort = "Я", Color = "green", IsMain = false, RussianLong = "Яблоко", Result = 3.43},
                          };

      public static FooData[] PresFooData2004 = new[]
                        {
                           new FooData() {RussianShort = "Путин", Color = "", IsMain = true, RussianLong = "Путин В.В.", Result =71.31},
                           new FooData() {RussianShort = "Харитонов", Color = "red", IsMain = false, RussianLong = "Харитонов Н.М.", Result =13.69},
                           new FooData() {RussianShort = "Глазьев", Color = "yellow", IsMain = false, RussianLong = "Глазьев Г.Ю.", Result =4.10},
                           new FooData() {RussianShort = "Хакамада", Color = "blue", IsMain = false, RussianLong = "Хакамада И.М.", Result =3.84},
                        };

      public static FooData[] PresFooData2008 = new[]
                        {
                           new FooData() {RussianShort = "Медведев", Color = "", IsMain = true, RussianLong = "Медведев Д.А.", Result =70.28},
                           new FooData() {RussianShort = "Зюганов", Color = "red", IsMain = false, RussianLong = "Зюганов Г.А.", Result =17.72},
                           new FooData() {RussianShort = "Жириновский", Color = "blue", IsMain = false, RussianLong = "Жириновский В.В.", Result =9.35},
                           new FooData() {RussianShort = "Богданов", Color = "yellow", IsMain = false, RussianLong = "Богданов А.В.", Result =1.30},
                        };

      public static FooData[] PresFooData2012 = new[]
                        {
                           new FooData() {RussianShort = "Путин", Color = "", IsMain = true, RussianLong = "Путин В.В.", Result = 63.6},
                           new FooData() {RussianShort = "Зюганов", Color = "red", IsMain = false, RussianLong = "Зюганов Г.А.", Result = 17.18},
                           new FooData() {RussianShort = "Прохоров", Color = "green", IsMain = false, RussianLong = "Прохоров М.Д.", Result = 7.98},
                           new FooData() {RussianShort = "Жириновский", Color = "blue", IsMain = false, RussianLong = "Жириновский В.В.", Result = 6.22},
                           //////
                           new FooData() {RussianShort = "Миронов", Color = "yellow", IsMain = false,  IsHiddenForIks = true, RussianLong = "Миронов А.В.", Result = 3.86},
                        };

      public static FooData[] AstrahanFooData2009 = new[]
                        {
                           new FooData() {RussianShort = "Боженов", Color = "", IsMain = true, RussianLong = "Боженов С.А.", Result = 68.03},
                           new FooData() {RussianShort = "Шеин", Color = "red", IsMain = false, RussianLong = "Шеин О.В.", Result = 23.88},
                        };

      public static FooData[] AstrahanFooData2012 = new[]
                        {
                           new FooData() {RussianShort = "Столяров", Color = "", IsMain = true, RussianLong = "Столяров М.Н.", Result = 60.00},
                           new FooData() {RussianShort = "Шеин", Color = "red", IsMain = false, RussianLong = "Шеин О.В.", Result = 29.96},
                        };      

      #endregion

      public static readonly ElectionYear ElectionYear2003 = new ElectionYear(ElectionType.Duma, 2003, 55.75, DumaLink2003, DumaFooData2003);
      public static readonly ElectionYear ElectionYear2007 = new ElectionYear(ElectionType.Duma, 2007, 63.71, DumaLink2007, DumaFooData2007);
      public static readonly ElectionYear ElectionYear2011 = new ElectionYear(ElectionType.Duma, 2011, 60.2, DumaLink2011, DumaFooData2011);

      public static readonly ElectionYear ElectionYear2004 = new ElectionYear(ElectionType.President, 2004, 64.38, PresidentLink2004, PresFooData2004);
      public static readonly ElectionYear ElectionYear2008 = new ElectionYear(ElectionType.President, 2008, 69.70, PresidentLink2008, PresFooData2008);
      public static readonly ElectionYear ElectionYear2012 = new ElectionYear(ElectionType.President, 2012, 65.34, PresidentLink2012, PresFooData2012);

      public static readonly ElectionYear ElectionAstrahan2009 = new ElectionYear(ElectionType.Astrahan, 2009, 0, AstrahanLink2009, AstrahanFooData2009);
      public static readonly ElectionYear ElectionAstrahan2012 = new ElectionYear(ElectionType.Astrahan, 2012, 0, AstrahanLink2012, AstrahanFooData2012);
 
      public const string ElectionsDir = "Elections";

      private const string DumaLink2003 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100095621&vrn=100100095619&region=0&global=1&sub_region=0&prver=0&pronetvd=0&vibid=100100095621&type=233";
      private const string DumaLink2007 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100021960186&vrn=100100021960181&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=100100021960186&type=233";
      private const string DumaLink2011 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100028713304&vrn=100100028713299&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=100100028713304&type=233";

      private const string PresidentLink2004 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=1001000882951&vrn=1001000882950&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=1001000882951&type=227";
      private const string PresidentLink2008 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100022249920&vrn=100100022176412&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=100100022249920&type=227";
      private const string PresidentLink2012 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100031793509&vrn=100100031793505&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=100100031793509&type=227";

      private const string AstrahanLink2009 = @"http://www.astrakhan.vybory.izbirkom.ru/region/region/astrakhan?action=show&root=1&tvd=430422094012&vrn=430422093999&region=30&global=&sub_region=30&prver=0&pronetvd=null&vibid=430422094012&type=222";
      private const string AstrahanLink2012 = @"http://www.astrakhan.vybory.izbirkom.ru/region/region/astrakhan?action=show&root=1&tvd=4304220103086&vrn=4304220103082&region=30&global=&sub_region=0&prver=0&pronetvd=null&vibid=4304220103086&type=222";
       public const string Ending2009Txt = "2009.txt";
   }
}
