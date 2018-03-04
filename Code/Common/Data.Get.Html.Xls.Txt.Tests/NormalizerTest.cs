using System.IO;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Data.Get.Html.Xls.Txt.Tests
{
    [TestFixture]
    public class NormalizerTest
    {
        [Test]
        public void GetDictionaryTest()
        {
            var dir = Path.GetDirectoryName(this.GetType().Assembly.Location);

            var normalizer = new Normalizer();

            var directoryInfo = new DirectoryInfo(dir + @"\..\" + Path.Combine(Core.Consts.ResultsPath, Core.Consts.ResultsDuma));
            var dict = normalizer.GetDictionary(directoryInfo);
            Assert.AreEqual(225, dict.Count);
            Assert.AreEqual("Архангельская область - Архангельский", dict["ОИК №72"]);
        }
    }
}
