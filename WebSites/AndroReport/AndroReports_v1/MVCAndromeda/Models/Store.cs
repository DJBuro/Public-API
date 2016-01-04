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

        private List<long?>[] storeData;
        public List<long?>[] StoreData
        {
            get
            {
                return storeData;
            }
        }
        static public readonly string[] storeDataNames = CubeAdapter.storeDataNames;

        public List<long?> this[string s]
        {
            get
            {
                if (!storeDataNames.Contains(s)) throw new Exception("Assigning to non-existing Store Data Field");
                return storeData[Array.IndexOf(storeDataNames, s)];
            }
            set
            {
                if (!storeDataNames.Contains(s)) throw new Exception("Reading from non-existing Store Data Field");
                storeData[Array.IndexOf(storeDataNames, s)] = value;
            }
        }

        public Store(string storename, string country)
        {
            StoreName = storename;
            Country = country;
            storeData = new List<long?>[storeDataNames.Length];
        }


        public new string ToString()
        {
            StringBuilder data = new StringBuilder("");
            foreach (var storeDataName in storeDataNames)
            {
                data.Append(storeDataName + ": ");

                if (this[storeDataName].Count == 1)

                    data.Append(this[storeDataName][0]).Append(", ");
                else if (this[storeDataName].Count == 7)
                {
                    int k = 0;
                    foreach (var field in this[storeDataName])
                        data.Append(DateUtilities.weekDays[k++] + ": ").Append(field + ",");
                }
                else //(this[storeDataName].Count == 7)
                {
                    int k = 0;
                   
                    foreach (var field in this[storeDataName])
                        data.Append(DateUtilities.months[k++] + ": ").Append(field + ", ");
                }

            }
            return "Store: " + StoreName + ",\n\rCountry: " + Country + "\n\r" + data;
        }
    }
}
