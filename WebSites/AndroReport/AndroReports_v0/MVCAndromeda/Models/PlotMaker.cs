using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace mdx
{
    public class PlotMaker
    {
        private static SeriesChartType chartType = SeriesChartType.Column;
        private static string[] xDaily = { "M", "T", "W", "T", "F", "S", "S" };
        private static string[] xMonthly = { "J", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };
        private static string[] xYearly = {DateTime.Now.ToString("yy"),DateTime.Now.AddYears(-1).ToString("yy"),
                                             DateTime.Now.AddYears(-2).ToString("yy"),DateTime.Now.AddYears(-3).ToString("yy"),
                                             DateTime.Now.AddYears(-4).ToString("yy")};
        private static Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Orange, Color.Gray, Color.Yellow };

        public static byte[] CreateChart(Store[] stores, string[] cases/* plotted cases*/, int storeDataIndex, string term)
        {
            Chart chart = new Chart();
            ChartConfigure(chart);
            string title;
            if (term.Contains("Day"))
                title = "Daily: " + CubeAdapter.storeDataNames[storeDataIndex];
            else if (term.Contains("Month"))
                title = "Monthly: " + CubeAdapter.storeDataNames[storeDataIndex];
            else
                title = "Yearly: " + CubeAdapter.storeDataNames[storeDataIndex];
            chart.Titles.Add(CreateTitle(title));
            chart.Legends.Add(new Legend("All"));
            chart.Legends[0].Docking = Docking.Top;
            for (int i = 0; i < stores.Length; i++)
            {
                chart.Series.Add(CreateSeries(cases[i], stores[i], colors[i % (colors.Length - 1)], storeDataIndex, term));
            }
            chart.ChartAreas.Add(CreateChartArea());
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            chart.SaveImage(ms);
            return ms.GetBuffer();
        }

        private static Series CreateSeries(string name, Store store, Color color, int storeDataIndex, string term)
        {
            Series series = new Series();
            series.Name = name;
            series.Color = color;
            series.IsVisibleInLegend = true;
            series.IsValueShownAsLabel = false;

            series.ChartType = chartType;
            series.BorderWidth = 2;
            series["DrawingStyle"] = "Cylinder";
            DataPoint point;

            string[] xLegend;
            if (term.Contains("Week Day"))
                xLegend = xDaily;
            else if (term.Contains("Month"))
                xLegend = xMonthly;
            else 
                xLegend = xYearly;

            var yValues = CubeAdapter.Scale(store, storeDataIndex);

            for (int k = 0; k < xLegend.Length; k++)
            {
                point = new DataPoint();
                point.AxisLabel = xLegend[k];
                point.YValues = new double[] { Convert.ToDouble(yValues[k]) };
                series.Points.Add(point);
            }
            series.ChartArea = "Result Chart";
            return series;
        }

        private static void ChartConfigure(Chart chart)
        {
            chart.Width = 300;
            chart.Height = 200;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
        }

        private static Title CreateTitle(string _title)
        {
            Title title = new Title();
            title.Text = _title;
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);
            return title;
        }
        private static ChartArea CreateChartArea()
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Result Chart";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;
            return chartArea;
        }

    }
}