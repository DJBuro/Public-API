using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCAndromeda.Models
{
    public class Store
    {
        public string StoreName { get; set; }
        public string Country { get; set; }
        static public readonly string[] storeMeasureNames = CubeAdapter.storeMeasureNames;
        private List<long?>[] storeMeasures; // all measures

        public List<long?>[] StoreMeasures
        {
            get
            {
                return storeMeasures;
            }
        }

        //-------gets/sets store measure for a given data name--------------------------
        public List<long?> this[string storeMeasureName]
        {
            get
            {
                if (!storeMeasureNames.Contains(storeMeasureName)) throw new Exception("Assigning to non-existing Store Measure Field");
                return storeMeasures[Array.IndexOf(storeMeasureNames, storeMeasureName)];
            }
            set
            {
                if (!storeMeasureNames.Contains(storeMeasureName)) throw new Exception("Reading from non-existing Store Measure Field");
                storeMeasures[Array.IndexOf(storeMeasureNames, storeMeasureName)] = value;
            }
        }
        //-------------Constructor-------------------------------------------
        public Store(string storename, string country)
        {
            StoreName = storename;
            Country = country;
            storeMeasures = new List<long?>[storeMeasureNames.Length];
        }

        //------------------------------------for printing --------------------------------
        public new string ToString()
        {
            StringBuilder data = new StringBuilder("");
            foreach (var storeMeasureName in storeMeasureNames)
            {
                data.Append(storeMeasureName + ": ");

                if (this[storeMeasureName].Count == 1)

                    data.Append(this[storeMeasureName][0]).Append(", ");
                else if (this[storeMeasureName].Count == 7)
                {
                    int k = 0;
                    foreach (var field in this[storeMeasureName])
                        data.Append(DateUtilities.weekDays[k++] + ": ").Append(field + ",");
                }
                else //(this[storeMeasureName].Count == 7)
                {
                    int k = 0;

                    foreach (var field in this[storeMeasureName])
                        data.Append(DateUtilities.months[k++] + ": ").Append(field + ", ");
                }

            }
            return "Store: " + StoreName + ",\n\rCountry: " + Country + "\n\r" + data;
        }
    }
}
