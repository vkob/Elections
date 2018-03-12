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

            var workBooks = _app.Workbooks;
            var workBook = workBooks.Add(); Marshal.ReleaseComObject(workBooks);

            var worksheets = workBook.Worksheets;
            var workSheet = (Worksheet)worksheets[1]; Marshal.ReleaseComObject(worksheets);

            var usedRange = workSheet.UsedRange;
            var cells = workSheet.Cells;
            var usedRangeCells = usedRange.Cells;

            for (int column = 0; column < diagramData.HorizontalNames.Length; column++)
            {
                cells[1, column + 2] = diagramData.HorizontalNames[column];
            }

            for (int row = 0; row < diagramData.RowItem.Length; row++)
            {
                cells[row + 2, 1] = diagramData.RowItem[row].Name;
                for (int column = 0; column < diagramData.HorizontalNames.Length; column++)
                {
                    var range1 = usedRangeCells[row + 2, column + 2] as Range;
                    range1.Value2 = diagramData.RowItem[row].Values[column];
                    range1.NumberFormat = "###,##%";
                    Marshal.ReleaseComObject(range1);
                }
            }
            Marshal.ReleaseComObject(usedRangeCells);
            Marshal.ReleaseComObject(cells);
            Marshal.ReleaseComObject(usedRange);

            var chartObjects = (ChartObjects)workSheet.ChartObjects(Type.Missing);

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
            Marshal.ReleaseComObject(chartObjects);
            var chart = chartObject.Chart;
            Marshal.ReleaseComObject(chartObject);
            chart.ChartType = XlChartType.xlColumnClustered;

            var axis = (Axis)chart.Axes(XlAxisType.xlValue);
            axis.MaximumScale = 1;
            Marshal.ReleaseComObject(axis);

            var range = workSheet.Range["1:1", string.Format("{0}:{1}", diagramData.RowItem.Length + 1, diagramData.RowItem.Length + 1)];
            Marshal.ReleaseComObject(workSheet);

            chart.HasTitle = true;
            var chartTitle = chart.ChartTitle;
            chartTitle.Text = diagramData.ChartTitle; Marshal.ReleaseComObject(chartTitle);
            chart.SetSourceData(range, 1); Marshal.ReleaseComObject(range);
            var legend = chart.Legend;
            var font = legend.Font;
            font.Size = 11;
            font.Bold = true; Marshal.ReleaseComObject(font);
            var plotArea = chart.PlotArea;
            plotArea.Width = uiksWidth;
            legend.Left = plotArea.Left + plotArea.Width + 10; Marshal.ReleaseComObject(plotArea); Marshal.ReleaseComObject(legend);

            chart.Export(diagramData.PicName, "JPG");
            Marshal.ReleaseComObject(chart);
            Console.WriteLine(diagramData.PicName + " " + stopWatch.Elapsed);

            workBook.Close(false);
            
            Marshal.ReleaseComObject(workBook);

            stopWatch.Stop();

            return diagramData.PicName;
        }

        public void Dispose()
        {
            _app.Quit();
            Marshal.ReleaseComObject(_app);
        }
    }
}
