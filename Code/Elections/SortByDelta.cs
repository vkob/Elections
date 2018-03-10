using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Data.Core;
using Elections.Diagrams.Graphic;
using Elections.Rows;
using Elections.Utility;
using Elections.XmlProcessing;

namespace Elections
{
   public enum Indexes
   {
      SortedByPrevious = 0,
      SortedByLast = 1,
      SortedByDelta = 2,
      SortedByAlphabet = 3,
      SortedByPresencePrevious = 4,
      SortedByPresenceLast = 5,
      SortedByPresenceDelta = 6,
      SortedByIrregularity = 7,
   }

   public partial class SortByDelta
   {
      private const string TitleHolder = "#Tittle#";

      private const string Counter = "<table align=\"center\">" +
                                     "<tr><td align=\"center\">" +
                                     "Счётчик посещений с 19 февраля 2012 года" +
                                     "<a href=\"http://www.warlog.ru/\" target=\"_blank\"><img border=\"0\" src=\"http://www.warlog.ru/counter/?i=1\" alt=\"счетчик посещений\" title=\"счетчик посещений\" /></a>" +
                                     "</td></tr>" +
                                     "</table>";

      const string AllIksBy = "AllIksBy";
      const string AllRegionsBy = "AllRegionsBy";

      private const string IrregularityHeader = "Неравно-<br>мерность<br>{0}, %";
      private const string Style = "<style type=\"text/css\">" + 
         "table{border-collapse: collapse;}" +
         "td { text-align: right;padding:5px 10px 5px 10px; }" + 
         "td.name { text-align: left}" + 
         "td.header { text-align: center;font-weight:bold;}" + 
         "td.footer { font-weight:bold;}" + 
         "</style>";
      private const string NoData = "нет данных";

      public const string EmbeddedResource = "Elections.EmbeddedResources.";
      public const string YandexScripts = "Elections.YandexScripts.";

      #region Fileds

      private const string email = "RuElect@yandex.ru";

      private string Email =
         string.Format(
            "<tr><td align=\"center\"><a href=\"mailto:{0}?subject=Vibori\" >Свои высказывания, пожелания, предложения по улучшению шлите на {0}</a></td></tr>",
            email);

      private SortedDictionary<string, double> valuesPrevious = new SortedDictionary<string, double>();

      private string[] indexFiles = new string[8];

      private Dictionary<string, int> win = new Dictionary<string, int>();
      private Dictionary<string, FooData> fooDataDict;

      #endregion

      #region Constructor

      public SortByDelta()
      {
         indexFiles[(int) Indexes.SortedByPrevious] = "IzbirKomsByPrevious.html";
         indexFiles[(int)Indexes.SortedByLast] = "IzbirKomsByLast{0}.html";
         indexFiles[(int)Indexes.SortedByDelta] = "IzbirKomsByDelta.html";
         indexFiles[(int)Indexes.SortedByAlphabet] = "IzbirKomsByAlphabet.html";
         indexFiles[(int)Indexes.SortedByPresencePrevious] = "IzbirKomsByPresencePrevious.html";
         indexFiles[(int)Indexes.SortedByPresenceLast] = "IzbirKomsByPresenceLast.html";
         indexFiles[(int)Indexes.SortedByPresenceDelta] = "IzbirKomsByPresenceDelta.html";
         indexFiles[(int)Indexes.SortedByIrregularity] = "IzbirKomsByIrregularity.html";
      }

      #endregion

      #region Public Methods

      public void StartDominantForRegions(ElectionYear[] electionYears)
      {
         electionYears = electionYears.OrderBy(ey => ey.Year).ToArray();
         var electionsFilePathes =
            electionYears
               .Select(ey => Consts.LocalPath +
                             @"\" +
                             Consts.ElectionsDir +
                             @"\" +
                             string.Format(@"{0}\{0}", ey.DirElectionInfo) +
                             ey.Year +
                             ".xml");

         int c = 0;
         var electionsAll = electionsFilePathes
            .Select(ProcessData.GetElectionDataWithNormalizedPlace)
            .ToArray();
         var electionsByRegionAll = electionsAll
            .Select(e => e.GroupBy(kvp => kvp.Value.TextData.Region, kvp => kvp.Value)
            .ToDictionary(g => g.Key, g => g.ToList()))
            .ToArray();

         Directory.CreateDirectory(Consts.AllRegionsPath);

         var allRows = new List<SimpleRow>(electionsByRegionAll[electionYears.Length - 1].Count);
         var mainFoos = electionYears.Select(ey => ey.FooData.First(f => f.IsMain)).ToArray();
         electionsByRegionAll[electionYears.Length - 1].ForEach(kvp =>
         {
            var list = new List<double?>(electionYears.Length);

            for (int i = 0; i < electionYears.Length; i++)
            {
               if (electionsByRegionAll[i].ContainsKey(kvp.Key))
               {
                  var elections = electionsByRegionAll[i][kvp.Key];
                  var numberOfInvalidBallot = elections.Sum(e => e.NumberOfInvalidBallot);
                  var numberOfValidBallot = elections.Sum(e => e.NumberOfValidBallot);
                  var numberVotedFor = NumberVotedFor(electionsByRegionAll[i], kvp.Key, mainFoos[i].EnglishShort);
                  var numberOfVoted = numberOfInvalidBallot + numberOfValidBallot;
                  var percentResult = numberVotedFor * 100.0 / numberOfVoted;
                  list.Add(percentResult);
               }
               else
               {
                  list.Add(null);
               }
            }

            var simpleRows = new SimpleRow() { Values = list, Text = kvp.Key };
            allRows.Add(simpleRows);
         });

         var fileRegionsByName = AllRegionsBy + "{0}.html";

         Action<string, SimpleRow[]> output = (ouputFile, rows) =>
         {
            using (var sw = new StreamWriter(ouputFile, false, Encoding.GetEncoding(1251)))
            {
               const string title = "Сводная таблица результатов выборов Партии Власти и Президента за все года по регионам";
               sw.WriteLine("<html>");
               sw.WriteLine(GetHead().Replace(TitleHolder, title));
               sw.WriteLine(
                  "<table align=\"center\" style=\"font-weight:bold;\">" +
                     "<tr><td><a href=\"../index.html\">На главную</a></td></tr>" +
                  "</table>" +
                  "<table align=\"center\">" +
                     "<tr><td style=\"font-size: 18pt;font-weight:bold;\">{0}</td></tr>" +
                  "</table><br>", title);
               sw.WriteLine("<table class=\"my\" border=\"1\" bgcolor=\"#ffffff\" cellpadding=\"2\" cellspacing=\"1\" align=\"center\" vspace=\"0\">");
               sw.WriteLine("<tr><td class=\"header\">Номер</td><td class=\"header\">Регион</td>");
               for (int j = 0; j < rows[0].Values.Count; j++)
               {
                  sw.WriteLine("<td class=\"header\">{0}</td>",
                     string.Format("<a href=\"{0}\">Результат<br>{1},<br>{2}, %</a>",
                     string.Format(fileRegionsByName, mainFoos[j].EnglishShort + electionYears[j].Year), 
                     mainFoos[j].RussianLong, 
                     electionYears[j].Year));
               }
               sw.WriteLine("</tr>");
               var ci = new CultureInfo("ru-ru");
               for (int i = 0; i < rows.Length; i++)
               {
                  var row = rows[i];
                  sw.WriteLine("<tr{0}>", i % 2 == 0 ? " bgcolor=\"#eeeeee\"" : "");
                  sw.WriteLine("<td>{0}</td>", i + 1);
                  sw.WriteLine("<td class=\"name\">{0}</td>", row.Text);
                  for (int j = 0; j < rows[i].Values.Count; j++)
                  {
                     sw.WriteLine("<td>{0}</td>", row.Values[j].HasValue ? row.Values[j].Value.ToString("N2") : "");
                  }
                  sw.WriteLine("</tr>");
               };
               sw.WriteLine("<tr>");
               sw.WriteLine("<td></td>");
               sw.WriteLine("<td class=\"footer\" style=\"text-align: left;\">Итого:</td>");
               for (int j = 0; j < mainFoos.Length; j++)
               {
                  sw.WriteLine("<td class=\"footer\">{0}</td>", mainFoos[j].Result.ToString("N2"));
               }
               sw.WriteLine("</tr>");
               sw.WriteLine("</table>");
               sw.WriteLine("</html>");
            }
         };

         output(Path.Combine(Consts.AllRegionsPath, string.Format(fileRegionsByName, "Name")), allRows.ToArray());

         for (int j = 0; j < mainFoos.Length; j++)
         {
            var rows = allRows.OrderByDescending(r => r.Values[j]).ToArray();
            output(Path.Combine(Consts.AllRegionsPath, string.Format(fileRegionsByName, mainFoos[j].EnglishShort + electionYears[j].Year)), rows);
         }
      }

      public void StartDominantForIks(ElectionYear[] electionYears)
      {
         electionYears = electionYears.OrderBy(ey => ey.Year).ToArray();
         
         var electionsFilePathes =
            electionYears
               .Select(ey => Consts.LocalPath +
                             @"\" +
                             Consts.ElectionsDir +
                             @"\" +
                             string.Format(@"{0}\{0}", ey.DirElectionInfo) +
                             ey.Year +
                             ".xml");

         int c = 0;
         var electionsAll = electionsFilePathes.Select(ProcessData.GetElectionDataWithNormalizedPlace).ToArray();

         var electionLast = electionsAll[electionsAll.Length - 1];
         var mainFoos = electionYears.Select(ey => ey.FooData.First(f => f.IsMain)).ToArray();
         var maxIrregularity = -1.0;
         foreach (var electionKvp in electionLast)
         {
            for (int i = 0; i < electionsAll.Length; i++)
            {
               var mainFoo = mainFoos[i].EnglishShort;
               var election = electionsAll[i].ContainsKey(electionKvp.Key) ? electionsAll[i][electionKvp.Key] : null;
               if (election != null)
               {
                  var irregularity = election.GetFoo(mainFoo).Irregularity;
                  if (irregularity > maxIrregularity) maxIrregularity = irregularity;
               }
            }
         }
         var d = maxIrregularity;

         var rowsByName = new List<SimpleRow>(electionLast.Count);
         foreach (var electionKvp in electionLast)
         {
            var row = new SimpleRow();
            var values = new List<double?>();
            row.Text = electionKvp.Value.TextData.ElectionCommitteeName;
            for (int i = 0; i < electionsAll.Length; i++)
            {

               var mainFoo = mainFoos[i].EnglishShort;
               var election = electionsAll[i].ContainsKey(electionKvp.Key) ? electionsAll[i][electionKvp.Key] : null;
               if (election != null)
               {
                  values.Add(election.GetFoo(mainFoo).Value);
                  values.Add(election.GetFoo(mainFoo).Irregularity);
               }
               else
               {
                  values.Add(null);
                  values.Add(null);
               }
            }
            row.Values = values;
            rowsByName.Add(row);
         }

         var fileAllByName = AllIksBy + "Name.html";
         Action<string, List<SimpleRow>> outputHtml = (fileName, rows) =>
         {
            using (var sw = new StreamWriter(fileName, false, Encoding.GetEncoding(1251)))
            {
               const string title = "Сводная таблица результатов выборов Партии Власти и Президента за все года по Избирательным Комиссиям";
               sw.WriteLine("<html>");
               sw.WriteLine(GetHead().Replace(TitleHolder, title));
               sw.WriteLine(
                  "<table align=\"center\" style=\"font-weight:bold;\">" +
                     "<tr><td><a href=\"../index.html\">На главную</a></td></tr>" +
                  "</table>" +
                  "<table align=\"center\">" +
                     "<tr><td style=\"font-size: 18pt;font-weight:bold;\">" +
                        "{0}" +
                     "</td></tr>" +
                  "</table><br>", title);
               sw.WriteLine(
                  "<table class=\"my\" border=\"1\" bgcolor=\"#ffffff\" cellpadding=\"2\" cellspacing=\"1\" align=\"center\" vspace=\"0\">");
               var sbHeader = new StringBuilder();
               sbHeader.AppendLine("<tr>");
               sbHeader.AppendFormat("<td class=\"name\" style=\"font-weight:bold;\"><a href=\"{0}\">Избирательная комиссия</a></td>", fileAllByName);
               for (int i = 0; i < mainFoos.Length; i++)
               {
                  sbHeader.AppendFormat("<td class=\"header\"><a href=\"{0}{3}{2}.html\">{1},<br>{2},<br>%</a></td>", AllIksBy, mainFoos[i].RussianLong, electionYears[i].Year, mainFoos[i].EnglishShort);
                  sbHeader.AppendFormat("<td class=\"header\" bgcolor=\"#ffffdd\"><a href=\"{0}{2}Irr{1}.html\">{3}</a></td>", 
                     AllIksBy,
                     electionYears[i].Year, 
                     mainFoos[i].EnglishShort,
                     string.Format(IrregularityHeader, electionYears[i].Year));
               }
               sbHeader.AppendLine("</tr>");
               sw.WriteLine(sbHeader.ToString());
               int cc = 0;
               foreach (var row in rows)
               {
                  cc++;
                  //if (cc == 10) break;
                  var sb = new StringBuilder();
                  sb.AppendFormat("<tr{0}>", cc % 2 == 0 ? " bgcolor=\"#eeeeee\"" : "");
                  var translit = TextProcessFunctions.Translit(row.Text);
                  var hrefHtmlFile = string.Format("<a href=\"../{0}{1}/{2}.html\">{3}</a>", Consts.Files, electionYears[electionYears.Length - 1].Year, translit, row.Text);
                  sb.AppendFormat("<td class=\"name\">{0}</td>", hrefHtmlFile);
                  for (int i = 0; i < row.Values.Count - 1; i = i + 2)
                  {
                     if (row.Values[i].HasValue)
                     {
                        var value = (row.Values[i].Value).ToString("N2");
                        sb.AppendFormat("<td>{0}</td>", value);
                        var irregularity = (row.Values[i + 1].Value);
                        var redColor = ((int) (512.0*irregularity/d));
                        if (redColor > 256) redColor = 255;
                        sb.AppendFormat("<td style=\"color:#{0}0000\" bgcolor=\"#ffffdd\">{1}</td>", redColor.ToString("X2"), irregularity.ToString("N2"));
                     }
                     else
                     {
                        sb.Append("<td></td><td></td>");
                     }
                  }
                  sb.AppendLine("</tr>");
                  sw.Write(sb.ToString());
               }
               sw.WriteLine("</table>");
            }
         };

         Directory.CreateDirectory(Consts.AllIksPath);
         outputHtml(Path.Combine(Consts.AllIksPath, AllIksBy + "Name.html"), rowsByName);
         for (int i = 0; i < mainFoos.Length; i++)
         {
            var rows = rowsByName.OrderByDescending(r => r.Values[2 * i]).ToList();
            outputHtml(Path.Combine(Consts.AllIksPath, string.Format("{0}{1}{2}.html", AllIksBy, mainFoos[i].EnglishShort, electionYears[i].Year)), rows);

            rows = rowsByName.OrderByDescending(r => r.Values[2 * i + 1]).ToList();
            outputHtml(Path.Combine(Consts.AllIksPath, string.Format("{0}{1}Irr{2}.html", AllIksBy, mainFoos[i].EnglishShort, electionYears[i].Year)), rows);
         }
      }

      public Pair<string, string> Start(
         bool needOutput,
         ElectionYear[] electionYears, ElectionYear electionYearPrevious,
         bool calcDiff)
      {
         electionYears = electionYears.OrderByDescending(ey => ey.Year).ToArray();
         var lastElectionYear = electionYears[0];
         var lastYear = lastElectionYear.Year;
         FooData[] lastFooData = lastElectionYear.FooData.Where(f => !f.IsHiddenForIks).ToArray();

         var mainFoo = lastFooData.First(f => f.IsMain);
         //goto met;
         fooDataDict = lastFooData.ToDictionary(f => f.EnglishShort);

         win.Clear();

         win.Add(mainFoo.EnglishShort, 0);
         var otherFoos = lastFooData.Where(f => !f.IsMain).ToDictionary(f => f.EnglishShort);
         otherFoos.ForEach(o => win.Add(o.Value.EnglishShort, 0));

         string what = electionYears[0].DirElectionInfo;

         var resultsPrev = electionYearPrevious != null
                              ? Consts.LocalPath + @"\" + Consts.ElectionsDir + @"\" + what + @"\" + what +
                                electionYearPrevious.Year + ".xml"
                              : null;
         var resultsLast = Consts.LocalPath + @"\" + Consts.ElectionsDir + @"\" + what + @"\" + what +
                           electionYears[0].Year + ".xml";

         var electionsPrev = resultsPrev != null ? ProcessData.GetElectionDataWithNormalizedPlace(resultsPrev) : null;

         var electionsLast = ProcessData.GetElectionDataWithNormalizedPlace(resultsLast);

         var electionsFilePathes =
            electionYears
               .Select(ey => Consts.LocalPath +
                             @"\" +
                             Consts.ElectionsDir +
                             @"\" +
                             string.Format(@"{0}\{0}", ey.DirElectionInfo) +
                             ey.Year +
                             ".xml");

         int c = 0;
         var electionsAll = needOutput 
            ? electionsFilePathes
               .Select(p => ProcessData.GetElectionDataWithNormalizedPlace(p))
               .ToArray()
            : null;

         var listRows = new List<Row>(electionsLast.Count);

         var keys = electionsLast.Keys.ToArray();
         Action<int, int> doAll = (start, end) =>
         {
            for (int i = start; i < end; i++)
            {
               var electionCommitteeName = keys[i];
               var electionCurrent = electionsLast[electionCommitteeName];
               //if (!place.Contains("ольяново")) continue;

               if (needOutput)
                  GenerateRegionHtml(electionCurrent.TextData.Translit, electionCommitteeName, electionCurrent, electionsAll, electionYears, mainFoo);
                  
               var electionPrev = (calcDiff)
                                    ? electionsPrev != null
                                          ? electionsPrev.ContainsKey(electionCommitteeName)
                                             ? electionsPrev[electionCommitteeName]
                                             : null
                                          : null
                                    : null;

               var row = new Row(mainFoo.EnglishShort, otherFoos, electionCurrent.TextData.HrefHtmlFile, electionCurrent.TextData.Region, electionCommitteeName, electionCurrent, electionPrev);
               listRows.Add(row);
            }
         };

         const int n = 1;
         var count = keys.Length/n;
         var threads = new Thread[n];
         for (int i = 0; i < n; i++)
         {
            var start = i*count;
            var end = (i == n - 1) ? keys.Length : start + count;
            threads[i] = new Thread(() => doAll(start, end));
         }
         threads.ForEach(t => t.Start());
         threads.ForEach(t => t.Join());

      met:
         string uiks = "";
         ResultsHtmlElectionCommittees(listRows, otherFoos, mainFoo, electionYears, electionYearPrevious, lastFooData, calcDiff);
         uiks = ResultsHtmlLocalElectionsCommitees(electionsLast, lastElectionYear.FooData, electionYears[0]);
         ResultsHtmlRegions(electionsLast, lastElectionYear, mainFoo);

         var res = GenerateDiagrams(mainFoo, electionYears, lastFooData);
         var sbGraphics = new StringBuilder();

         sbGraphics.AppendFormat("<tr>{0}{1}{2}{3}{4}{5}</tr>", res.First, res.Second, res.Third, res.Fourth, res.Fivth, res.Sixth);


         var sbMain = new StringBuilder();
         sbMain.AppendFormat("<tr>\n");
         sbMain.AppendFormat("<td class=\"name\" style=\"{{font-weight:bold;}}\">{0}</td>\n", lastYear);
         sbMain.AppendFormat("<td class=\"name\" style=\"{{font-weight:bold;}}\">{0}</td>\n", ElectionCaption(lastElectionYear.ElectionType));
         sbMain.AppendFormat("<td class=\"name\">{0}</td>\n",
            "<a href=\"" + Consts.Regions + lastYear + "/RegionsByContribution.html" + string.Format("\">Результаты по регионам, {0} год</a><br>", lastYear) 

            +
            "<a href=\"" + Consts.Iks + lastYear + "/" + string.Format(indexFiles[(int)Indexes.SortedByAlphabet], mainFoo.EnglishShort) + string.Format("\">Результаты по избирательным комиссиям, {0} год</a><br>", lastYear) +
            uiks);
        
         sbMain.AppendFormat("</tr>");
         return new Pair<string, string>(sbMain.ToString(), sbGraphics.ToString());
      }

      #endregion

      #region Private Methods

      private static string ElectionCaption(ElectionType electionType)
      {
          var caption = (electionType == ElectionType.Duma)
              ? "В Думу"
              : "Президента";
                               
         return caption;
      }

      private Six<string, string, string, string, string, string> GenerateDiagrams(
         FooData mainFoo, ElectionYear[] electionYears, FooData[] fooData)
      {
         var caption = ElectionCaption(electionYears[0].ElectionType);
         var first = electionYears[0];
         var href = string.Format("<a href=\"{0}{1}/{2}\">{3}</a>", Consts.Iks, electionYears[0].Year,
                                  string.Format(indexFiles[(int)Indexes.SortedByLast], mainFoo.EnglishShort), caption);

         var threshold = 5.0;
         var foosThreshold = fooData.Where(f => f.Result > threshold).Select(f => f.EnglishShort).ToArray();

         var diagramPresenceByPeople = GraphicPreparer.GenerateGraphic(electionYears[0], foosThreshold, AxisYType.People, DiagramType.Presence);
         var diagramPresenceByUIKs = GraphicPreparer.GenerateGraphic(electionYears[0], foosThreshold, AxisYType.UIK, DiagramType.Presence);

         var diagramResultsByPeople = GraphicPreparer.GenerateGraphic(electionYears[0], foosThreshold, AxisYType.People, DiagramType.Results);
         var diagramResultsByUIKs = GraphicPreparer.GenerateGraphic(electionYears[0], foosThreshold, AxisYType.UIK, DiagramType.Results);

         Func<string, string> getImgHref =
            (name) =>
            string.Format("<a href=\"{0}{1}/{2}\"><img src=\"{3}\"></a>", Consts.Iks, electionYears[0].Year,
                          string.Format(indexFiles[(int)Indexes.SortedByLast], mainFoo.EnglishShort), Consts.Graphics + "/" + name);

         Func<string, string> td = (content) => string.Format("<td>{0}</td>\n", content);
         Func<string, string> tdImg =
            (content) => string.Format("<td align=\"center\" class=\"tdimg\">{0}</td>\n", content);

         return new Six<string, string, string, string, string, string>(
            td(first.Year.ToString()),
            td(href),
            tdImg(getImgHref(diagramResultsByPeople)),
            tdImg(getImgHref(diagramResultsByUIKs)),
            tdImg(getImgHref(diagramPresenceByPeople)),
            tdImg(getImgHref(diagramPresenceByUIKs)));         
      }

      public static int NumberVotedFor(Dictionary<string, List<Election>> electionsByRegion, string region, string foo)
      {
         return electionsByRegion[region].Sum(e => e.GetFoo(foo).Number);
      }

      public static int NumberOfElectorsInList(Dictionary<string, List<Election>> electionsByRegion, string region, string foo)
      {
         return electionsByRegion[region].Sum(e => e.NumberOfElectorsInList);
      }

      private void ResultsHtmlRegions(Dictionary<string, Election> electionsLast, ElectionYear electionYear, FooData mainFoo)
      {
         var electionsByRegion = electionsLast
            .GroupBy(kvp => kvp.Value.TextData.Region, kvp => kvp.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

         var regionsFolder = Path.Combine(Consts.UpdatePath, Consts.Regions + electionYear.Year);
         Directory.CreateDirectory(regionsFolder);

         var allRows = new List<SimpleRow>(electionsByRegion.Count);
         var sumPercentFromAll = 0.0;
         var sumVotedFor = 0.0;
         var sumNumberOfElectorsInList = electionsByRegion.Sum(kvp => kvp.Value.Sum(e => e.NumberOfElectorsInList));
         var sumNumberOfVoted = electionsByRegion.Sum(kvp => kvp.Value.Sum(e => e.NumberOfInvalidBallot)) +
            electionsByRegion.Sum(kvp => kvp.Value.Sum(e => e.NumberOfValidBallot));

         electionsByRegion.ForEach(kvp =>
         {
            var list = new List<double?>();
            var elections = kvp.Value;
            var numberOfElectorsInList = NumberOfElectorsInList(electionsByRegion, kvp.Key, mainFoo.EnglishShort);
            var numberOfInvalidBallot = elections.Sum(e => e.NumberOfInvalidBallot);
            var numberOfValidBallot = elections.Sum(e => e.NumberOfValidBallot);
            var numberVotedFor = NumberVotedFor(electionsByRegion, kvp.Key, mainFoo.EnglishShort);
            var numberOfVoted = numberOfInvalidBallot + numberOfValidBallot;
            var percentResult = numberVotedFor * 100.0 / numberOfVoted;
            var percentFromAll = numberVotedFor * 100.0 / sumNumberOfVoted;
            var presence = numberOfVoted *100.0 /numberOfElectorsInList;

            list.Add(percentResult);
            list.Add(percentFromAll);
            list.Add(numberVotedFor);
            list.Add(numberOfElectorsInList);
            list.Add(presence);

            var simpleRows = new SimpleRow() { Values = list, Text = kvp.Key };
            sumPercentFromAll += percentFromAll;
            sumVotedFor += numberVotedFor;
            allRows.Add(simpleRows);
         });

         var fileRegionsByName = "RegionsByName.html";
         var fileRegionsByResult = "RegionsByResult.html";
         var fileRegionsByContribution = "RegionsByContribution.html";
         var fileRegionsByVoteFor = "RegionsByVoteFor.html";
         var fileRegionsByVotersNumber = "RegionsByVotersNumber.html";
         var fileRegionsByPresence = "RegionsByPresence.html";

         Action<string, SimpleRow[]> output = (ouputFile, rows) =>
         {
            using (var sw = new StreamWriter(ouputFile, false, Encoding.GetEncoding(1251)))
            {
               var title = string.Format("Результаты выборов {0} года по регионам", electionYear.Year);
               sw.WriteLine("<html>");
               sw.WriteLine(GetHead().Replace(TitleHolder, title));
               sw.WriteLine(
                  "<table align=\"center\" style=\"font-weight:bold;\">" + 
                     "<tr><td><a href=\"../index.html\">На главную</a></td></tr>"+
                  "</table>" +
                  "<table align=\"center\">" +
                     "<tr><td style=\"font-size: 20pt;font-weight:bold;\">{0}</td></tr>" +
                  "</table><br>", title);
               sw.WriteLine("<table class=\"my\" border=\"1\" bgcolor=\"#ffffff\" cellpadding=\"2\" cellspacing=\"1\" align=\"center\" vspace=\"0\">");
               sw.WriteLine("<tr><td class=\"header\">{0}</td><td class=\"header\">{1}</td><td class=\"header\">{2}</td><td class=\"header\">{3}</td><td class=\"header\">{4}</td><td class=\"header\">{5}</td><td class=\"header\">{6}</td></tr>",
                  "Номер",
                  string.Format("<a href=\"{0}\">Регион</a>", fileRegionsByName),
                  string.Format("<a href=\"{0}\">Результат<br>{1}<br>%</a>", fileRegionsByResult, mainFoo.RussianLong),
                  string.Format("<a href=\"{0}\">Вклад <br>региона<br>в итоговый <br>результат, %</a>", fileRegionsByContribution),
                  string.Format("<a href=\"{0}\">Количество <br>проголосовавших <br>за</a>", fileRegionsByVoteFor),
                  string.Format("<a href=\"{0}\">Количество<br>избирателей</a>", fileRegionsByVotersNumber),
                  string.Format("<a href=\"{0}\">Явка</a>", fileRegionsByPresence)
                  );
               var ci = new CultureInfo("ru-ru");
               for (int i = 0; i < rows.Length; i++)
               {
                  var row = rows[i];
                  sw.WriteLine("<tr{0}>", i  % 2 ==0 ?  " bgcolor=\"#eeeeee\"" : "");
                  sw.WriteLine("<td>{0}</td>", i + 1);
                  sw.WriteLine("<td class=\"name\">{0}</td>", row.Text);
                  sw.WriteLine("<td>{0}</td>", row.Values[0].Value.ToString("N2"));
                  sw.WriteLine("<td>{0}</td>", row.Values[1].Value.ToString("N2"));
                  sw.WriteLine("<td>{0}</td>", row.Values[2].Value.ToString("N0", ci));
                  sw.WriteLine("<td>{0}</td>", row.Values[3].Value.ToString("N0", ci));
                  sw.WriteLine("<td>{0}</td>", row.Values[4].Value.ToString("N2"));
                  sw.WriteLine("</tr>");
               };
               sw.WriteLine("<tr>");
               sw.WriteLine("<td></td>");
               sw.WriteLine("<td class=\"footer\" style=\"text-align: left;\">Итого:</td>");
               sw.WriteLine("<td class=\"footer\">{0}</td>", mainFoo.Result.ToString("N2"));
               sw.WriteLine("<td class=\"footer\">{0}</td>", sumPercentFromAll.ToString("N2"));
               sw.WriteLine("<td class=\"footer\">{0}</td>", sumVotedFor.ToString("N0", ci));
               sw.WriteLine("<td class=\"footer\">{0}</td>", sumNumberOfElectorsInList.ToString("N0", ci));
               sw.WriteLine("<td class=\"footer\">{0}</td>", electionYear.Presence.ToString("N2"));
               sw.WriteLine("</tr>");
               sw.WriteLine("</table>");
               sw.WriteLine("</html>");
            }
         };

         var rowsByName = allRows.OrderBy(r => r.Text).ToArray();
         output(Path.Combine(regionsFolder, fileRegionsByName), rowsByName);

         var rowsByResult = allRows.OrderByDescending(r => r.Values[0]).ToArray();
         output(Path.Combine(regionsFolder, fileRegionsByResult), rowsByResult);

         var rowsByContribution = allRows.OrderByDescending(r => r.Values[1]).ToArray();
         output(Path.Combine(regionsFolder, fileRegionsByContribution), rowsByContribution);

         var rowsByVoteFor = allRows.OrderByDescending(r => r.Values[2]).ToArray();
         output(Path.Combine(regionsFolder, fileRegionsByVoteFor), rowsByVoteFor);

         var rowsByVotersNumber = allRows.OrderByDescending(r => r.Values[3]).ToArray();
         output(Path.Combine(regionsFolder, fileRegionsByVotersNumber), rowsByVotersNumber);

         var rowsByPresence = allRows.OrderByDescending(r => r.Values[4]).ToArray();
         output(Path.Combine(regionsFolder, fileRegionsByPresence), rowsByPresence);

      }

      private string ResultsHtmlLocalElectionsCommitees(Dictionary<string,Election> elections, FooData[]fooData, ElectionYear electionYear)
      {
         var sbUiks = new StringBuilder("Избир. участки, где в лидерах<br>");
         var output = Path.Combine(Consts.UpdatePath, Consts.Uiks + electionYear.Year);
         Directory.CreateDirectory(output);
         var array = elections.Values.ToArray();
         const double e = 0.001;
         var numberOfUiks = array.Sum(election => election.Foos[0].AllValues.Length);
         for (int counter = 0; counter < fooData.Length; counter++)
         {
            var listRows = new List<SimpleRow>();
            var foo = fooData[counter];
            var name = foo.EnglishShort;
            foreach(var election in array)
            {
               for (int j = 0; j < election.Foos[0].AllValues.Length; j++)
               {
                  var max = election.Foos.Max(f => f.AllValues[j]);
                  if (Math.Abs(max - 0) < e) continue;
                  var fooWithMax = election.Foos.FirstOrDefault(f => Math.Abs(f.AllValues[j] - max) < e && f.Name == name);
                  if (fooWithMax != null)
                  {
                     var simpleRow = new SimpleRow() { Text = election.TextData.ElectionCommitteeName, Number = election.GetUikNumbers()[j]};
                     simpleRow.Values = fooData.Select(f => (double?) election.GetFoo(f.EnglishShort).AllValues[j]).ToList();
                     listRows.Add(simpleRow);
                  }
               }
            }

            Action<string, SimpleRow[]> outputToHtml = (ouputFile, rows) =>
            {
               using (var sw = new StreamWriter(ouputFile, false, Encoding.GetEncoding(1251)))
               {
                  var title = string.Format("Избирательные участки, где в лидерах {0}, {1} год", foo.RussianLong, electionYear.Year);
                  sw.WriteLine("<html>");
                  sw.WriteLine(GetHead().Replace(TitleHolder, title));
                  sw.WriteLine("<table align=\"center\" style=\"font-weight:bold;\">" +
                               "<tr><td><a href=\"../index.html\">На главную</a></td></tr>" +
                               "</table>");
                  sw.WriteLine("<table align=\"center\">" +
                     "<tr><td style=\"font-size: 20pt;font-weight:bold;\">{0}</td></tr>" +
                  "</table><br>", title);
                  sw.WriteLine("<table class=\"my\" border=\"1\" bgcolor=\"#ffffff\" cellpadding=\"2\" cellspacing=\"1\" align=\"center\" vspace=\"0\">");
                  var sbHeader = new StringBuilder();
                  sbHeader.AppendLine("<tr>");
                  sbHeader.AppendFormat("<td class=\"header\">{0}</td>", "Номер");
                  sbHeader.AppendFormat("<td class=\"header\">{0}</td>", "УИК");
                  sbHeader.AppendFormat("<td class=\"header\">{0}</td>", "Название ИК");
                  fooData.ForEach(f => sbHeader.AppendFormat("<td class=\"header\">{0}</td>", f.RussianLong));
                  sbHeader.AppendLine("</tr>");
                  sw.WriteLine(sbHeader.ToString());
                  CultureInfo ci = new CultureInfo("ru-ru");
                  int c = 0;
                  rows.ForEach(r =>
                  {
                     var max = r.Values.Max(v => v).Value;
                     var sb = new StringBuilder();
                     sw.WriteLine("<tr{0}>", c % 2 == 0 ? " bgcolor=\"#eeeeee\"" : "");
                     sb.AppendFormat("<td>{0}</td>", ++c);
                     sb.AppendFormat("<td class=\"name\">УИК №{0}</td>", r.Number);
                     sb.AppendFormat("<td class=\"name\">{0}</td>", r.Text);
                     for (int i = 0; i < r.Values.Count; i++)
                     {
                        sb.AppendFormat("<td {0}>{1:N2}</td>", Math.Abs(r.Values[i].Value - max) < e ? " bgcolor=\"#ffffdd\""  : "",  r.Values[i]);
                     }
                     sb.AppendLine("</tr>");
                     sw.WriteLine(sb.ToString());
                  });
                  sw.WriteLine("</table>");
                  sw.WriteLine("</html>");
               }

            };
            if (listRows.Count > 0)
            {
               var file = Path.Combine(output, string.Format("Uiks{0}.html", name));
               outputToHtml(file, listRows.OrderByDescending(l => l.Values[counter]).ToArray());
               var fi = new FileInfo(file);
               var sizeKB = (double)fi.Length / 1024;
               var sizeMB = sizeKB /1024;
               var sizeValue = sizeKB > 500 ? sizeMB : sizeKB;
               var sizeText = sizeKB > 500 ? "MB" : "KB";
               if (sizeMB < 10)
               {
                  sbUiks.AppendFormat(
                     "<a href=\"" + Consts.Uiks + electionYear.Year + "/Uiks{0}.html" +
                     "\">{1} на {2} из {3} ({4:N0} {5})</a><br>",
                     foo.EnglishShort, foo.RussianLong, listRows.Count, numberOfUiks, sizeValue, sizeText);
               }
               else
               {
                  fi.Delete();
               }
            }
         }
         return sbUiks.ToString();
      }

      private void ResultsHtmlElectionCommittees(
         List<Row> listRows, Dictionary<string, FooData> otherFoos,
         FooData mainFoo, ElectionYear[] electionYears, ElectionYear electionYearPrevious, FooData[] fooData, bool calcDiff) 
      {
         var rowsSortedByResultLast = listRows.OrderByDescending(r => r.ResultLast).ToArray();
         var rowsSortedByResultPrevious = listRows.OrderByDescending(r => r.ResultPrevious).ToArray();
         var rowsSortedByDelta = listRows.OrderByDescending(r => r.Delta).ToArray();
         var rowsSortedByName = listRows.OrderBy(r => r.ElectionCommitteeName).ToArray();
         var rowsSortedByPresencePrevious = listRows.OrderByDescending(r => r.PresencePrevious).ToArray();
         var rowsSortedByPresenceLast = listRows.OrderByDescending(r => r.PresenceLast).ToArray();
         var rowsSortedByPresenceDelta = listRows.OrderByDescending(r => r.PresenceDelta).ToArray();
         var rowsSortedByIrregularity = listRows.OrderByDescending(r => r.Irregularity).ToArray();

         var rowsOtherSortedByResultLast = new Row[otherFoos.Count][];
         int k = 0;
         foreach (var kvp in otherFoos)
         {
            rowsOtherSortedByResultLast[k] = listRows.OrderByDescending(r => r.ElectionLast.GetFoo(kvp.Key).Value).ToArray();
            k++;
         }

         var begin = PrepareHeader(rowsSortedByResultLast, mainFoo, fooData.Where(f => !f.IsMain).ToArray(), electionYears[0], electionYearPrevious, calcDiff);
         var output = Path.Combine(Consts.UpdatePath, Consts.Iks + electionYears[0].Year);
         Directory.CreateDirectory(output);
         Action<Row[], Func<Row, double, string, string, bool>, string[], bool> outputHtml = (rows, func, fileNameAndFooName, isAlphabet) =>
         {
            var fileName = fileNameAndFooName[0];
            var fooName = fileNameAndFooName[1];
            int range = 100;
            var ouputFile = Path.Combine(output, fileName);

            using (var sw = new StreamWriter(ouputFile, false, Encoding.GetEncoding(1251)))
            {
               sw.WriteLine(begin);

               int counter = 0;
               string classAttr = "class=\"same{0}\"";
               for (int i = 0; i < rows.Length; i++)
               {
                  var row = rows[i];
                  var nextRegion = (i < rows.Length - 1) ? rows[i + 1].RegionName : null;

                  bool isSameGroup = false;
                  while (!isSameGroup)
                  {
                     isSameGroup = func(row, range, fooName, nextRegion);
                     if (isSameGroup)
                     {
                        //Trace.WriteLine(row.Name);
                        counter = WriteTR(counter, sw, row, classAttr, mainFoo, otherFoos, electionYearPrevious != null && calcDiff);

                        classAttr = "class=\"same{0}\"";
                     }
                     if (!isSameGroup || i == rows.Length - 1)
                     {
                        if (isAlphabet)
                        {
                           counter = WriteTR(counter, sw, row, classAttr, mainFoo, otherFoos, electionYearPrevious != null && calcDiff);

                           classAttr = "class=\"changed{0}\"";
                           break;
                        }

                        Debug.Assert(range >= Row.NoDelta, "Wrong range");
                        range -= 10;
                        classAttr = "class=\"changed{0}\"";
                     }
                  }
               }

               Debug.Assert(counter == rows.Length, "Wrount count");
               sw.WriteLine();

               sw.WriteLine("</table>");
               sw.WriteLine("</html>");
            }
         };

         Func<Row, double, string, string, bool> funcPrevious =
            (row, range, fooName, nextFileName) => row.ResultPrevious > range - 10 && row.ResultPrevious <= range;

         Func<Row, double, string, string, bool> funcPresencePrevious =
            (row, range, fooName, nextFileName) => row.PresencePrevious > range - 10 && row.PresencePrevious <= range;

         Func<Row, double, string, string, bool> funcLast =
            (row, range, fooName, nextFileName) => (row.ResultLast > range - 10 && row.ResultLast <= range);

         Func<Row, double, string, string, bool> funcOtherLast =
            (row, range, fooName, nextFileName) => (row.ElectionLast.GetFoo(fooName).Value > range - 10 && row.ElectionLast.GetFoo(fooName).Value <= range);

         Func<Row, double, string, string, bool> funcPresenceLast =
            (row, range, fooName, nextFileName) => row.PresenceLast > range - 10 && row.PresenceLast <= range;

         Func<Row, double, string, string, bool> funcDelta =
            (row, range, fooName, nextFileName) => row.Delta > range - 10 && row.Delta <= range;

         Func<Row, double, string, string, bool> funcPresenceDelta =
            (row, range, fooName, nextFileName) => row.PresenceDelta > range - 10 && row.PresenceDelta <= range;

         Func<Row, double, string, string, bool> funcIrregularity =
            (row, range, fooName, nextFileName) => row.Irregularity > range - 10 && row.Irregularity <= range;

         Func<Row, double, string, string, bool> funcName =
            (row, range, fooName, nextRegion) => row.RegionName == nextRegion;

         if (electionYears.Length > 1)
         {
            outputHtml(rowsSortedByResultPrevious, funcPrevious, new[] { indexFiles[(int)Indexes.SortedByPrevious], "" }, false);
            outputHtml(rowsSortedByPresencePrevious, funcPresencePrevious, new[] { indexFiles[(int)Indexes.SortedByPresencePrevious], "" }, false);
         }
         outputHtml(rowsSortedByResultLast, funcLast, new[] { string.Format(indexFiles[(int)Indexes.SortedByLast], mainFoo.EnglishShort), "" }, false);
         k = 0;
         foreach (var kvp in otherFoos)
         {
            outputHtml(rowsOtherSortedByResultLast[k], funcOtherLast, new[] { string.Format(indexFiles[(int)Indexes.SortedByLast], kvp.Key), kvp.Key }, false);
            k++;
         }

         outputHtml(rowsSortedByPresenceLast, funcPresenceLast, new[] { indexFiles[(int)Indexes.SortedByPresenceLast], "" }, false);

         if (electionYears.Length > 1)
         {
            outputHtml(rowsSortedByDelta, funcDelta, new[] { indexFiles[(int)Indexes.SortedByDelta], "" }, false);
            outputHtml(rowsSortedByPresenceDelta, funcPresenceDelta, new[] { indexFiles[(int)Indexes.SortedByPresenceDelta], "" }, false);
         }
         outputHtml(rowsSortedByName, funcName, new[] { indexFiles[(int)Indexes.SortedByAlphabet], "" }, true);
         outputHtml(rowsSortedByIrregularity, funcIrregularity, new[] { indexFiles[(int)Indexes.SortedByIrregularity], "" }, false);       
      }

      private void CalculateMaxValues(Row[] rows)
      {
         rows.ForEach(row =>
                {
                   var maxValues = row.MaxValues;
                   if (maxValues.Count == 1)
                   {
                      var nameOfMax = maxValues.First();
                      win[nameOfMax]++;
                   }
                });
      }

      private int WriteTR(int counter, StreamWriter sw, Row row, string classAttr, FooData mainFoo, Dictionary<string, FooData> otherFoos, bool isPreviousExisting)
      {
         var maxValues = row.MaxValues;
         string color = "";
         bool main = false;
         if (maxValues.Count > 1)
         {
            color = "gray";
         }
         else
         {
            var nameOfMax = maxValues.First();
            if (nameOfMax != mainFoo.EnglishShort)
            {
               color = otherFoos[nameOfMax].Color;
            }
            else
            {
               main = true;
            }
         }

         var specialClassAttr = classAttr; 
         classAttr = string.Format(classAttr, color);

         var sb = new StringBuilder();

         sb.Append("<tr>");

         sb.AppendFormat("<td {1}>{0}</td>\n", row.ResultLast.ToString("N2"), classAttr);

         foreach (var other in row.Others)
         {
            var otherClassAttr = (main) ? string.Format(specialClassAttr, "Light" + other.Value.Color) : classAttr;
            sb.AppendFormat("<td {1}>{0}</td>\n", row.GetOtherValue(other.Key).ToString("N2"), otherClassAttr);
         }

         sb.AppendFormat("<td style=\"padding-left:10;text-align: left;\" {1}>{0}</td>\n", row.Href, classAttr);

         if (isPreviousExisting)
            sb.AppendFormat("<td {1}>{0}</td>\n", (row.PresencePrevious != -1) ? row.PresencePrevious.ToString("N2") : "нет данных", classAttr);

         sb.AppendFormat("<td {1}>{0}</td>\n", row.PresenceLast.ToString("N2"), classAttr);


         if (isPreviousExisting)
            sb.AppendFormat("<td {1}>{0}</td>\n", row.PresenceDelta == -200 ?
                                                                                    "нет данных" :
                                                                                    (row.PresenceDelta >= 0) ?
                                                                                       string.Format("+{0:N2}", row.PresenceDelta) :
                                                                                       (row.PresenceDelta < 0) ?
                                                                                          string.Format("{0:N2}", row.PresenceDelta) :
                                                                                          "0.00", classAttr);

         if (isPreviousExisting)
            sb.AppendFormat("<td {1}>{0}</td>\n", (row.ResultPrevious != -1) ? row.ResultPrevious.ToString("N2") : "нет данных", classAttr);

         if (isPreviousExisting)
            sb.AppendFormat("<td {1}>{0}</td>\n", row.Delta == -200 ?
                                                                                       "нет данных" :
                                                                                       (row.Delta >= 0) ?
                                                                                          string.Format("+{0:N2}", row.Delta) :
                                                                                          (row.Delta < 0) ? string.Format("{0:N2}", row.Delta) : "0.00", classAttr);
         
         sb.AppendFormat("<td {1}>{0}</td>\n", row.Irregularity.ToString("N2"), classAttr);
         sb.Append("</tr>");
         sw.WriteLine(sb.ToString());
         counter++;

         return counter;
      }

      private void GenerateRegionHtml(
         string translit, string place, Election election, 
         Dictionary<string, Election>[] electionsAll, ElectionYear[] electionYears, FooData mainFoo)
      {
         Debug.Assert(electionYears.Length == electionsAll.Length, "Wrong length");

         var filesDirs = new string[electionYears.Length];
         int i = 0;
         electionYears.ForEach(y =>
         {
            var filesDir = Path.Combine(Consts.UpdatePath, Consts.Files + y.Year);
            var imagesDir = Path.Combine(Consts.UpdatePath, Consts.Images + y.Year);

            if (!Directory.Exists(filesDir)) Directory.CreateDirectory(filesDir);
            if (!Directory.Exists(imagesDir)) Directory.CreateDirectory(imagesDir);

            filesDirs[i] = filesDir;
            i++;
         });

         var htmlFile = Path.Combine(filesDirs[0],  translit + ".html");
         if (place.Contains("Гольяново"))
         {


         }
         using (var sw = new StreamWriter(htmlFile, false, Encoding.GetEncoding(1251)))
         {
            var titleMain = "Результаты выборов " + place;
            sw.WriteLine("<html>");
            sw.WriteLine(GetHead().Replace(TitleHolder, titleMain));
            sw.WriteLine("<table align=\"center\">");

            for (int j = 0; j < electionsAll.Length; j++)
            {
                var what_ = (electionYears[j].ElectionType == ElectionType.Duma)
                    ? Data.Core.Consts.ResultsDuma
                    : Data.Core.Consts.ResultsPresident;

               if (!electionsAll[j].ContainsKey(place)) continue;
               var localPath = Data.Core.Consts.ResultsPath + @"\" + what_ + @"\" + electionsAll[j][place].ElectionCommittee;
               var di = new DirectoryInfo(localPath);
               //if (!di.Exists) continue;
               var pattern = string.Format(Consts.PatternExtJpg, electionYears[j].Year);
               var jpgFiles = di.GetFiles(pattern);
               if (jpgFiles.Length == 0) continue;

               var jpgFile = jpgFiles[0];
               var jpg = Path.Combine(Consts.UpdatePath,  Consts.Images + electionYears[j].Year + @"\" + jpgFile.Name);
               if (!File.Exists(jpg))
               {
                  jpgFile.CopyTo(jpg);
                  Trace.WriteLine(jpg);
               }

               var title = string.Format(electionYears[j].CaptionDiagram, electionYears[j].Year, place);
               var href = election.Href;
               sw.WriteLine("<tr><td align=\"center\" style=\"font-size: 16pt;font-weight:bold;\">{0}</td></tr>", title);
               sw.WriteLine("<tr><td align=\"center\"><a href=\"{0}\">Посмотреть результаты на официальном сайте</a></td></tr>", href);
               sw.WriteLine("<tr><td align=\"center\"><img align=\"center\" src=\"../{0}{2}/{1}\"/><br><br></td></tr>", Consts.Images, jpgFile.Name, electionYears[j].Year);
            };

            sw.WriteLine("<tr><td align=\"center\"><a href=\"../{0}{1}/{2}\">Обратно на главную</a></td></tr>", Consts.Iks, electionYears[0].Year, string.Format(indexFiles[(int)Indexes.SortedByLast], mainFoo.EnglishShort));
            sw.WriteLine("</table>");
            sw.WriteLine("<br>");
            sw.WriteLine("</html>");
         }
      }

      private string PrepareHeader(Row[] rows, FooData mainFoo, FooData[] others, ElectionYear electionYearCurrent, ElectionYear electionYearPrevious, bool calcDiff)
      {
         if (electionYearPrevious != null) Debug.Assert(electionYearCurrent.ElectionType == electionYearPrevious.ElectionType, "Different types");

         CalculateMaxValues(rows);

         string style;
         using (var streamStyle = Assembly.GetExecutingAssembly().GetManifestResourceStream(EmbeddedResource + "Style.html"))
         using (var streamReader = new StreamReader(streamStyle, Encoding.GetEncoding(1251)))
         {
            style = streamReader.ReadToEnd();
         }

         string td;
         using (var streamStyle = Assembly.GetExecutingAssembly().GetManifestResourceStream(EmbeddedResource + "td.html"))
         using (var streamReader = new StreamReader(streamStyle, Encoding.GetEncoding(1251)))
         {
            td = streamReader.ReadToEnd();
         }

         var what = (electionYearCurrent.ElectionType == ElectionType.Duma) ? "фракции" : "кандидата";

         const string widthValue = "70";
         const string width = "<width>";
         const string text = "<text>";
         const string file = "<file>";
         var column0 = (electionYearPrevious != null && calcDiff) ?
            td
            .Replace(width, widthValue)
            .Replace(text, string.Format("Результаты {2} \"{0}\" по избирательным комиссиям в {1} году\", %", mainFoo.RussianLong, electionYearPrevious.Year, what))
            .Replace(file, indexFiles[0]) : "";

         var column1 = td
            .Replace(width, widthValue)
            .Replace(text, string.Format("Результаты {2} \"{0}\" по избирательным комиссиям в {1} году\", %", mainFoo.RussianLong, electionYearCurrent.Year, what))
            .Replace(file, string.Format(indexFiles[1], mainFoo.EnglishShort));

         var column2 = (electionYearPrevious != null && calcDiff) ?
            td
            .Replace(width, widthValue)
            .Replace(text, string.Format("Изменения результатов {3} \"{0}\" в {1} году относительно {2} года, %", mainFoo.RussianLong, electionYearCurrent.Year, electionYearPrevious.Year, what))
            .Replace(file, indexFiles[2]) : "";

         var column = new string[others.Length];
         for (int i = 0; i < column.Length; i++)
         {
            column[i] =
            td
            .Replace(width, widthValue)
            .Replace(text, string.Format("Результаты {2} \"{0}\" по избирательным комиссиям в {1} году\", %", others[i].RussianLong, electionYearCurrent.Year, what))
            .Replace(file, string.Format(indexFiles[1], others[i].EnglishShort));
         }

         var column3 = td
            .Replace(width, "500")
            .Replace(text, string.Format("Избирательная комиссия (ссылка на диаграмму результатов выборов по УИК)</a>"))
            .Replace(file, indexFiles[3]);


         var column4 = (electionYearPrevious != null && calcDiff) ? 
            td
            .Replace(width, widthValue)
            .Replace(text, string.Format("Явка избирателей в {0} году, %", electionYearPrevious.Year))
            .Replace(file, indexFiles[4]) : "";

         var column5 = td
            .Replace(width, widthValue)
            .Replace(text, string.Format("Явка избирателей в {0} году, %", electionYearCurrent.Year))
            .Replace(file, indexFiles[5]);

         var column6 = (electionYearPrevious != null && calcDiff) ? 
            td
            .Replace(width, widthValue)
            .Replace(text, string.Format("Изменение явки в {0} году относительно {1} года", electionYearCurrent.Year, electionYearPrevious.Year))
            .Replace(file, indexFiles[6]) : "";

         var column7 =
            td
               .Replace(width, widthValue)
               .Replace(text, string.Format(IrregularityHeader, electionYearCurrent.Year))
               .Replace(file, indexFiles[7]);

         var maxReport = new StringBuilder();
         foreach (var winKvp in win)
         {
            maxReport.AppendFormat("<tr><td align=\"left\"><b>{0}</b> имеет максимальные результаты на <b>{1}</b> избирательных комиссиях</td><tr>", 
               fooDataDict[winKvp.Key].RussianLong, winKvp.Value);
         }

         var sum = win.Sum(kvp => kvp.Value);
         bool b = sum == rows.Length;

         var title = string.Format("Сводная таблица результатов выборов {0} по всем избирательным комиссиям, {1} год</h2>", electionYearCurrent.MainTitle, electionYearCurrent.Year);
         var begin = new StringBuilder();
         begin.Append("<html>");
         begin.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1251\">");
         begin.Append(GetHead().Replace(TitleHolder, title));
         begin.Append(style);    
         begin.AppendFormat("<table align=\"center\"><tr><td align=\"center\" style=\"font-size: 18pt;font-weight:bold;\">{0}</td><tr></table><br>", title);
         begin.AppendFormat("<table align=\"center\">{0}</table>", maxReport);
         begin.Append("<table align=\"center\"><tr><td align=\"center\" style=\"font-size: 15pt;font-weight:bold;\"><a href=\"../index.html\">На главную</a></h3></td></tr>");//h3
         begin.Append("<tr><td align=\"center\" style=\"font-size: 15pt;font-weight:bold;\">Данные можно сортировать, нажав на соответствующую ячейку в шапке таблицы</td></tr>");
         begin.Append("</table><br>");
         var end = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", column1, string.Join("", column), column3, column4, column5, column6, column0, column2, column7);
         begin.AppendFormat("<table border=\"1\" bgcolor=\"#ffffff\" cellpadding=\"2\" cellspacing=\"1\" align=\"center\" vspace=\"0\"><tr>{0}</tr>", end);
         return begin.ToString();
      }

      #endregion
   }
}
