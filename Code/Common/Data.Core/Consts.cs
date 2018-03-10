using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Data.Core
{
    public class Consts
    {
        public const string Ending2003Txt = "2003.txt";
        public const string Ending2007Txt = "2007.txt";
        public const string Ending2011Txt = "2011.txt";
        public const string Ending2016Txt = "2016.txt";

        public const string Ending2004Txt = "2004.txt";
        public const string Ending2008Txt = "2008.txt";
        public const string Ending2012Txt = "2012.txt";
        public const char Tab = '	';

        public const string ResultsDuma = "ResultsDuma";
        public const string ResultsPresident = "ResultsPresident";
        public const string TopPath = @"..\..\..\..\";
        public static string ResultsPath = Path.Combine(TopPath, "Results");
        public const string LocalCommittee = @"СИЗКСРФ";

        public const int Year2003 = 2003;

        public static List<int> Duma = new List<int>{ Year2003, 2007, 2011, 2016 };
        public static List<int> President = new List<int> { 2004, 2008, 2012 };
    }
}
