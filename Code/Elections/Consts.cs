﻿using System;
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

        public static string UpdatePath = Path.Combine(Data.Core.Consts.TopPath, Upload);
        public static string ToZipPath = Path.Combine(Data.Core.Consts.TopPath, "ToZip");

        public static string GraphicsPath = Path.Combine(UpdatePath, Graphics);
        public static string AllIksPath = Path.Combine(UpdatePath, AllIks);
        public static string AllRegionsPath = Path.Combine(UpdatePath, AllRegions);
        public static string ZipPath = Path.Combine(UpdatePath, Zip);

        public static string LocalPathDumaResults = Path.Combine(Data.Core.Consts.ResultsPath, Data.Core.Consts.ResultsDuma);
        public static string LocalPathPresidentResults = Path.Combine(Data.Core.Consts.ResultsPath, Data.Core.Consts.ResultsPresident);

        #endregion

        public const string PatternExtJpg = "*{0}.jpg";
        public const string PatternExtTxt = "*{0}.txt";
        public const string PatternExtXls = "*{0}.xls";
        public const string PatternExtCommonHtml = "*{0}.html";

        public const string DumaExtremeResults = "Duma{0}.txt";
        public const string PresidentExtremeResults = "President{0}.txt";

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
            new FooData() { RussianShort = "ЕР", Color = "white", IsMain = true, RussianLong = "Единая Россия", Result = 49.32},
            new FooData() { RussianShort = "КПРФ", Color = "red", IsMain = false, RussianLong = "КПРФ" , Result =19.19},
            new FooData() { RussianShort = "СР", Color = "yellow", IsMain = false, RussianLong = "СР" , Result =13.24},
            new FooData() { RussianShort = "ЛДПР", Color = "blue", IsMain = false, RussianLong = "ЛДПР", Result = 11.67},
            new FooData() { RussianShort = "Я", Color = "green", IsMain = false, RussianLong = "Яблоко", Result = 3.43},
        };

        public static FooData[] DumaFooData2016 = new[]
        {
            new FooData() { RussianShort = "ЕР", Color = "white", IsMain = true, RussianLong = "Единая Россия", Result =54.20},
            new FooData() { RussianShort = "КПРФ", Color = "red", IsMain = false, RussianLong = "КПРФ" , Result = 13.34},
            new FooData() { RussianShort = "СР", Color = "yellow", IsMain = false, RussianLong = "СР" , Result = 6.22},
            new FooData() { RussianShort = "ЛДПР", Color = "blue", IsMain = false, RussianLong = "ЛДПР", Result = 13.14},
            new FooData() { RussianShort = "Я", Color = "green", IsMain = false, RussianLong = "Яблоко", Result = 1.99},
        };

        #region President

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

        public static FooData[] PresFooData2018 = new[] 
        {
            new FooData() {RussianShort = "Путин", Color = "", IsMain = true, RussianLong = "Путин В.В.", Result = 76.69},
            new FooData() {RussianShort = "Грудинин", Color = "red", IsMain = false, RussianLong = "Грудинин П.Н.", Result =11.77},
            new FooData() {RussianShort = "Собчак", Color = "green", IsMain = false, RussianLong = "Собчак К.А.", Result = 1.68},
            new FooData() {RussianShort = "Жириновский", Color = "blue", IsMain = false, RussianLong = "Жириновский В.В.", Result = 5.65},
            new FooData() {RussianShort = "Явлинский", Color = "yellow", IsMain = false, RussianLong = "Явлинский Г.А.", Result = 1.05},
        };

        #endregion

        #endregion

        public static readonly ElectionYear ElectionYear2003 = new ElectionYear(ElectionType.Duma, 2003, 55.75, DumaFooData2003);
        public static readonly ElectionYear ElectionYear2007 = new ElectionYear(ElectionType.Duma, 2007, 63.71, DumaFooData2007);
        public static readonly ElectionYear ElectionYear2011 = new ElectionYear(ElectionType.Duma, 2011, 60.2, DumaFooData2011);
        public static readonly ElectionYear ElectionYear2016 = new ElectionYear(ElectionType.Duma, 2016, 47.88, DumaFooData2011);

        public static readonly ElectionYear ElectionYear2004 = new ElectionYear(ElectionType.President, 2004, 64.38, PresFooData2004);
        public static readonly ElectionYear ElectionYear2008 = new ElectionYear(ElectionType.President, 2008, 69.70, PresFooData2008);
        public static readonly ElectionYear ElectionYear2012 = new ElectionYear(ElectionType.President, 2012, 65.34, PresFooData2012);

        public static readonly ElectionYear ElectionYear2018 = new ElectionYear(ElectionType.President, 2018, 67.49, PresFooData2018);

        public const string ElectionsDir = "Elections";

        public const string Ending2009Txt = "2009.txt";

    }
}
