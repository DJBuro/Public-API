using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AnalysisServices.AdomdClient;
using System.Data;
using System.Globalization;
using System.Diagnostics;

namespace MVCAndromeda.Models
{
    public static class CubeAdapter
    {
        private static string cubeName = "Daily Reporting";
        // All arrays below must have equal length
        // These are names of measures in the cube. Note average ones must include suffix "Avg"
        public static readonly string[] storeDBMeasures = { "Total Orders", "Net Sales", "Spent Avg", "Out Door < 30m Avg", 
                                                              "Door Time Avg", "Out The Door Avg", "Total Cancels", "Value Cancels" };
        // These are names of the measures used here in plots
        public static readonly string[] storeMeasureNames = { "Orders", "Sales", "Average Spent", "Out Door < 30 min (%)", "Door Time (min)", 
                                                             "Out The Door (min)", "Total Cancels", "Value Cancels" };
        // These are data formats for the measures, in particular, G - general is just a number, C- currency for particular culture
        public static readonly string[] storeMeasureFormat = { "G", "C", "C", "G", "G", "G", "G", "C" };
        // Scales for the measures, e.g. pence to pound, seconds to minutes
        public static readonly int[] storeMeasureScale = { 1, 100, 100, 1, 60, 60, 1, 100 };

        //-------------scale Store Data, e.g. pence to pounds--------------------------
        public static double?[] Scale(Store store, int storeMeasureIndex)
        {
            double?[] result = new double?[store.StoreMeasures[storeMeasureIndex].Count];
            for (int k = 0; k < store.StoreMeasures[storeMeasureIndex].Count; k++)
            {
                if (store.StoreMeasures[storeMeasureIndex][k] != null)
                    result[k] = (double)store.StoreMeasures[storeMeasureIndex][k] / (double)storeMeasureScale[storeMeasureIndex];
            }
            return result;
        }
        //----------------------get all user's chains descendants incl chains themselves------------------
        private static List<string> getAllOwnerChainDescendants(List<string> chains)
        {
            var ownerChainDescendants = new List<string>();
            var collectChains = new StringBuilder("{");
            foreach (var chain in chains)
                collectChains.Append("DESCENDANTS(chains.[Parent ID].[" + chain + "], chains.[Parent ID].[" + chain + "], SELF_BEFORE_AFTER), ");
            collectChains.Length -= 2;
            collectChains.Append("}");
            getMemberNames("chains.[Parent ID]", collectChains.ToString(), cubeName, ref ownerChainDescendants);
            return ownerChainDescendants;
        }
        //----------------------get all chains referred by stores as their chains------------------
        private static List<string> getAllLeafChains()
        {
            var allLeafChains = new List<string>();
            getMemberNames("stores.[Chain-Store]",
               "DESCENDANTS(stores.[Chain-Store], stores.[Chain-Store].Chain)",
               cubeName, ref allLeafChains);
            return allLeafChains.Count == 0 ? null : allLeafChains;
        }
        //----------------------get list of store names for an owner------------------
        private static List<string> getStoreNames(List<string> chains)
        {
            var storeNames = new List<string>();
            var ownerAllChainDescendants = getAllOwnerChainDescendants(chains);
            var allLeafChains = getAllLeafChains();
            var leafChainNames = allLeafChains.Intersect(ownerAllChainDescendants);

            var rows = new StringBuilder("{");
            foreach (var leafName in leafChainNames)
                rows.Append("[Stores].[Chain-Store].[").Append(leafName).Append("].CHILDREN, ");
            rows.Length -= 2;
            rows.Append("} ");

            getMemberNames("stores.[Chain-Store]", rows.ToString(), cubeName, ref storeNames);

            return storeNames.Count == 0 ? null : storeNames;
        }

        //-----------------------performs generic query to get names of measures----------------------------
        private static void getMemberNames(string column, string rows, string cube, ref List<string> result)
        {
            string queryString = "WITH " +
               "MEMBER [Measures].[Label] AS " + column + ".CURRENTMEMBER.MEMBER_CAPTION " +
               "SELECT {[Measures].[Label]} ON COLUMNS, " + rows + " ON ROWS FROM [" + cubeName + "]";

            DataTable dataTable = connectAndQuery(queryString);

            /* 
             // writing table returned by query 
             Debug.WriteLine(queryString);

             foreach (DataRow dr in dataTable.Rows)
             {
                 foreach (DataColumn dc in dataTable.Columns)
                     Debug.Write(dr[dc] + " | ");
                 Debug.WriteLine("");
             }*/

            foreach (DataRow row in dataTable.Rows)
                result.Add((string)row[dataTable.Columns.Count - 1]);
            result.Sort();
        }
        //----------------------get list of countries and stores in them for an owner------------------
        public static Dictionary<string, List<string>> GetCountriesAndStores(List<string> chains)
        {
            var countries = new Dictionary<string, List<string>>();
            // Get  list of countries
            var storeNames = getStoreNames(chains);

            foreach (var storeName in storeNames)
            {
                var countryForStore = new List<string>();
                getMemberNames("Stores.stores", "ANCESTORS([stores].[stores].[" + storeName + "],[stores].[stores].[country])",
                    cubeName, ref countryForStore);

                string country = countryForStore[0];// expect one country only

                if (!countries.Keys.Contains(country))
                    countries[country] = new List<string>();

                countries[country].Add(storeName);

            }
            if (countries.Count == 0) return null;
            return countries;
        }
        //------------------------filling Owner with all-store measures----------
        public static void fillStoreMeasures(DateTime today, Owner owner, string term, int unitsAgo)
        {
            var storenames = owner.Stores.Keys.ToList<string>();
            int j = 0;
            foreach (string storeDBMeasure in storeDBMeasures)
            {
                List<List<long?>> resultSet = getStoreMeasures(today, storenames, term, storeDBMeasure, unitsAgo);
                int i = 0;
                foreach (var name in storenames)
                    (owner.Stores[name])[storeMeasureNames[j]] = resultSet[i++];
                j++;
            }
        }
        //-----------------------fill one Store with measures--------------------------------
        public static Store fillStoreMeasures(DateTime today, string storeName, string country, string term, int unitsAgo)
        {
            var store = new Store(storeName, country);
            var storenames = new List<string>() { store.StoreName };
            int j = 0;
            foreach (string storeDBMeasure in storeDBMeasures)
            {
                List<List<long?>> resultSet = getStoreMeasures(today, storenames, term, storeDBMeasure, unitsAgo);
                store[storeMeasureNames[j++]] = resultSet[0];
            }
            return store;
        }
        //------------------getting measures from the cube, for single values returns a list of single-sized list----
        //                 returns list corresponding to stores with the inner list containin the measure at date
        //                 for term=Day,Month or Year the list has one element, for Week Day - 7 elements For Month For Year - 12 elements
        private static List<List<long?>> getStoreMeasures(DateTime today, List<string> storeNames, string term, string measure, int unitsAgo)
        {
            string dimension;// dimension member in the cube
            int startIndex;  // column index of the first store in resulting table
            // dimension members depends on the term:
            GetDateDimensionMember(today, term, unitsAgo, out dimension, out startIndex, ref measure);

            StringBuilder queryString = new StringBuilder(dimension + " ON ROWS, {");
            //var distinct = storeNames.Distinct();
            foreach (string storeName in storeNames)
                queryString.Append("[Stores].[Stores].[").Append(storeName.Trim()).Append("], ");
            
            queryString.Length -= 2;
            queryString.Append("} ON COLUMNS FROM [" + cubeName + "] WHERE [Measures].[" + measure + "]");


            DataTable dataTable = connectAndQuery(queryString.ToString());

            //PrintQueryResults(storeNames, startIndex, queryString.ToString(), dataTable);

            var storeResultList = new List<List<long?>>();
            // loop over stores
            for (int storeIndex = 0; storeIndex < (dataTable.Columns.Count) - startIndex; storeIndex++)
            {
                var timeResult = new List<long?>();
                //  loop over date rows: one rows for Day, Month, Year, 7 for Week Day, 11 for Month of year
                for (int timeIndex = 0; timeIndex < dataTable.Rows.Count; timeIndex++)
                {   // Measures come in different types. Must convert to long
                    if (dataTable.Rows[timeIndex][startIndex + storeIndex] is DBNull)
                        timeResult.Add(null);
                    else if (dataTable.Rows[timeIndex][startIndex + storeIndex] is Double)
                        timeResult.Add(Convert.ToInt64((double)dataTable.Rows[timeIndex][startIndex + storeIndex]));
                    else if (dataTable.Rows[timeIndex][startIndex + storeIndex] is Int32)
                        timeResult.Add(Convert.ToInt64((Int32)dataTable.Rows[timeIndex][startIndex + storeIndex]));
                    else if (dataTable.Rows[timeIndex][startIndex + storeIndex] is Int16)
                        timeResult.Add(Convert.ToInt64((Int16)dataTable.Rows[timeIndex][startIndex + storeIndex]));
                    else timeResult.Add(null);
                }
                storeResultList.Add(timeResult);
            }
            return storeResultList;
        }

        //---------------testing-------------------------------------------
        private static void PrintQueryResults(List<string> storeNames, int startIndex, string queryString, DataTable dataTable)
        {
            Debug.WriteLine("------------------------------------------");
            Debug.WriteLine(queryString);
            Debug.WriteLine("------------------------------------------");
            foreach (DataRow dr in dataTable.Rows)
            {
                foreach (DataColumn dc in dataTable.Columns)
                    Debug.Write(dr[dc] + " | ");
                Debug.WriteLine("");
            }
            string[] timePoint;
            if (dataTable.Rows.Count == 1)
                timePoint = new string[] { "" };
            else if (dataTable.Rows.Count == 7)
                timePoint = DateUtilities.weekDays;
            else
                timePoint = DateUtilities.months;
            for (int storeIndex = 0; storeIndex < (dataTable.Columns.Count) - startIndex; storeIndex++)
            {
                for (int timeIndex = 0; timeIndex < dataTable.Rows.Count; timeIndex++)
                    Debug.WriteLine(timePoint[timeIndex] + " " + storeNames[storeIndex] + ": " + dataTable.Rows[timeIndex][startIndex + storeIndex] + "of type:" +
                        dataTable.Rows[timeIndex][startIndex + storeIndex].GetType());
            }
        }

        // ----------------dimension members depends on the term: it can give a date list, e.g. for week days {Mon, Tues, etc}-----------
        private static void GetDateDimensionMember(DateTime today, string term, int unitsAgo, out string dimension
            , out int startIndex, ref string measure)
        {
            string day, week, month, year;
            int intWeek, intYear;
            bool firstNonFullWeekOfYear, lastNonFullWeekOfYear;

            dimension = "SELECT {[Time].";
            DateUtilities.ParseDate(today, term, unitsAgo, out day, out week, out month, out year, out intWeek
                , out intYear, out firstNonFullWeekOfYear, out lastNonFullWeekOfYear);
            startIndex = 0; // column where the store list start in resulting table. E.G. |year|month|store1|store2|
                            // the rows correspond to dates. one date for Day, Week,etc and multiple for Week Days, Months of Year
            switch (term)
            {
                case ("Day"):       //=DataUtilities.averagingPeriod[0]
                    dimension += "[Year-Month-Day].[" + year + "].[" + month + "].[" + day + "]}";
                    startIndex = 3;
                    break;
                case ("Week"):      //=DataUtilities.averagingPeriod[1]
                    if (firstNonFullWeekOfYear)
                    {
                        string newMember = "[Time].[Year-Week].[" + year + "].[Full Week]";
                        string aggregation = measure.Contains("Avg") ? "AVG" : "SUM";
                        dimension = "WITH MEMBER " + newMember + " AS (" + aggregation + "({[Time].[Year-Week-Weekday].[" + year +
                            "].FIRSTCHILD.CHILDREN, [Time].[Year-Week-Weekday].[Calendar " + (intYear - 1).ToString() + "].LASTCHILD.CHILDREN}))"
                            + "SELECT " + newMember;
                    }
                    else
                        dimension += "[Year-Week].[" + year + "].[" + week + "]}";
                    startIndex = 2;
                    break;
                case ("Month"):     //=DataUtilities.averagingPeriod[2]
                    dimension += "[Year-Month-Day].[" + year + "].[" + month + "]}";
                    startIndex = 2;
                    break;
                case ("Year"):      //=DataUtilities.averagingPeriod[3]
                    dimension += "[Year-Month-Day].[" + year + "]}";
                    startIndex = 1;
                    break;
                case ("Week Day for Week"):     //=DataUtilities.weekDayAveragingPeriod[0]
                    // Tackle non-full weeks around New Year
                    if (firstNonFullWeekOfYear) // first non-full week of year
                    {
                        dimension = dimension + "[Year-Week-WeekDay].[Calendar " + (intYear - 1).ToString() + "].[Week "
                            + DateUtilities.numOfLastWeek(intYear).ToString() + "].CHILDREN, "
                            + dimension.Replace("SELECT {", "") + "[Year-Week-WeekDay].[" + year + "].[" + week + "].CHILDREN}";
                    }
                    else if (lastNonFullWeekOfYear)// last non-full week of year
                    {
                        dimension = dimension + "[Year-Week-WeekDay].[" + year + "].[" + week + "].CHILDREN, "
                            + dimension.Replace("SELECT {", "") + "[Year-Week-WeekDay].[Calendar " + (intYear + 1).ToString() + "].[Week 1].CHILDREN}";
                    }
                    else  // normal week*/
                    {
                        dimension += "[Year-Week-WeekDay].[" + year + "].[" + week + "].CHILDREN}";
                    }
                    startIndex = 3;
                    break;
                case ("Week Day for Month"):        //=DataUtilities.weekDayAveragingPeriod[1]
                    dimension += "[Year-Month-WeekDay].[" + year + "].[" + month + "].CHILDREN}";
                    startIndex = 3;
                    break;
                case ("Week Day for Year"):         //=DataUtilities.weekDayAveragingPeriod[2]
                    dimension += "[Year-WeekDay].[" + year + "].CHILDREN}";
                    startIndex = 2;
                    break;
                case ("Month for Year"):            //=DataUtilities.weekDayAveragingPeriod[3]
                    dimension += "[Year-Month-Day].[" + year + "].CHILDREN}";
                    startIndex = 2;
                    break;
            }
            if (term.Contains("for") && !measure.Contains(" Avg"))
                measure += " Avg";
        }

        //-----------------------execute MDX query string-----------------------------------------
        private static DataTable connectAndQuery(string queryString)
        {
            //using (var conn = new AdomdConnection(connectionString))
            using (var conn = new AdomdConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cube"].ConnectionString))
            {
                conn.Open();
                var dataSet = new DataSet();
                using (var adapter = new AdomdDataAdapter(queryString, conn))
                    adapter.Fill(dataSet);
                return dataSet.Tables["Table"];
            }
        }
    }
}
