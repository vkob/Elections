using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Elections.Diagrams.BarChart
{
    public class BarChartDrawer : IDisposable
    {
        private ApplicationClass _app;

        public BarChartDrawer()
        {
            _app = new ApplicationClass();
        }

        public string DrawDiagramForTxtData(DiagramData diagramData)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            object misValue = System.Reflection.Missing.Value;

            var workBooks = _app.Workbooks;
            var workBookNew = workBooks.Add(misValue);

            var workSheetNew = (Worksheet)workBookNew.Worksheets[1];

            var rangeNew = workSheetNew.UsedRange;

            for (int column = 0; column < diagramData.HorizontalNames.Length; column++)
            {
                workSheetNew.Cells[1, column + 2] = diagramData.HorizontalNames[column];
            }

            for (int row = 0; row < diagramData.RowItem.Length; row++)
            {
                workSheetNew.Cells[row + 2, 1] = diagramData.RowItem[row].Name;
                for (int column = 0; column < diagramData.HorizontalNames.Length; column++)
                {
                    var range1 = rangeNew.Cells[row + 2, column + 2] as Range;
                    range1.Value2 = diagramData.RowItem[row].Values[column];
                    range1.NumberFormat = "###,##%";
                }
            }

            var chartObjects = (ChartObjects)workSheetNew.ChartObjects(Type.Missing);

            const int oneWidth = 20;
            const int widthFactions = 190;
            const int widthMin = 700;
            int uiksWidth = oneWidth * diagramData.HorizontalNames.Length;
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

            var range = workSheetNew.Range["1:1", string.Format("{0}:{1}", diagramData.RowItem.Length + 1, diagramData.RowItem.Length + 1)];
            chart.HasTitle = true;
            chart.ChartTitle.Text = diagramData.ChartTitle;
            chart.SetSourceData(range, 1);
            chart.Legend.Font.Size = 11;
            chart.Legend.Font.Bold = true;
            chart.PlotArea.Width = uiksWidth;
            chart.Legend.Left = chart.PlotArea.Left + chart.PlotArea.Width + 10;

            chart.Export(diagramData.PicName, "JPG", misValue);

            workBookNew.Close(false);

            Marshal.ReleaseComObject(workSheetNew);
            Marshal.ReleaseComObject(workBookNew);
            Marshal.ReleaseComObject(workBooks);

            stopWatch.Stop();
            Console.WriteLine(diagramData.PicName + " " + stopWatch.Elapsed);

            return diagramData.PicName;
        }

        public void Dispose()
        {
            _app.Quit();
            Marshal.ReleaseComObject(_app);
        }
    }
}
