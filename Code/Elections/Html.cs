using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elections
{
    public static class Html
    {
        public const string AllIksBy = "AllIksBy";
        public const string AllRegionsBy = "AllRegionsBy";

        public const string YandexScripts = "Elections.YandexScripts.";

        public const string TitleHolder = "#Tittle#";

        public static string T(int lastYear, string caption, string name, string englishShort, string uiks)
        {
            var sbMain = new StringBuilder();
            sbMain.AppendFormat("<tr>\n");
            sbMain.AppendFormat("<td class=\"name\" style=\"{{font-weight:bold;}}\">{0}</td>\n", lastYear);
            sbMain.AppendFormat("<td class=\"name\" style=\"{{font-weight:bold;}}\">{0}</td>\n", caption);
            sbMain.AppendFormat("<td class=\"name\">{0}</td>\n",
                "<a href=\"" + Consts.Regions + lastYear + "/RegionsByContribution.html" + string.Format("\">Результаты по регионам, {0} год</a><br>", lastYear)

                +
                "<a href=\"" + Consts.Iks + lastYear + "/" + string.Format(name, englishShort) +
                string.Format("\">Результаты по избирательным комиссиям, {0} год</a><br>", lastYear) +
                uiks);

            sbMain.AppendFormat("</tr>");

            return sbMain.ToString();
        }

        public static void GenerateHtmlMain(string path, StringBuilder stringBuilder)
        {
            using (var sw = new StreamWriter(Path.Combine(path, "index.html"), false, Encoding.GetEncoding(1251)))
            {
                var title = "Выборы Российской Федерации";
                sw.WriteLine("<html>");
                sw.WriteLine(GetHead().Replace(TitleHolder, title));
                sw.WriteLine("<table align=\"center\"><tr><td style=\"font-size: 22pt;font-weight:bold;\">{0}</td></tr></table><br>", title);
                sw.WriteLine("<table class=\"my\" border=\"1\" bgcolor=\"#ffffff\" cellpadding=\"2\" cellspacing=\"1\" align=\"center\" vspace=\"0\">");
                sw.WriteLine(
                    "<td clas=\"header\" style=\"{ text-align: center;font-weight:bold;}\">Год</td>" +
                    "<td clas=\"header\" style=\"{ text-align: center;font-weight:bold;}\">Выборы</td>" +
                    "<td clas=\"header\" style=\"{ text-align: center;font-weight:bold;}\">Результаты</td>");
                sw.WriteLine(stringBuilder);
                sw.WriteLine("</table>");
                sw.WriteLine("</html>");
            }
        }

        private static string GetFromResource(string fileName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(YandexScripts + fileName))
            using (var streamReader = new StreamReader(stream, Encoding.GetEncoding(1251)))
            {
                var head = streamReader.ReadToEnd();
                return head;
            }
        }

        public static string GetHead()
        {
            return GetFromResource("Head.htm");
        }

        public static void GenerateHtmlWithGraphics(string path, StringBuilder stringBuilder)
        {
            using (var sw = new StreamWriter(Path.Combine(path, "Graphics.html"), false, Encoding.GetEncoding(1251)))
            {
                const string title = "Диаграммы результатов и явки выборов";
                sw.WriteLine(GetHead().Replace(TitleHolder, title));
                sw.WriteLine("<style type=\"text/css\">.tdimg { padding: 0; }</style>");
                sw.WriteLine("<table align=\"center\"><tr><td style=\"font-size: 20pt;font-weight:bold;\">{0}</td></tr></table><br>", title);
                sw.WriteLine("<table align=\"center\"><tr><td style=\"font-size: 13pt;font-weight:bold;\">Нажмите на картинку для загрузки таблицы данных.</td></tr></table><br>");
                sw.WriteLine("<table border=\"1\" align=\"center\" cellpadding=\"10\"");
                sw.WriteLine("{0}", stringBuilder);
                sw.WriteLine("</table>");
                sw.WriteLine(
                    "<table align=\"center\"><tr><td align=\"center\">Диаграмма явки в других странах</td></tr><tr><td align=\"center\"><img src=\"{0}/OtherCountries.png\"></td></tr></table>", Consts.Graphics);
                sw.WriteLine("</html>");
            }
        }

        public static void GenerateResult(string path, StringBuilder sbMain, StringBuilder sbGraphics)
        {
            GenerateHtmlWithGraphics(path, sbGraphics);
            sbMain = GenerateHtmlMainFooter(sbMain);
            GenerateHtmlMain(path, sbMain);
        }

        private static StringBuilder GenerateHtmlMainFooter(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat("<tr>\n");
            stringBuilder.AppendFormat("<td></td>");
            stringBuilder.AppendFormat("<td style=\"{{text-align: left;font-weight:bold;}}\">\nИтого</td>\n");
            var cell = new StringBuilder();
            cell.AppendFormat("Сводная таблица результатов выборов<br> Партии Власти и Президента за все года");
            cell.AppendFormat(
                "<a href=\"{0}\"><br>1) по Регионам<a/>\n<br>\n",
                string.Format("{0}/{1}{2}.html", Consts.AllRegions, AllRegionsBy, "Name"));
            cell.AppendFormat(
                "<a href=\"{0}\">2) по Избирательным Коммиссиям<a/>\n<br>\n<br>\n",
                string.Format("{0}/{1}{2}.html", Consts.AllIks, AllIksBy, "Name"));
            cell.AppendFormat("<a href=\"{0}.html\">Графики-Гауссианы всех результатов выборов и явки</a>", Consts.Graphics);
            stringBuilder.AppendFormat("<td style=\"{{text-align: left;font-weight:bold;}}\">\n{0}</td>\n", cell);
            stringBuilder.AppendFormat("</tr>");
            return stringBuilder;
        }
    }
}
