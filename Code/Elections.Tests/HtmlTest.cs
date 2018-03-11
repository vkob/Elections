using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    class HtmlTest
    {
        [Test]
        public void Test()
        {
            Html.GenerateResult(@"W:\VS\Reps\GitHub\Elections\Upload", new StringBuilder(""), new StringBuilder(""));
        }
    }
}
