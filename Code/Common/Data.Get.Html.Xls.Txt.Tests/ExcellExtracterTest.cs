﻿using System.IO;
using NUnit.Framework;

namespace Data.Get.Html.Xls.Txt.Tests
{
    [TestFixture]
    class ExcellExtracterTest
    {
        [Test]
        public void SaveXlsToTxtTest()
        {
            var files = new[]  
            {
                @"\..\..\..\..\..\Results\ResultsDuma\ОИК №1\Адыгейская\СИЗКСРФ\Адыгейская 2016",
                @"\..\..\..\..\..\Results\ResultsDuma\ОИК №1\Гиагинская\СИЗКСРФ\Гиагинская 2016",
                @"\..\..\..\..\..\Results\ResultsDuma\ОИК №1\Кошехабльская\СИЗКСРФ\Кошехабльская 2016",

            }; 

            using (var excellExtracter = new ExcellExtracter())
            {
                var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

                foreach (var file in files)
                {
                    var path = dir + file;
                    var txt = path + ".txt";
                    var xls = path + ".xls";

                    File.Delete(txt);
                    excellExtracter.SaveXlsToTxt(new FileInfo(xls));

                    Assert.IsTrue(File.Exists(txt));
                }
            }
        }
    }
}
