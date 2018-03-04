using System.IO;
using System.Linq;
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

            var normalizer = new Normalizer2016();

            var directoryInfo = new DirectoryInfo(dir + @"\..\" + Path.Combine(Core.Consts.ResultsPath, Core.Consts.ResultsDuma));
            var dict = normalizer.GetDictionary(directoryInfo);
            Assert.AreEqual(225, dict.Count);
            Assert.AreEqual("Архангельская область - Архангельский", dict["ОИК №72"]);

            using (var sw =
                new StreamWriter(dir + @"\..\..\..\Data.Core\Names2016.txt"))
            {
                foreach (var kvp in dict.OrderBy(kvp => kvp.Value))
                {
                    sw.WriteLine("{0}\t{1}", kvp.Key, kvp.Value);
                }
                
            }
        }
    }
}
