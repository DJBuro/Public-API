using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AnalysisServices.AdomdClient;
using System.Data;
using System.Globalization;
using System.Diagnostics;

namespace mdx
{
    public class CubeAdapter
    {
        private static string
            dataSource = "local",
            cubeName = "PJ Moscow",
            initialCatalog = "PJ Moscow",
            provider = "MSOLAP",
            connectionString = "Provider=" + provider + "; Initial Catalog=" + initialCatalog + "; Data Source=" + dataSource;

        private string defaultOwnerName = "Papa John's";
        private List<string> owners = null;

        public static readonly string[] storeDBMeasures = { "Total Orders", "Net Sales", "Spent Avg", "Out Door < 30m Avg", "Door Time Avg", "Out The Door Avg", "Total Cancels", "Value Cancels" };// measure names in the cube
        public static readonly string[] storeDataNames = { "Orders", "Sales", "Average Spent", "Out Door < 30 min (%)", "Door Time (min)", "Out The Door (min)", "Total Cancels", "Value Cancels" };// measure names on our web page
        public static readonly string[] storeDataFormat = { "G", "C", "C", "G", "G", "G", "G", "C" };
        public static readonly int[] storeDataScale = { 1, 100, 100, 1, 60, 60, 1, 100 };
        public static readonly string cultInfo = "ru-Ru";

        //-------------scale Store Data, e.g. pence to pounds--------------------------
        public static double?[] Scale(Store store, int storeDataIndex) //we are losing decimals here!
        {
            double?[] result = new double?[store.StoreData[storeDataIndex].Count];
            for (int k = 0; k < store.StoreData[storeDataIndex].Count; k++)
            {
                if (store.StoreData[storeDataIndex][k] != null)
                    result[k] = (double)store.StoreData[storeDataIndex][k] / (double)storeDataScale[storeDataIndex];
            }
            return result;
        }

        //----------------------get list of countries and stores in them for an owner------------------
        public Dictionary<string, List<string>> GetCountriesAndStores(string ownerName)
        {
            var countries = new Dictionary<string, List<string>>();
            // Get  list of countries
            if (ownerName == defaultOwnerName) ownerName = "";
            string queryString = "WITH " +
                    "MEMBER [Measures].[Label] AS [Stores].[stores].CURRENTMEMBER.MEMBER_CAPTION " +
                    "SELECT " +
                        "{[Measures].[Label]} ON COLUMNS , " +
                        "[stores].[stores].[" + ownerName + "].CHILDREN ON ROWS " +
                    "FROM [" + cubeName + "]";

            DataTable dataTable = connectAndQuery(queryString);

            int countryIndex = 1;//in resulting table
            int storeNameIndex = countryIndex + 1;

            foreach (DataRow row in dataTable.Rows)
            {
                string country = (string)row[countryIndex];
                // get list of storenames for a given country
                queryString = "WITH " +
                    "MEMBER [Measures].[Label] AS [Stores].[stores].CURRENTMEMBER.MEMBER_CAPTION " +
                    "SELECT " +
                         "{[Measures].[Label]} ON COLUMNS , " +
                        "[stores].[stores].[" + ownerName + "].[" + country + "].CHILDREN ON ROWS " +
                     "FROM [" + cubeName + "]";

                DataTable datTable = connectAndQuery(queryString);

                var storeNames = new List<string>();
                foreach (DataRow row1 in datTable.Rows)
                    storeNames.Add((string)row1[storeNameIndex]);

                countries.Add(country, storeNames);
            }

            if (countries.Count == 0) return null;
            return countries;
        }


        // -------------get list of all owners--------------------------------
        public List<string> Owners
        {
            get
            {
                if (owners == null)
                {
                    owners = new List<string>();
                    // get list of owner names
                    string queryString =
                        "WITH " +
                        "MEMBER [Measures].[Label] AS [Stores].[stores].CURRENTMEMBER.MEMBER_CAPTION " +
                        "SELECT " +
                            "{[Measures].[Label]} ON COLUMNS , " +
                            "[stores].[stores].[owner].ALLMEMBERS ON ROWS " +
                        "FROM [" + cubeName + "]";

                    DataTable dataTable = connectAndQuery(queryString);

                    int ownerNameIndex = 0;//in resulting table
                    foreach (DataRow row in dataTable.Rows)
                        owners.Add(((string)row[ownerNameIndex]).Trim() == "" ? defaultOwnerName : (string)row[ownerNameIndex]);

                    owners.Sort();
                    if (owners.Count == 0) owners = null;

                }
                return owners;
            }
        }

        //------------------------filling Owner with store data----------
        public void FillStoreData(ref Owner owner, string term, int unitsAgo)
        {
            var storenames = owner.Stores.Keys.ToList<string>();
            int j = 0;
            foreach (string storeDBMeasure in storeDBMeasures)
            {
                List<List<long?>> resultSet = getStoreData(storenames, term, storeDBMeasure, unitsAgo);
                int i = 0;
                foreach (var name in storenames)
                    (owner.Stores[name])[storeDataNames[j]] = resultSet[i++];
                j++;
            }
        }
        //-----------------------fill one Store with data--------------------------------
        public Store FillStoreData(string storeName, string country, string term, int unitsAgo)
        {
            var store = new Store(storeName, country);
            var storenames = new List<string>() { store.StoreName };
            int j = 0;
            foreach (string storeDBMeasure in storeDBMeasures)
            {
                List<List<long?>> resultSet = getStoreData(storenames, term, storeDBMeasure, unitsAgo);
                store[storeDataNames[j++]] = resultSet[0];
            }
            return store;
        }
        //------------------getting data from the cube, for single values returns a list of single-sized list----
        private List<List<long?>> getStoreData(List<string> storeNames, string term, string measure, int unitsAgo)
        {
            string dimension;// deminsion member in the cube
            int startIndex;  // column index of the first store in resulting table
            // dimension members depends on the term: it can give a date lit, e.g. for week days
            GetDateDimensionMember(term, unitsAgo, out dimension, out startIndex, ref measure);

            StringBuilder queryString = new StringBuilder(dimension + " ON ROWS, {");
            foreach (string storeName in storeNames)
                queryString.Append("[Stores].[Stores].[").Append(storeName).Append("], ");
            queryString.Length -= 2;
            queryString.Append("} ON COLUMNS FROM [" + cubeName + "] WHERE [Measures].[" + measure + "]");


            DataTable dataTable = connectAndQuery(queryString.ToString());

            //PrintQueryResults(storeNames, startIndex, queryString.ToString(), dataTable);

            var storeResultList = new List<List<long?>>();

            for (int storeIndex = 0; storeIndex < (dataTable.Columns.Count) - startIndex; storeIndex++)
            {
                var timeResult = new List<long?>();
                // 
                for (int timeIndex = 0; timeIndex < dataTable.Rows.Count; timeIndex++)
                {
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

        // ----------------dimension members depends on the term: it can give a date lit, e.g. for week days-----------------------
        private static void GetDateDimensionMember(string term, int unitsAgo, out string dimension, out int startIndex, ref string measure)
        {
            string day, week, month, year;
            int intWeek, intYear;
            Boolean firstNonFullWeek, lastNonFullWeek;

            dimension = "SELECT {[Time].";
            DateUtilities.ParseDate(term, unitsAgo, out day, out week, out month, out year, out intWeek, out intYear, out firstNonFullWeek, out lastNonFullWeek);
            startIndex = 0;
            switch (term)
            {
                case ("Day"):
                    dimension += "[Year-Month-Day].[" + year + "].[" + month + "].[" + day + "]}";
                    startIndex = 3;
                    break;
                case ("Week"):
                    if (firstNonFullWeek)
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
                case ("Month"):
                    dimension += "[Year-Month-Day].[" + year + "].[" + month + "]}";
                    startIndex = 2;
                    break;
                case ("Year"):
                    dimension += "[Year-Month-Day].[" + year + "]}";
                    startIndex = 1;
                    break;
                case ("Week Day for Week"):
                    // Tackle non-full weeks around New Year
                    if (firstNonFullWeek) // first non-full week of year
                    {
                        dimension = dimension + "[Year-Week-WeekDay].[Calendar " + (intYear - 1).ToString() + "].[Week "
                            + DateUtilities.numOfLastWeek(intYear).ToString() + "].CHILDREN, "
                            + dimension.Replace("SELECT {", "") + "[Year-Week-WeekDay].[" + year + "].[" + week + "].CHILDREN}";
                    }
                    else if (lastNonFullWeek)// last non-full week of year
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
                case ("Week Day for Month"):
                    dimension += "[Year-Month-WeekDay].[" + year + "].[" + month + "].CHILDREN}";
                    startIndex = 3;
                    break;
                case ("Week Day for Year"):
                    dimension += "[Year-WeekDay].[" + year + "].CHILDREN}";
                    startIndex = 2;
                    break;
                case ("Month for Year"):
                    dimension += "[Year-Month-Day].[" + year + "].CHILDREN}";
                    startIndex = 2;
                    break;
            }
            if (term.Contains("for") && !measure.Contains(" Avg"))
                measure += " Avg";
        }



        private DataTable connectAndQuery(string queryString)
        {
            using (var conn = new AdomdConnection(connectionString))
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
