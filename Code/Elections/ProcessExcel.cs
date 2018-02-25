using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Data.Core;
using Elections.Utility;
using Elections.XmlProcessing;
using Microsoft.Office.Interop.Excel;
using Action = System.Action;
using Excel = Microsoft.Office.Interop.Excel;

namespace Elections
{
   public enum AxisYType
   {
      UIK,
      People
   }

   public enum DiagramType
   {
      Results,
      Presence
   }

   public class ProcessExcel : IDisposable
   {
      #region Consts

      public const int MinRowNumberForFactions2007 = 30;
      public const int MinRowNumberForFactions2011 = 29;
      
      private const int ColNumberForFactions = 2;

      public const string UIK = "УИК";
      public const string ER = "ЕДИНАЯ РОССИЯ";

      private const int Count2010 = 2408;
      private const double CountDelta2011 = (double) Count2010 / 100;

      private const int Count2007 = 2402;
      private const double CountDelta2007 = (double) Count2007 / 100;

        #endregion

        #region Fields

       private ApplicationClass app = new ApplicationClass();

        public static Dictionary<string, string> Parties = new[]
         {
            //2003 

            //new KeyValuePair<string, string>("1. \"ЕДИНЕНИЕ\"","ЕДИНЕНИЕ"), 
            new KeyValuePair<string, string>("2. \"СОЮЗ ПРАВЫХ СИЛ\"","СПС"), 
            new KeyValuePair<string, string>("3. \"РОССИЙСКАЯ ПАРТИЯ ПЕНСИОНЕРОВ И ПАРТИЯ СОЦИАЛЬНОЙ СПРАВЕДЛИВОСТИ\"","РППиПСС"), 
            new KeyValuePair<string, string>("4. \"Российская демократическая партия \"ЯБЛОКО\"","Яблоко"), 
            //new KeyValuePair<string, string>("5. \"За Русь Святую\"","За Русь Святую"), 
            //new KeyValuePair<string, string>("6. \"Объединенная Российская партия \"Русь\"","ОРП"), 
            //new KeyValuePair<string, string>("7. \"Новый курс - Автомобильная Россия\"","НКАР"), 
            //new KeyValuePair<string, string>("8. \"Народно-республиканская партия России\"","НРПР"), 
            //new KeyValuePair<string, string>("9. \"Российская экологическая партия \"Зеленые\"","РЭПЗ"), 
            new KeyValuePair<string, string>("10. \"Аграрная партия России\"","АПР"), 
            //new KeyValuePair<string, string>("11. \"Истинные патриоты России\"","ИПР"), 
            //new KeyValuePair<string, string>("12. \"НАРОДНАЯ ПАРТИЯ Российской Федерации\"","НПРФ"), 
            //new KeyValuePair<string, string>("13. \"Демократическая партия России\"","ДПР"), 
            //new KeyValuePair<string, string>("14. \"Великая Россия - Евразийский Союз\"","ВРЕС"), 
            //new KeyValuePair<string, string>("15. \"Партия СЛОН\"","СЛОН"), 
            //new KeyValuePair<string, string>("17. \"Партия Мира и Единства (ПМЕ)\"","ПМЕ"), 

            new KeyValuePair<string, string>("16. \"Родина\" (народно-патриотический союз)\"","Родина"), 
            new KeyValuePair<string, string>("18. \"ЛДПР\"","ЛДПР"), 

            //new KeyValuePair<string, string>("19. \"Партия Возрождения России - Российская партия ЖИЗНИ\"","Жизнь"), 

            new KeyValuePair<string, string>("20. \"Политическая партия \"Единая Россия\"","Единая Россия"), 

            //new KeyValuePair<string, string>("21. \"Российская Конституционно-демократическая партия\"", "РКДП"),
            //new KeyValuePair<string, string>("22. \"Развитие предпринимательства\"", "РП"),

            new KeyValuePair<string, string>("23. \"Коммунистическая партия Российской Федерации (КПРФ)\"","КПРФ"), 

            ////////////////

            new KeyValuePair<string, string>("1. Политическая партия СПРАВЕДЛИВАЯ РОССИЯ","Справедливая Россия"), 
            new KeyValuePair<string, string>("2. Политическая партия \"Либерально-демократическая партия России\"","ЛДПР"),
            new KeyValuePair<string, string>("3. Политическая партия \"ПАТРИОТЫ РОССИИ\"","Патриоты России")     ,
            new KeyValuePair<string, string>("4. Политическая партия \"Коммунистическая партия Российской Федерации\"","КПРФ"),
            new KeyValuePair<string, string>("5. Политическая партия \"Российская объединенная демократическая партия \"ЯБЛОКО\"","Яблоко")     ,
            new KeyValuePair<string, string>("6. Всероссийская политическая партия \"ЕДИНАЯ РОССИЯ\"","Единая Россия"),
            new KeyValuePair<string, string>("7. Всероссийская политическая партия \"ПРАВОЕ ДЕЛО\"","Правое Дело"),

            new KeyValuePair<string, string>("1.Политическая партия «Аграрная партия России»","Аграрная партия России"),
            new KeyValuePair<string, string>("2.Всероссийская политическая партия «Гражданская Сила»","Гражданская Сила"),
            new KeyValuePair<string, string>("3.Политическая партия «Демократическая партия России»","Демократическая партия России"),
            new KeyValuePair<string, string>("4.Политическая партия «Коммунистическая партия Российской Федерации»","КПРФ"),
            new KeyValuePair<string, string>("5.Политическая партия «СОЮЗ ПРАВЫХ СИЛ»","СОЮЗ ПРАВЫХ СИЛ"),
            new KeyValuePair<string, string>("6.Политическая партия «Партия социальной справедливости»","Партия социальной справедливости"),
            new KeyValuePair<string, string>("7.Политическая партия «Либерально-демократическая партия России»","ЛДПР"),
            new KeyValuePair<string, string>("8.Политическая партия «СПРАВЕДЛИВАЯ РОССИЯ: РОДИНА/ПЕНСИОНЕРЫ/ЖИЗНЬ»","Справедливая Россия"),
            new KeyValuePair<string, string>("9.Политическая партия «ПАТРИОТЫ РОССИИ»","Патриоты России"),
            new KeyValuePair<string, string>("10.Всероссийская политическая партия «ЕДИНАЯ РОССИЯ»","Единая Россия"),
            new KeyValuePair<string, string>("11.Политическая партия «Российская объединенная демократическая партия «ЯБЛОКО»","Яблоко"),
            
            new KeyValuePair<string, string>("Глазьев Сергей Юрьевич", "Глазьев"),
            new KeyValuePair<string, string>("Малышкин Олег Александрович", "Малышкин"),
            new KeyValuePair<string, string>("Миронов Сергей Михайлович", "Миронов"),
            new KeyValuePair<string, string>("Путин Владимир Владимирович", "Путин"),
            new KeyValuePair<string, string>("Хакамада Ирина Муцуовна", "Хакамада"),
            new KeyValuePair<string, string>("Харитонов Николай Михайлович", "Харитонов"),
            new KeyValuePair<string, string>("Против всех", "Против всех"),

            new KeyValuePair<string, string>("Богданов Андрей Владимирович", "Богданов"),
            new KeyValuePair<string, string>("Жириновский Владимир Вольфович", "Жириновский"),
            new KeyValuePair<string, string>("Зюганов Геннадий Андреевич", "Зюганов"),
            new KeyValuePair<string, string>("Медведев Дмитрий Анатольевич", "Медведев"),

            new KeyValuePair<string, string>("Прохоров Михаил Дмитриевич", "Прохоров"),

            //////////////
            
            new KeyValuePair<string, string>("2.Боженов Сергей Анатольевич", "Боженов"),

            new KeyValuePair<string, string>("4.Столяров Михаил Николаевич", "Столяров"),
            new KeyValuePair<string, string>("5.Шеин Олег Васильевич", "Шеин"),

         }.ToDictionary(k => k.Key, k => k.Value);

      public Dictionary<string, int> PartiesOrder = new[]
         {
            new KeyValuePair<string, int>("Справедливая Россия", 1), 
            new KeyValuePair<string, int>("ЛДПР", 2),
            new KeyValuePair<string, int>("Патриоты России", 3),
            new KeyValuePair<string, int>("КПРФ", 4),
            new KeyValuePair<string, int>("Яблоко", 5),
            new KeyValuePair<string, int>("Единая Россия", 6),
            new KeyValuePair<string, int>("Правое Дело", 7),
         }.ToDictionary(k => k.Key, k => k.Value);

      public Dictionary<string, int> PartiesOrder2003 = new[]
         {
            new KeyValuePair<string, int>("Единая Россия", 1),
            new KeyValuePair<string, int>("КПРФ", 2),
            new KeyValuePair<string, int>("ЛДПР", 3),
            new KeyValuePair<string, int>("Родина", 4), 
            new KeyValuePair<string, int>("Яблоко", 5), 
            //new KeyValuePair<string, int>("СПС", 6), 
            //new KeyValuePair<string, int>("АПР", 7), 
            //new KeyValuePair<string, int>("РППиПСС", 8), 
         }.ToDictionary(k => k.Key, k => k.Value);

      public Dictionary<string, int> PresidentOrder = new[]
      {
         new KeyValuePair<string, int>("Путин",1),
         new KeyValuePair<string,int>("Медведев",1),

         new KeyValuePair<string,int>("Харитонов",2),
         new KeyValuePair<string,int>("Зюганов",2),

         new KeyValuePair<string,int>("Глазьев",3),
         new KeyValuePair<string,int>("Жириновский",3),

         new KeyValuePair<string,int>("Хакамада",4),
         new KeyValuePair<string,int>("Богданов",4),
         new KeyValuePair<string,int>("Прохоров",4),

         new KeyValuePair<string,int>("Миронов",5),
      }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

      public Dictionary<string, int> AstrahanOrder = new[]
      {
         new KeyValuePair<string, int>("Боженов",1),
         new KeyValuePair<string, int>("Столяров",1),
         new KeyValuePair<string,int>("Шеин",2),
      }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

      private int minRowNumberForFactions;

      public string electionInfoDir;
      
      #endregion

      #region Properties

      private Pair<double, string> MaxDeltaPair { get; set; }
      public bool IsStopped { get; set; }

      #endregion

      #region Constructors

      public ProcessExcel() 
      {
         MaxDeltaPair = new Pair<double, string>(-1, "");

         app = new ApplicationClass();
      }

      #endregion

      #region Public Methods

      public void PrepareDrawAllDiagrams(ElectionYear[] electionYears)
      {
         Debug.Assert(electionYears.Select(ey => ey.ElectionType).Distinct().Count() == 1, "Wrong type in election years array");
         var stopWatch = new Stopwatch();
         stopWatch.Start();

         string path =  Path.Combine(Consts.ResultsPath, electionYears[0].Result);
         var electionType = electionYears[0].ElectionType;
         var patterns = electionYears.Select(ey => ey.PatternExt).ToArray();

         if (IsStopped) return;

         var directoryInfo = new DirectoryInfo(path);
         var directoryInfos = directoryInfo.GetDirectories(); //Enumerable.Range(0, 10).Select(i => directoryInfo.GetDirectories()[i]).ToArray();
         
         var wantThreads = true;

         if (wantThreads)
         {
            const int n = 4;
            var count = directoryInfos.Length/n;

            var threads = new Thread[n];
            for (int i = 0; i < n; i++)
            {
               var start = i*count;
               int length = (i < n - 1) ? count : directoryInfos.Length - start;
               var directoryInfosNew = new DirectoryInfo[length];
               Array.Copy(directoryInfos, start, directoryInfosNew, 0, length);
               threads[i] = new Thread(() => DrawAllDiagrams(directoryInfosNew, electionYears));
            }

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
         }
         else
         {
            DrawAllDiagrams(directoryInfos, electionYears);
         }

         stopWatch.Stop();
         Trace.WriteLine(stopWatch.Elapsed);
      }

      public void DrawAllDiagrams(DirectoryInfo[] directoryInfos, ElectionYear[] electionYears)
      {
         if (IsStopped) return;
         foreach (var di in directoryInfos)
         {
            if (IsStopped) return;

            Action<ElectionYear> processFiles = (electionYear) =>
            {
               foreach (var fi in di.GetFiles(electionYear.PatternExt))
               {
                  if (IsStopped) return;
                  DrawDiagramForTxtData(fi, electionYear, false);
               }
            };

            if (di.FullName.EndsWith(Consts.LocalCommittee))
            {
               electionYears.ForEach(processFiles);
            }
            DrawAllDiagrams(di.GetDirectories(), electionYears);
         }
      }

      public void DrawDiagramForTxtData(FileInfo fi, ElectionYear electionYear, bool overWrite)
      {
         var stopWatch = new Stopwatch();
         stopWatch.Start();

         var fileNameTmp = fi.DirectoryName + @"\" + Path.GetFileNameWithoutExtension(fi.FullName) + "tmp.xls";

         var year = Convert.ToInt32(fi.FullName.Substring(fi.FullName.Length - 8, 4));

         var location = TextProcessFunctions.GetElectionCommitteeName(electionYear, fi.DirectoryName);
         var picName = string.Format(@"{0}\{1}{2}.jpg", fi.DirectoryName, TextProcessFunctions.Translit(location), year);

         if (File.Exists(picName) && !overWrite) return;
         Trace.WriteLine(fi.FullName);

         object misValue = System.Reflection.Missing.Value;

         var workBookNew = app.Workbooks.Add(misValue);

         var workSheetNew = (Worksheet)workBookNew.Worksheets[1];

         var rangeNew = workSheetNew.UsedRange;
         
         var electionCommitteeResults = new ElectionCommitteeResults(fi.FullName);

         for (int i = 0; i < electionCommitteeResults.uiks.Count; i++)
         {
            workSheetNew.Cells[1,  i + 2] = electionCommitteeResults.uiks[i];
         }

         int rowNew = 1;
         foreach (var kvp in electionCommitteeResults.partiesData)
         {
            var fooName = kvp.Key;
            Trace.WriteLine(fooName);
            var partiesOrder = year == Consts.ElectionYear2003.Year ? PartiesOrder2003 : PartiesOrder;
            if (electionYear.ElectionType == ElectionType.Duma && !partiesOrder.ContainsKey(fooName) || electionYear.ElectionType == ElectionType.President && !PresidentOrder.ContainsKey(fooName)) continue;
            var partyOrder = (electionYear.ElectionType == ElectionType.Duma) 
               ? partiesOrder[fooName]
               : (electionYear.ElectionType == ElectionType.President) 
                  ? PresidentOrder[fooName]
                  : AstrahanOrder[fooName];

            workSheetNew.Cells[partyOrder + 1, 1] = string.Format("{0}, {1}%", fooName, kvp.Value.Percent.ToString().Replace(",", "."));
            for (int j = 0; j < electionCommitteeResults.uiks.Count; j++)
            {
               var value = kvp.Value.LocalElectionCommittees[j].Percent / 100;
               if (double.IsNaN(value)) value = 0;
               (rangeNew.Cells[partyOrder + 1, j + 2] as Range).Value2 = value;
               (rangeNew.Cells[partyOrder + 1, j + 2] as Range).NumberFormat = "###,##%";
            }
            rowNew++;
         }

         var chartObjects = (ChartObjects)workSheetNew.ChartObjects(Type.Missing);

         const int oneWidth = 20;
         const int widthFactions = 190;
         const int widthMin = 700;
         int uiksWidth = oneWidth * electionCommitteeResults.uiks.Count;
         int width = widthFactions + uiksWidth;
         if (width < widthMin) 
         { 
            width = widthMin;
            uiksWidth = width - widthFactions;
         }

         var chartObject = chartObjects.Add(0, 0, width, 250);
         var chart = (Chart)chartObject.Chart;
         chart.ChartType = XlChartType.xlColumnClustered;

         var axis = (Axis)chart.Axes(XlAxisType.xlValue);
         axis.MaximumScale = 1;

         var range = workSheetNew.Range["1:1", string.Format("{0}:{1}", rowNew, rowNew)];
         chart.HasTitle = true;
         chart.ChartTitle.Text = string.Format(electionYear.CaptionDiagram, year, location);
         chart.SetSourceData(range, 1);
         chart.Legend.Font.Size = 11;
         chart.Legend.Font.Bold = true;
         chart.PlotArea.Width = uiksWidth;
         chart.Legend.Left = chart.PlotArea.Left + chart.PlotArea.Width + 10;
         //((Excel.LegendEntry) chart.Legend.LegendEntries(5)).
         //var f = ((Excel.LegendEntry) chart.Legend.LegendEntries(1));
         // ((Excel.LegendEntry)chart.Legend.LegendEntries(1)).LegendKey.Interior.ColorIndex = 1;//.Color = (int)Excel.XlRgbColor.rgbRed;
         //Trace.WriteLine(chart.Legend.Left);

         chart.Export(picName, "JPG", misValue);

         //if (!File.Exists(fileNameTmp) || overWrite)
         //{
         //   if (overWrite)
         //   {
         //      if (File.Exists(fileNameTmp)) File.Delete(fileNameTmp);
         //   }
         //   workBookNew.SaveAs(fileNameTmp, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
         //                      XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
         //   workBookNew.Close(true, null, null);
         //}
         //else
         {
            workBookNew.Close(false, null, null);
         }
            
         ReleaseObject(workSheetNew);
         ReleaseObject(workBookNew);

         stopWatch.Stop();
         Trace.WriteLine(stopWatch.Elapsed);
         //using (var sw = new StreamWriter("trace.txt", true))
         //{
         //   sw.WriteLine("{0} {1}", stopWatch.Elapsed, fi.FullName);
         //}
      }

      private static SortedDictionary<int, int> GetNumbersToPresence(string foo, List<Election> elections, AxisYType axisYType)
      {
         var percentsCounter = new SortedDictionary<int, int>();
         foreach (var election in elections)
         {
            if (axisYType == AxisYType.UIK)
            {
               election.AllPresences.ForEach(presence =>
               {
                  if (!double.IsNaN(presence))
                  {
                     var percentRound = (int)presence;

                     if (!percentsCounter.ContainsKey(percentRound))
                        percentsCounter[percentRound] = 1;
                     else
                        percentsCounter[percentRound]++;
                  }
               });
            }
            else
            {
               Debug.Assert(election.AllPresences.Length == election.AllNumberOfElectorsInList.Length, "Wrong length");

               for (int i = 0; i < election.AllPresences.Length; i++)
               {
                  var presence = election.AllPresences[i];
                  var numberOfElectors = election.AllNumberOfElectorsInList[i];

                  if (!double.IsNaN(presence))
                  {
                     var percentRound = (int) election.AllPresences[i];
                     if (!percentsCounter.ContainsKey(percentRound))
                        percentsCounter[percentRound] = numberOfElectors;
                     else
                        percentsCounter[percentRound] += numberOfElectors;

                  }
               }
            }
         }
         return percentsCounter;
      }

      private static SortedDictionary<int, int> GetNumbersToResult(string foo, List<Election> elections, AxisYType axisYType)
      {
         var valuesCounter = new SortedDictionary<int, int>();
         foreach (var election in elections)
         {
            if (axisYType == AxisYType.UIK)
            {
               election.GetFoo(foo).AllValues.ForEach(v =>
                                                         {
                                                            if (!double.IsNaN(v))
                                                            {
                                                               var value = (int) v;
                                                               if (!valuesCounter.ContainsKey(value))
                                                                  valuesCounter[value] = 1;
                                                               else
                                                                  valuesCounter[value]++;
                                                            }
                                                         });
            }
            else
            {
               Debug.Assert(election.GetFoo(foo).AllValues.Length == election.AllNumberOfElectorsInList.Length, "Wrong length");
               for (int i = 0; i < election.GetFoo(foo).AllValues.Length; i++)
               {
                  var v = election.GetFoo(foo).AllValues[i];
                  var numberOfElectors = election.AllNumberOfElectorsInList[i];

                  if (!double.IsNaN(v))
                  {
                     var value = (int)v;
                     if (!valuesCounter.ContainsKey(value))
                        valuesCounter[value] = numberOfElectors;
                     else
                        valuesCounter[value] += numberOfElectors;
                  }
               }               
            }

         }
         return valuesCounter;
      }

      public static string GenerateGraphic(
         ElectionYear electionYear, 
         string[] foos, 
         AxisYType axisYType,
         DiagramType diagramType
         )
      {
         var fooDatas = electionYear.FooData.Where(f => !f.IsHiddenForIks).ToArray();
         var path = Consts.LocalPath;
         Func<string, List<Election>, AxisYType, SortedDictionary<int, int>> getResults;
         
         if  (diagramType == DiagramType.Results) 
            getResults = GetNumbersToResult;
         else
            getResults = GetNumbersToPresence;

         var firstPart = diagramType == DiagramType.Results ? "Results" : "Presence";
         var secondPart = axisYType == AxisYType.People ? "ByPeople" : "ByUIKs";

         string picName =  firstPart + secondPart;

         Directory.CreateDirectory(Consts.GraphicsPath);
         var name = Path.Combine(Consts.GraphicsPath, picName + electionYear.Year);
         var pictureName = name + ".jpg";
         var xls = name + ".xls";
         var fi = new FileInfo(pictureName);
         var fiXls = new FileInfo(xls);

         if (File.Exists(pictureName)) return fi.Name;

         var what = electionYear.DirElectionInfo;

         var fileName = path + @"\" + Consts.ElectionsDir + @"\" + what + @"\" + what + electionYear.Year + ".xml";
         var elections = ProcessData.ReadSavedData(fileName);

         object misValue = System.Reflection.Missing.Value;

         const int Width = 400;// 467;
         const int Height = 230;// 200;

         var app = new ApplicationClass();
         var workBook = app.Workbooks.Add(misValue);
         var workSheet = (Worksheet)workBook.Worksheets[1];

         var chartObjects = (ChartObjects)workSheet.ChartObjects(Type.Missing);
         var chartObject = chartObjects.Add(10, 80, (diagramType == DiagramType.Results) ? Width : 300 , Height);
         var chart = chartObject.Chart;
         chart.ChartType = XlChartType.xlXYScatterLinesNoMarkers;

         var seriesCollection = (SeriesCollection)chart.SeriesCollection(misValue);

         if (diagramType == DiagramType.Presence)
         {
            int c = 0;
            var percentsCounter = getResults(null, elections, axisYType);
            foreach (var kvp in percentsCounter)
            {
               c++;
               workSheet.Cells[c, 1] = kvp.Value;
               workSheet.Cells[c, 2] = kvp.Key;
            }

            var series = seriesCollection.NewSeries();
            series.Values = workSheet.Range["a1", "a" + c.ToString()];
            series.XValues = workSheet.Range["b1", "b" + c.ToString()];
            chart.Legend.Delete();
         }
         else
         {
            var values = new Triple<SortedDictionary<int, int>, string, int>[foos.Length];
            for (int i = 0; i < values.Length; i++)
            {
               var value = getResults(foos[i], elections, axisYType);
               values[i] = new Triple<SortedDictionary<int, int>, string, int>(value, foos[i], value.Count);
            }
            for (int i = 0; i < values.Length; i++)
            {
               int c = 0;
               foreach (var kvp in values[i].First)
               {
                  c++;
                  workSheet.Cells[c, 2*i + 1] = kvp.Value;
                  workSheet.Cells[c, 2*i + 2] = kvp.Key;
               }
               var series = seriesCollection.NewSeries();
               var first = (char) ('a' + 2*i);
               var second = (char) ('a' + 2*i + 1);
               var foo = fooDatas.First(f => string.Compare(f.EnglishShort, foos[i], StringComparison.CurrentCultureIgnoreCase) == 0);
               series.Name = string.Format("{0},\n{1}%", foo.RussianLong, foo.Result);
               series.Values = workSheet.Range[first.ToString() + "1", first.ToString() + c.ToString()];
               series.XValues = workSheet.Range[second + "1", second.ToString() + c.ToString()];
            }
         }

         Axis xAxis = (Axis)chart.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary);
         xAxis.HasTitle = true;
         xAxis.AxisTitle.Text = diagramType == DiagramType.Results ? "Результат, %" : "Явка, %";

         xAxis.MinimumScale = 0;
         xAxis.MajorUnit = 10;
         xAxis.MinorUnit = 1;
         xAxis.MinorTickMark = XlTickMark.xlTickMarkInside;
         xAxis.HasMajorGridlines = true;
         //xAxis.HasMinorGridlines = true;
         xAxis.MaximumScale = 100;

         Axis yAxis = (Axis)chart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary);
         yAxis.HasTitle = true;
         yAxis.AxisTitle.Text = (axisYType == AxisYType.UIK) ? "Количество участков" : "Количество людей на участках";
         yAxis.MaximumScale = diagramType == DiagramType.Results
                                 ? axisYType == AxisYType.People ? 14000000 : 10000
                                 : axisYType == AxisYType.People ? 6000000 : 6000;
         

         chart.HasTitle = true;
         var presence = diagramType == DiagramType.Results ? "" : string.Format(", {0}%", electionYear.Presence.ToString().Replace(",", "."));
         chart.ChartTitle.Text = string.Format("{0} {1} {2} {3}", diagramType == DiagramType.Results ? "Результаты выборов" : "Явка на выборах", 
                                                               (electionYear.ElectionType == ElectionType.Duma) ? "в Думу" : "президента",
                                                               electionYear.Year, presence);

         if (diagramType == DiagramType.Results) chart.PlotArea.Width = 262;
         Trace.WriteLine(chart.PlotArea.Width);
         File.Delete(pictureName);
         File.Delete(fiXls.FullName);
         //workBook.SaveAs(fiXls.FullName, XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
         chart.Export(fi.FullName, "JPG", misValue);
         workBook.Close(false, misValue, misValue);
         app.Quit();

         ReleaseObject(workSheet);
         ReleaseObject(workBook);
         ReleaseObject(app);

         return fi.Name;
      }

       public void ExportXls(string path, string years)
       {
           var filePatterns = years.Split(',').Select(y => $"*{y}.xls").ToArray();
           ExportXls(path, filePatterns);
       }

      public void ExportXls(string path, string[] filePatterns)
      {
         if (IsStopped) return;
         var directoryInfo = new DirectoryInfo(path);
         var directoryInfos = directoryInfo.GetDirectories();
         foreach (var di in directoryInfos)
         {
            if (IsStopped) return;

            if (di.FullName.EndsWith(Consts.LocalCommittee))
            {
               filePatterns.ForEach(pattern => ProcessFiles(di, pattern));
            }
            ExportXls(di.FullName, filePatterns);
         }          
      }

       void ProcessFiles(DirectoryInfo di, string pattern)
       {
           foreach (var fi in di.GetFiles(pattern))
           {
               if (IsStopped) return;
               try
               {

                   SaveXlsToTxt(fi);
               }
               catch (Exception ex)
               {
                   Trace.WriteLine(string.Format("{0}: {1}", fi.FullName, ex.Message));
               }
           }
        }

      public void SaveXlsToTxt(FileInfo fi)
      {
         var fileName = string.Format(@"{0}\{1}.txt", fi.DirectoryName, Path.GetFileNameWithoutExtension(fi.FullName));
         if (File.Exists(fileName)) return;

         Trace.WriteLine(fileName);

         object misValue = System.Reflection.Missing.Value;

         var workbooks = app.Workbooks;
         var workBook = workbooks.Open(fi.FullName, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

         var worksheets = workBook.Worksheets;
         var workSheet = (Worksheet)worksheets[1];

         workBook.SaveAs(fileName, XlFileFormat.xlTextWindows, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
         workBook.Close(true, misValue, misValue);

         ReleaseObject(workSheet);
         ReleaseObject(worksheets);
         ReleaseObject(workBook);
      }

        #endregion Public Methods

        private ElectionInfo ProcessXlsFile(string fileName, string faction)
      {
         var app = new ApplicationClass();
         var workBook = app.Workbooks.Open(fileName, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
         var workSheet = (Worksheet) workBook.Worksheets[1];

         var range = workSheet.UsedRange;

         var year = fileName.Contains(Consts.Ending2007Xls) ? Consts.ElectionYear2007 : fileName.Contains(Consts.Ending2011Xls) ?  Consts.ElectionYear2011 : null;

         minRowNumberForFactions = MinRowNumberForFactions(year, range);

         var factionNumbersRow = FindFactionRow(range, faction);
         var factionPercentRow = factionNumbersRow + 1;

         int maxColUik = MaxColumnUIK(range, ElectionFoo.Flag, ElectionFoo.Duma2007.RowLocalElectionCommittee, ElectionFoo.MinColUik);
         var pair = FindMinMaxPercentages(range, factionPercentRow, ElectionFoo.MinColUik, maxColUik);
         var delta = pair.Second - pair.First;
         
         var electionInfo = new ElectionInfo();
         electionInfo.Min = pair.First;
         electionInfo.Max = pair.Second;
         electionInfo.Value = pair.Third;
         electionInfo.Number = maxColUik - ElectionFoo.MinColUik + 1;

         if (delta > MaxDeltaPair.First)
         {
            MaxDeltaPair = new Pair<double, string>(delta, fileName);
         }

         Trace.WriteLine(string.Format("Min={0}, Max={1}, Delta={2}, Number={3}, Value={4}, {5}", electionInfo.Min, electionInfo.Max, delta, electionInfo.Number, electionInfo.Value, fileName));

         workBook.Close(true, null, null);
         app.Quit();

         ReleaseObject(workSheet);
         ReleaseObject(workBook);
         ReleaseObject(app);

         return electionInfo;
      }

      private static void ReleaseObject(object obj)
      {
         try
         {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            obj = null;
         }
         catch (Exception ex)
         {
            obj = null;
            Trace.WriteLine("Exception Occured while releasing object " + ex.ToString());
         }
         finally
         {
            GC.Collect();
         }
      }

      private static int MinRowNumberForFactions(ElectionYear year, Range range)
      {
         return year == Consts.ElectionYear2007 ? MinRowNumberForFactions2007 : year == Consts.ElectionYear2011 ? MinRowNumberForFactions2011 : range.Rows.Count;
      }

      private int FindFactionRow(Range range, string faction)
      {
         for (var row = minRowNumberForFactions; row <= range.Rows.Count; row++)
         {
            var str = (range.Cells[row, ColNumberForFactions] as Range).Value2 as string;
            if (str == null) continue;

            if (!string.IsNullOrEmpty(str) && str.Contains(faction))
            {
               return row;
            }
         }
         throw new Exception(string.Format("Wasn't able to find {0}", faction));
      }

      private static int MaxColumnUIK(Range range, string flag, int rowUIK, int colUIK)
      {
         if (((string) (range.Cells[rowUIK, colUIK - 1] as Range).Value2) != flag)
            throw new Exception(String.Format("No {0} was found in [{1},{2}]", flag, rowUIK, colUIK - 1));

         for (var col = range.Columns.Count; col >=colUIK; col--)
         {
            var val = (string)(range.Cells[rowUIK, col] as Range).Value2;
            if (!string.IsNullOrEmpty(val) && val.Contains(UIK))
            {
               return col;
            }
         }
         throw new Exception("Wasn't able to find any UIK");
      }

      private Triple<double, double, double> FindMinMaxPercentages(Range range, int row, int colMin, int colMax)
      {
         double min = 100;
         var columnWithMinHash = new HashSet<int>();
         var columnWithMin = -1;

         double max = 0;
         var val = (string)(range.Cells[row, colMin - 1] as Range).Value2;
         val = val.Replace("%", "").Replace(".", ",");
         var resultValue = Convert.ToDouble(val);

         Action findMinMax = () =>
                            {
                               for (int col = colMin; col <= colMax; col++)
                               {
                                  val = (string)(range.Cells[row, col] as Range).Value2;
                                  val = val.Replace("%", "").Replace(".", ",");
                                  var doubleValue = Convert.ToDouble(val);
                                  if (doubleValue < min && !columnWithMinHash.Contains(col))
                                  {
                                     min = doubleValue;
                                     columnWithMin = col;
                                  }
                                  if (doubleValue > max) max = doubleValue;
                                  //Trace.WriteLine(string.Format("{0}", doubleValue));
                               }
                            };

         findMinMax();

         bool isOk = false;
         if (min == 0)
         {
            for (var rw = minRowNumberForFactions; rw <= range.Rows.Count; rw = rw + 2)
            {
               var value = (range.Cells[rw, columnWithMin] as Range).Value2;
               if (value == null) continue;
               var str = value as string;
               if (str != null && str != "0" || value is double && (double)value != 0)
               {
                  isOk = true;
                  Trace.WriteLine(columnWithMin);
                  break;
               }
            }            
         }
         if (!isOk)
         {
            columnWithMinHash.Add(columnWithMin);
            findMinMax();
         }
         return new Triple<double, double, double>(min, max, resultValue);
      }

      public void Dispose()
      {
          app.Quit();
          ReleaseObject(app);
      }
    }
}
