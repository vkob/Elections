using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Get.Html.Xls.Txt
{
    public static class TextProcessFunctions
    {
        public static string GetElectionCommitteeName(string line)
        {
            var search = "<b>Наименование избирательной комиссии</b></td><td>";
            var idx = line.IndexOf(search);
            if (idx > -1)
            {
                var end = line.Substring(idx + search.Length).Replace("&ndash;", "-");
                var idxTd = end.IndexOf("</td>");
                var name = end.Substring(0, idxTd);
                return name;
            }

            return line;
        }
    }
}
