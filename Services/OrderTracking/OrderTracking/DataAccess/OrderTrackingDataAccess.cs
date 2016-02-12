using OrderTracking.Models;
using OrderTracking.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OrderTracking.DataAccess
{
    internal class OrderTrackingDataAccess
    {
        public static bool GetOrders(List<int> siteIds, string credentials, out List<Order> orders)
        {
            bool success = true;
            orders = new List<Order>();

            if (credentials.Equals("OTTEST", StringComparison.CurrentCultureIgnoreCase))
            {
                // Get the test order
                Order order = CacheManager.GetOTTestOrder();

                orders = new List<Order>();
                orders.Add(order);

                return true;
            }
            else if (credentials.Equals("OTTEST2", StringComparison.CurrentCultureIgnoreCase))
            {
                // Get the test order
                Order order = CacheManager.GetOTTestOrder();
                order.Status = 4;

                orders = new List<Order>();
                orders.Add(order);

                return true;
            }

            // Get a list of stores in the applicationid
          //  List<int> siteIds = ACSDataAccess.GetApplicationStores(applicationId);

            // No sites - no orders - goodbye...
            if (siteIds == null || siteIds.Count == 0) return true;

            // Build a comma delimited list of store ids for the SQL query
            StringBuilder sitesIdsIn = new StringBuilder();
            foreach (int siteId in siteIds)
            {
                if (sitesIdsIn.Length > 0) sitesIdsIn.Append(",");

                sitesIdsIn.Append("'");
                sitesIdsIn.Append(siteId.ToString());
                sitesIdsIn.Append("'");
            }

            string sql =
                "select " +
                "    o.id, " + // as OrderId, " 
                "    os.CreatedDateTime, " + // as OrderTakenDateTime, 
                "    os.DispatchedDateTime, " + // as OutForDeliveryDateTime, 
                "    case when (os.StatusId = 5 or os.StatusId = 6) then os.Time else null end, " + // as CompletedDateTime,
                "    cord2.Latitude, " + // as StoreLatitude,
                "    cord2.Longitude, " + // as StoreLongitude, 
                "    cord.Latitude, " + // as CustomerLatitude,
                "    cord.Longitude, " + // as CustomerLongitude,
                "    os.Processor, " + // as PersonProcessing,
                "    os.StatusId, " + // as OrderStatus     
                "    t.Name " + //as TrackerName " +          
                "from OrderTracking.dbo.tbl_Customer c " +
                "inner join OrderTracking.dbo.tbl_CustomerOrders co " +
                "   on co.CustomerId = c.Id " +
                "inner join OrderTracking.dbo.tbl_Order o " +
                "   on co.OrderId = o.Id " +
                "inner join OrderTracking.dbo.ClientStore cs " +
                "   on cs.StoreId = o.StoreId " +
                "inner join OrderTracking.dbo.tbl_Store s " +
                "   on cs.StoreId = s.Id " +
                "inner join OrderTracking.dbo.Client cl " +
                "   on cl.Id = cs.ClientId " +
                "inner join [OrderTracking].[dbo].[tbl_OrderStatus] os " +
                "   on os.OrderId = o.Id " +
                "left join [OrderTracking].[dbo].[tbl_Coordinates] cord " +
                "   on cord.id = c.Coordinates " +
                "left join [OrderTracking].[dbo].[tbl_Coordinates] cord2 " +
                "   on cord2.id = s.Coordinates " +
                "left join [OrderTracking].[dbo].[tbl_DriverOrders] do " +
                "    on do.OrderId = o.id " +
                "left join [OrderTracking].[dbo].[tbl_Tracker] t " +
                "    on t.DriverId = do.DriverId " +
                "where c.Credentials = @Credentials " +
                "and s.ExternalStoreId in (" + sitesIdsIn.ToString() + ")";

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sql;

                SqlParameter credentialsSqlParameter = new SqlParameter("@Credentials", credentials);
                sqlCommand.Parameters.Add(credentialsSqlParameter);

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Order id
                        Int64 orderId = dataReader.GetInt64(0);

                        // Order taken date/time
                        DateTime? orderTakenDateTime = null;
                        if (!dataReader.IsDBNull(1))
                        {
                            orderTakenDateTime = dataReader.GetDateTime(1);
                        }

                        // Out for delivery date/time
                        DateTime? outForDeliveryDateTime = null;
                        if (!dataReader.IsDBNull(2))
                        {
                            outForDeliveryDateTime = dataReader.GetDateTime(2);
                        }

                        // Order completed date/time
                        DateTime? orderCompletedDateTime = null;
                        if (!dataReader.IsDBNull(3))
                        {
                            orderCompletedDateTime = dataReader.GetDateTime(3);
                        }

                        // Store latitude
                        double? storeLatitude = null;
                        if (!dataReader.IsDBNull(4))
                        {
                            storeLatitude = dataReader.GetDouble(4);
                        }

                        // Store longitude
                        double? storeLongitude = null;
                        if (!dataReader.IsDBNull(5))
                        {
                            storeLongitude = dataReader.GetDouble(5);
                        }

                        // Customer latitude
                        double? customerLatitude = null;
                        if (!dataReader.IsDBNull(6))
                        {
                            customerLatitude = dataReader.GetDouble(6);
                        }

                        // Customer longitude
                        double? customerLongitude = null;
                        if (!dataReader.IsDBNull(7))
                        {
                            customerLongitude = dataReader.GetDouble(7);
                        }

                        // Person processing
                        string personProcessing = dataReader.GetString(8);

                        // Order status
                        Int64 status = dataReader.GetInt64(9);

                        // Tracker name
                        string trackerName = "";
                        if (!dataReader.IsDBNull(10))
                        {
                            trackerName = dataReader.GetString(10);
                        }

                        // Create a new order object
                        Order order = new Order(
                            orderId,
                            orderTakenDateTime,
                            outForDeliveryDateTime,
                            orderCompletedDateTime,
                            storeLatitude,
                            storeLongitude,
                            customerLatitude,
                            customerLongitude,
                            personProcessing,
                            status,
                            trackerName);

                        orders.Add(order);
                    }
                }

                sqlConnection.Close();
            }            

            return success;
        }

        public static bool GetPollOrders(string credentials, string customerOrderIds, out List<PollOrder> pollOrders)
        {
            bool success = true;
            pollOrders = new List<PollOrder>();

            if (credentials.Equals("OTTEST", StringComparison.CurrentCultureIgnoreCase))
            {
                // Get the test order
                Order order = CacheManager.GetOTTestOrder();

                PollOrder pollOrder = new PollOrder(order.OrderId, order.Status, order.CompletedDateTime, "");
                pollOrders.Add(pollOrder);

                return true;
            }
            else if (credentials.Equals("OTTEST2", StringComparison.CurrentCultureIgnoreCase))
            {
                // Get the test order
                Order order = CacheManager.GetOTTestOrder();

                PollOrder pollOrder = new PollOrder(order.OrderId, 4, order.CompletedDateTime, "Test Driver");
                pollOrders.Add(pollOrder);

                return true;
            }

            /*
select 
    os.OrderId as OrderId,
    os.StatusId as OrderStatus, 
    case when (os.StatusId = 5 or os.StatusId = 6) then os.Time else null end as CompletedDateTime,
    t.Name as TrackerName
from OrderTracking.dbo.tbl_OrderStatus os
left join OrderTracking.dbo.tbl_DriverOrders do
    on do.OrderId = os.OrderId
left join OrderTracking.dbo.tbl_Tracker t
    on t.DriverId = do.DriverId
where os.OrderId in (125812,125804)
             */

            string sql =
                "select " +
                "    os.OrderId, " + // as OrderId,
                "    os.StatusId, " + // as OrderStatus, " +
                "    case when (os.StatusId = 5 or os.StatusId = 6) then os.Time else null end, " + // as CompletedDateTime,
                "    t.Name " + // as TrackerName " +
                "from OrderTracking.dbo.tbl_OrderStatus os " +
                "left join OrderTracking.dbo.tbl_DriverOrders do " +
                "    on do.OrderId = os.OrderId " +
                "left join OrderTracking.dbo.tbl_Tracker t " +
                "    on t.DriverId = do.DriverId " +
                "where os.OrderId in (" + customerOrderIds + ")";

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sql;

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // Order id
                        Int64 orderId = dataReader.GetInt64(0);

                        // Status id
                        Int64 statusId = dataReader.GetInt64(1);

                        // Order completed date/time
                        DateTime? orderCompletedDateTime = null;
                        if (!dataReader.IsDBNull(2))
                        {
                            orderCompletedDateTime = dataReader.GetDateTime(2);
                        }

                        // Tracker name
                        string trackerName = "";
                        if (!dataReader.IsDBNull(3))
                        {
                            trackerName = dataReader.GetString(3);
                        }

                        // Create a new order object
                        PollOrder pollOrder = new PollOrder(
                            orderId,
                            statusId,
                            orderCompletedDateTime,
                            trackerName);

                        pollOrders.Add(pollOrder);
                    }
                }

                sqlConnection.Close();
            }

            return success;
        }

        public enum EventTypeEnum { NoError = 0, Informational = 1, Warning = 2, Error = 3 }
        public static bool LogEvent(Exception exception, string extraInfo)
        {
            bool success = true;

            string message = "";
            Exception currentException = exception;
            while (currentException != null)
            {
                message += currentException.Message + "\r\n";
                currentException = currentException.InnerException;
            }

            success = OrderTrackingDataAccess.LogEvent(message, EventTypeEnum.Error, extraInfo, exception.StackTrace);

            return success;
        }

        public static bool LogEvent(string message, EventTypeEnum eventType, string extraInfo, string location)
        {
            bool success = true;

            try
            {
                string sql =
                    "insert into [eventlog] " +
                    "([Message], [EventType], [ExtraInfo], [Location]) " +
                    "values " +
                    "(@message, @eventtype, @extrainfo, @location)";

                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = sql;

                    sqlCommand.Parameters.Add("@message", SqlDbType.NVarChar, 1000).Value = message;
                    sqlCommand.Parameters.Add("@eventtype", SqlDbType.Int).Value = (int)eventType;
                    sqlCommand.Parameters.Add("@extrainfo", SqlDbType.NVarChar).Value = extraInfo;
                    sqlCommand.Parameters.Add("@location", SqlDbType.NVarChar).Value = location;

                    sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}