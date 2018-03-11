using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Data.Core;
using Elections.Utility;
using Elections.XmlProcessing;
using Microsoft.Office.Interop.Excel;
using Action = System.Action;

namespace Elections.Diagrams.Graphic
{
    public class GraphicPreparer 
    {
        #region Consts

        public const int MinRowNumberForFactions2007 = 30;
        public const int MinRowNumberForFactions2011 = 29;

        private const int ColNumberForFactions = 2;

        public const string UIK = "УИК";
        public const string ER = "ЕДИНАЯ РОССИЯ";

        private const int Count2010 = 2408;
        private const double CountDelta2011 = (double)Count2010 / 100;

        private const int Count2007 = 2402;
        private const double CountDelta2007 = (double)Count2007 / 100;

        #endregion

        #region Fields

        private int minRowNumberForFactions;

        public string electionInfoDir;

        #endregion

        #region Properties

        public bool IsStopped { get; set; }

        #endregion

        #region Public Methods
      
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
                            var percentRound = (int)election.AllPresences[i];
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
                                                                      var value = (int)v;
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

            if (diagramType == DiagramType.Results)
                getResults = GetNumbersToResult;
            else
                getResults = GetNumbersToPresence;

            var firstPart = diagramType == DiagramType.Results ? "Results" : "Presence";
            var secondPart = axisYType == AxisYType.People ? "ByPeople" : "ByUIKs";

            string picName = firstPart + secondPart;

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
            var chartObject = chartObjects.Add(10, 80, (diagramType == DiagramType.Results) ? Width : 300, Height);
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
                        workSheet.Cells[c, 2 * i + 1] = kvp.Value;
                        workSheet.Cells[c, 2 * i + 2] = kvp.Key;
                    }
                    var series = seriesCollection.NewSeries();
                    var first = (char)('a' + 2 * i);
                    var second = (char)('a' + 2 * i + 1);
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
            chart.ChartTitle.Text = string.Format("{0} {1} {2}{3}", diagramType == DiagramType.Results ? "Результаты выборов" : "Явка на выборах",
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


            Marshal.ReleaseComObject(workSheet);
            Marshal.ReleaseComObject(workBook);
            Marshal.ReleaseComObject(app);

            return fi.Name;
        }

        #endregion Public Methods
    }
}
