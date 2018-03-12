using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Elections.Diagrams;
using Elections.Diagrams.BarChart;
using NUnit.Framework;

namespace Elections.Tests
{
    [TestFixture]
    public class BarChartDrawerTest
    {
        [Test]
        public void DrawDiagramForTxtDataTest()
        {
            var item = new DiagramData()
            {
                ChartTitle = "Hello",
                HorizontalNames = new[] {"u1", "u2"},
                RowItem = new[]
                {
                    new RowItem() {Name = "L1, 50%", Values = new List<double> {1.00, 0.90}},
                    new RowItem() {Name = "L2, 40%", Values = new List<double> {0.90, 0.80}},
                    new RowItem() {Name = "L3, 30%", Values = new List<double> {0.80, 0.70}},
                    new RowItem() {Name = "L4, 20%", Values = new List<double> {0.60, 0.50}},
                    new RowItem() {Name = "L5, 10%", Values = new List<double> {0.50, 0.40}},
                },
                PicName = @"W:\VS\Reps\GitHub\BarChartTest\hi.jpg"
            };

            int n = 1;
            int num = 60 / n;
            var t = new Thread[n];


            for (int i = 0; i < n; i++)
            {
                int tmp = i;
                t[i] = new Thread(() => { Do(num * tmp, num * tmp + num, item); });
            }

            for (int i = 0; i < n; i++)
            {
                t[i].Start();
            }

            for (int i = 0; i < n; i++)
            {
                t[i].Join();
            }

            Console.WriteLine(s);
        }

        private string s = "";

        private void Do(int start, int end, DiagramData item)
        {
            using (var barChartDrawer = new BarChartDrawer())
            {
                for (int i = start; i < end; i++)
                {
                    item.PicName = @"W:\VS\Reps\GitHub\BarChartTest\hi" + i + ".jpg";
                    barChartDrawer.DrawDiagramForTxtData(item);
                }
            }
        }
    }
}