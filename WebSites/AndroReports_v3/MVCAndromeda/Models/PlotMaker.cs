using System;

namespace MVCAndromeda.Models
{
    public class PlotMaker
    {
        //X -legend
        private static string[] xDaily = { "M", "T", "W", "T", "F", "S", "S" };
        private static string[] xMonthly = { "J", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };
        private static string[] xYearly = {DateTime.Now.ToString("yy"),DateTime.Now.AddYears(-1).ToString("yy"),
                                             DateTime.Now.AddYears(-2).ToString("yy"),DateTime.Now.AddYears(-3).ToString("yy"),
                                             DateTime.Now.AddYears(-4).ToString("yy")};

        public static StorePlotData getStorePlotData(Store[] stores, string[] cases/* plotted cases*/, int storeMeasureIndex, string term)
        {
            string title;
            if (term.Contains("Day"))
                title = "Daily: " + CubeAdapter.storeMeasureNames[storeMeasureIndex];
            else if (term.Contains("Month"))
                title = "Monthly: " + CubeAdapter.storeMeasureNames[storeMeasureIndex];
            else
                title = "Yearly: " + CubeAdapter.storeMeasureNames[storeMeasureIndex];

            string[] xLegend;
            if (term.Contains("Week Day"))
                xLegend = xDaily;
            else if (term.Contains("Month"))
                xLegend = xMonthly;
            else
                xLegend = xYearly;
            if (term.Contains("Day"))
                title = "Daily: " + CubeAdapter.storeMeasureNames[storeMeasureIndex];
            else if (term.Contains("Month"))
                title = "Monthly: " + CubeAdapter.storeMeasureNames[storeMeasureIndex];
            else
                title = "Yearly: " + CubeAdapter.storeMeasureNames[storeMeasureIndex];
            double?[][] yValues = new double?[stores.Length][];
            for (int i=0;i<stores.Length;i++)
            yValues[i] = CubeAdapter.Scale(stores[i], storeMeasureIndex);
            return new StorePlotData(stores[0].StoreName, stores[0].Country, title,cases, xLegend, yValues);
        }
    }
}