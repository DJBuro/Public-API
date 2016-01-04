using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.IO;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Configuration;

namespace Andromeda.GPSGateServices
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class GPSGateTrackerServices
    {
        //private const string server = "94.236.121.24";
        //private const string username = "Order7rack1n6U3er";
        //private const string password = "Order7rack1n6Pa55";
        //private const string database = "GpsGateServer";
        
        [WebGet(UriTemplate = "Trackers")]
        [OperationContract]
        public Stream Trackers()
        {
            Stream responseStream = null;
            StringBuilder outputXml = new StringBuilder();

            try
            {
                string sql =
                    "select [device_name], [latitude], [longitude], [time_stamp] " +
                    "from device " +
                    "where [latitude] is not null " +
                    "and [longitude] is not null " +
                    "and time_stamp > DATEADD(SECOND, -20, GETUTCDATE())";

                string connectionString = "";

                ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["ConnectionString"];

                if (connectionStringSettings == null)
                {
                    outputXml.Append("Missing connection string");
                }

                if (outputXml.Length == 0)
                {
                    if (connectionStringSettings.ConnectionString == null)
                    {
                        outputXml.Append("Missing connection string");
                    }
                    else
                    {
                        connectionString = connectionStringSettings.ConnectionString;
                    }
                }

                if (outputXml.Length == 0)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = sql;

                        outputXml.Append("<Trackers>");

                        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                outputXml.Append("<Tracker name=\"");
                                if (!dataReader.IsDBNull(0))
                                {
                                    outputXml.Append(dataReader.GetString(0));
                                }
                                outputXml.Append("\" lat=\"");
                                if (!dataReader.IsDBNull(1))
                                {
                                    outputXml.Append(dataReader.GetDouble(1));
                                }
                                outputXml.Append("\" lon=\"");
                                if (!dataReader.IsDBNull(2))
                                {
                                    outputXml.Append(dataReader.GetDouble(2));
                                }
                                outputXml.Append("\" />");
                            }
                        }
                        outputXml.Append("</Trackers>");
                    }
                }
                
                // Tell the caller that we're returning xml
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
            }
            catch (Exception exception)
            {
                // Return an error
                try 
                {
                    outputXml.Append(exception.ToString());
                    
                    //responseStream = ResponseHelper.GetResponseError<SubscriptionResponse>((IResponse)new SubscriptionResponse()); 
                }
                catch { }
            }

            // Stream the xml to the caller
            byte[] returnBytes = Encoding.UTF8.GetBytes(outputXml.ToString());
            responseStream = new MemoryStream(returnBytes);

            return responseStream;
        }
    }
}
