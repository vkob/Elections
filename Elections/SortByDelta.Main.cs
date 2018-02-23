using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Elections.Utility;

namespace Elections
{
   partial class SortByDelta
   {
      public void Main()
      {
         var sbMain = new StringBuilder();
         var sbGraphics = new StringBuilder(); 

         bool needOutput = false;

         Pair<string, string> res = null;

         var stopwatch = new Stopwatch();
         stopwatch.Start();

         //2003
         res = Start(needOutput, new[] { Consts.ElectionYear2003 }, null, false);
         sbMain.Append(res.First);
         if (res != null) sbGraphics.Append(res.Second);

         //2004
         res = Start(needOutput, new[] { Consts.ElectionYear2004 }, null, false);
         sbMain.Append(res.First);
         if (res != null) sbGraphics.Append(res.Second);

         //2007
         res = Start(needOutput, new[] { Consts.ElectionYear2007 }, null, false);
         sbMain.Append(res.First);
         if (res != null) sbGraphics.Append(res.Second);

         //2008
         res = Start(needOutput, new[] { Consts.ElectionYear2004, Consts.ElectionYear2008 }, null, false);
         sbMain.Append(res.First);
         if (res != null) sbGraphics.Append(res.Second);

         //2009
         res = Start(needOutput, new[] { Consts.ElectionAstrahan2009 }, Consts.ElectionAstrahan2009, false);
         sbMain.Append(res.First);
         if (res != null) sbGraphics.Append(res.Second);

         //2011
         res = Start(needOutput, new[] { Consts.ElectionYear2007, Consts.ElectionYear2011 }, Consts.ElectionYear2007, true);
         sbMain.Append(res.First);
         if (res != null) sbGraphics.Append(res.Second);

         //2012
         res = Start(needOutput, new[] { Consts.ElectionAstrahan2012, Consts.ElectionAstrahan2009 }, Consts.ElectionAstrahan2009, false);
         sbMain.Append(res.First);
         if (res != null) sbGraphics.Append(res.Second);

         //2012
         res = Start(needOutput, new[] { Consts.ElectionYear2003, Consts.ElectionYear2004, Consts.ElectionYear2007, Consts.ElectionYear2008, Consts.ElectionYear2011, Consts.ElectionYear2012 },
            Consts.ElectionYear2004, true);
         sbMain.Append(res.First);
         if (res != null) sbGraphics.Append(res.Second);

         GenerateResult(sbMain, sbGraphics);

         stopwatch.Stop();
         Trace.WriteLine(String.Format("Generated HTML-s {0}", stopwatch.Elapsed));
      }

      public void MainSum()
      {
         var needOutput = true;
         var sbGraphics = new StringBuilder();
         var sbMain = new StringBuilder();

         Pair<string, string> res;

         ////2003
         //res = Start(needOutput, new[] { Consts.ElectionYear2003 }, null, new[] { "ER", "KPRF", "LDPR", "Rodina", "YA" }, false);
         //sbGraphics.Append(res.Second);
         //sbMain.Append(res.First);

         //2012
         res = Start(needOutput, new[] { Consts.ElectionYear2012 }, Consts.ElectionYear2004, true);
         sbGraphics.Append(res.Second);
         sbMain.Append(res.First);

         ////2011
         //var res = Start(needOutput, new[] { Consts.ElectionYear2011 }, null, DumaFooData2011, new[] { "ER", "KPRF", "SR", "LDPR" }, false);
         //sbGraphics.Append(res.Second);
         //sbMain.Append(res.First);

         GenerateResult(sbMain, sbGraphics);
      }

      public void DominantForIks()
      {
         StartDominantForIks(new[]
                          {
                             Consts.ElectionYear2012, 
                             Consts.ElectionYear2011, 
                             Consts.ElectionYear2008, 
                             Consts.ElectionYear2007, 
                             Consts.ElectionYear2004, 
                             Consts.ElectionYear2003
                          });
      }

      public void DominantForRegions()
      {
         StartDominantForRegions(new[]
                          {
                             Consts.ElectionYear2012, 
                             Consts.ElectionYear2011, 
                             Consts.ElectionYear2008, 
                             Consts.ElectionYear2007, 
                             Consts.ElectionYear2004, 
                             Consts.ElectionYear2003
                          });
      }

      private void GenerateResult(StringBuilder sbMain, StringBuilder sbGraphics)
      {
         GenerateHtmlWithGraphics(sbGraphics);
         sbMain = GenerateHtmlMainFooter(sbMain, Consts.ElectionYear2012.FooData.First(f => f.IsMain), Consts.ElectionYear2012.Year);
         GenerateHtmlMain(sbMain);        
      }

      private StringBuilder GenerateHtmlMainFooter(StringBuilder stringBuilder, FooData mainFoo, int year)
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

      private void GenerateHtmlMain(StringBuilder stringBuilder)
      {
         using (var sw = new StreamWriter(Path.Combine(Consts.UpdatePath, "index.html"), false, Encoding.GetEncoding(1251)))
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

      private string GetFromResource(string fileName)
      {
         using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(YandexScripts + fileName))
         using (var streamReader = new StreamReader(stream, Encoding.GetEncoding(1251)))
         {
            var head = streamReader.ReadToEnd();
            return head;
         }
      }

      private string GetHead()
      {
         return GetFromResource("Head.htm");
      }
        
      private void GenerateHtmlWithGraphics(StringBuilder stringBuilder)
      {
         using (var sw = new StreamWriter(Path.Combine(Consts.UpdatePath, "Graphics.html"), false, Encoding.GetEncoding(1251)))
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
   }
}
