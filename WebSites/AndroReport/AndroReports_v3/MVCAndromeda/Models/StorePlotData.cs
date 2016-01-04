using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAndromeda.Models
{
    // this class is used to store data of stores to make plots
    // Cases and first index of YValues describe number of plots and their legends
    public class StorePlotData
    { 
        public string StoreName {get;set;}
        public string Country{get;set;}
        public string Title { get; set; }
        public string[] Cases { get; set; }
        public string[] XValues { get; set; }
        public double?[][] YValues { get; set; }

        public StorePlotData(string storeName, string storeCountry, string title, string[] cases, string[] xValues, double?[][] yValues)
        {
            StoreName = storeName;
            Country = storeCountry;
            Cases = cases;
            XValues = xValues;
            YValues = yValues;
            Title = title;
        }

    }
}