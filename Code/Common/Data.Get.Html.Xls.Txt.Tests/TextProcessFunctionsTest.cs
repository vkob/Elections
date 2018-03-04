using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Data.Get.Html.Xls.Txt.Tests
{
    [TestFixture]
    public class TextProcessFunctionsTest
    {
        [Test]
        public void GetElectionCommitteeNameTest()
        {
            Assert.AreEqual("Иркутская область - Братский", TextProcessFunctions.GetElectionCommitteeName("<td valign=\"top\" width=\"45%\"><b>Наименование избирательной комиссии</b></td><td>Иркутская область &ndash; Братский</td>"));
        }
    }
}
