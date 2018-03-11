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

        public const string ElectionsHtml = "Elections.Html.";

        public const string TitleHolder = "#Tittle#";

        public static string Middle(int lastYear, string caption, string name, string englishShort, string uiks)
        {
            var sbMain = new StringBuilder();
            sbMain.AppendFormat("<tr>\n");
            sbMain.AppendFormat("<td class=\"name\"><h3>{0}</h3></td>\n", lastYear);
            sbMain.AppendFormat("<td class=\"name\"><h3>{0}</h3></td>\n", caption);
            sbMain.AppendFormat("<td class=\"name\">{0}</td>\n",
                "<a href=\"" + Consts.Regions + lastYear + "/RegionsByContribution.html" + $"\"><h3>Результаты по регионам, {lastYear} год</h3></a>\n"
                +
                "<a href=\"" + Consts.Iks + lastYear + "/" + string.Format(name, englishShort) + $"\"><h3>Результаты по избирательным комиссиям, {lastYear} год</h3></a>\n" +
                uiks);

            sbMain.AppendFormat("</tr>\n");

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
                    "<td class=\"header\" style=\"{ text-align: center;}\"><h2>Год</h2></td>\n" +
                    "<td class=\"header\" style=\"{ text-align: center;}\"><h2>Выборы</h2></td>\n" +
                    "<td class=\"header\" style=\"{ text-align: center;}\"><h2>Результаты</h2></td>\n");
                sw.WriteLine(stringBuilder);
                sw.WriteLine("</table>");
                sw.WriteLine("</html>");
            }
        }

        private static string GetFromResource(string fileName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ElectionsHtml + fileName))
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
            stringBuilder.AppendFormat("<td class=\"name\"\">\n<h3>Итого</h3></td>\n");
            var cell = new StringBuilder();
            cell.AppendFormat("<h3>Сводная таблица результатов выборов<br> Партии Власти и Президента за все года</h3>");
            cell.AppendFormat(
                "<h4><a href=\"{0}\">1) по Регионам<a/>\n<br>\n",
                string.Format("{0}/{1}{2}.html", Consts.AllRegions, AllRegionsBy, "Name"));
            cell.AppendFormat(
                "<a href=\"{0}\">2) по Избирательным Коммиссиям<a/>\n<br>\n</h4>\n",
                string.Format("{0}/{1}{2}.html", Consts.AllIks, AllIksBy, "Name"));
            cell.AppendFormat("<h3><a href=\"{0}.html\">Графики-Гауссианы всех результатов выборов и явки</a></h3>", Consts.Graphics);
            stringBuilder.AppendFormat("<td class=\"name\">\n{0}</td>\n", cell);
            stringBuilder.AppendFormat("</tr>");
            return stringBuilder;
        }
    }
}
