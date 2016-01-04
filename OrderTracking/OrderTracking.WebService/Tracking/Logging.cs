using System;
using log4net;
using OrderTracking.Dao.Domain;

namespace OrderTracking.WebService.Tracking
{
    public static class Logging
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Log));
        const string Message = "log";

        public static string GetStoreId(string username)
        {
            if (username.StartsWith("MONITOR"))
            {
                return username.Substring(7);
            }

            return "?";
        }

        public static void LogNotice(string methodName, Store store, Results results)
        {
            try
            {
                if (results.ErrorMessages != null)
                {
                    foreach (var result in results.ErrorMessages)
                    {
                        LogNotice(methodName, store == null ? "?" : store.ExternalStoreId, "error", result); //Message, result);
                    }
                }

                if (results.WarningMessages != null)
                {
                    foreach (var result in results.WarningMessages)
                    {
                        LogNotice(methodName, store == null ? "?" : store.ExternalStoreId, "warning", result); // Message, result);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static void LogError(string methodName, string storeId, string debugInfo)
        {
            try
            {
                //  #if (DEBUG)  
                LogNotice(methodName, storeId, "error", debugInfo); //Message, debugInfo);
                //  #endif         
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static void LogDebug(string methodName, string storeId, string debugInfo)
        {
            try
            {
              //  #if (DEBUG)  
                LogNotice(methodName, storeId, "debug", debugInfo); //Message, debugInfo);
              //  #endif         
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static void LogInfo(string methodName, string storeId, string message)
        {
            try
            {
                //  #if (DEBUG)  
                LogNotice(methodName, storeId, "info", message);
                //  #endif         
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private static void LogNotice(string methodName, string externalStoreId, string severity, string message) //, string result)
        {
            //log4net.GlobalContext.Properties["storeId"] = externalStoreId;
            //log4net.GlobalContext.Properties["method"] = methodName;
            //log4net.GlobalContext.Properties["severity"] = severity;

            log4net.LogicalThreadContext.Properties["storeId"] = externalStoreId;
            log4net.LogicalThreadContext.Properties["method"] = methodName;
            log4net.LogicalThreadContext.Properties["severity"] = severity;

            if (severity == "info")
            {
                Logger.Info(message);
            }
            else if (severity == "debug")
            {
                Logger.Debug(message);
            }
            else
            {
                Logger.Error(message);
            }

            //if (externalStoreId == null && severity == null)
            //{
            //    //note: this should catch random errors
            //    Logger.Error(message);
            //}

            //Logger.Error(result);
        }
    }
}
