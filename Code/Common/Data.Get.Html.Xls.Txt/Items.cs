using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;

namespace Data.Get.Html.Xls.Txt
{
    public static class Items
    {

        private const string DumaLink2003 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100095621&vrn=100100095619&region=0&global=1&sub_region=0&prver=0&pronetvd=0&vibid=100100095621&type=233";
        private const string DumaLink2007 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100021960186&vrn=100100021960181&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=100100021960186&type=233";
        private const string DumaLink2011 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100028713304&vrn=100100028713299&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=100100028713304&type=233";
        private const string DumaLink2016 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100067795854&vrn=100100067795849&region=0&global=1&sub_region=0&prver=0&pronetvd=0&vibid=100100067795854&type=233";

        private const string PresidentLink2004 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=1001000882951&vrn=1001000882950&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=1001000882951&type=227";
        private const string PresidentLink2008 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100022249920&vrn=100100022176412&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=100100022249920&type=227";
        private const string PresidentLink2012 = @"http://www.vybory.izbirkom.ru/region/region/izbirkom?action=show&root=1&tvd=100100031793509&vrn=100100031793505&region=0&global=1&sub_region=0&prver=0&pronetvd=null&vibid=100100031793509&type=227";


        public abstract class Item
        {
            public Item(int year, string link)
            {
                Year = year;
                Link = link;
            }

            public int Year { get; set; }
            public string Link { get; set; }

            public abstract string Result { get; }
        }

        public class Duma : Item
        {
            public Duma(int year, string link) : base(year, link)
            {
            }

            public override string Result => Consts.ResultsDuma;
        }

        public class President : Item
        {
            public President(int year, string link) : base(year, link)
            {
            }

            public override string Result => Consts.ResultsPresident;
        }

        public static List<Item> ElectionItems = new List<Item>
        {
            new Duma(2003, DumaLink2003),
            new Duma(2007, DumaLink2007),
            new Duma(2011, DumaLink2011),
            new Duma(2016, DumaLink2016),

            new President(2004, PresidentLink2004),
            new President(2008, PresidentLink2008),
            new President(2012, PresidentLink2012),
        };
    }
}
