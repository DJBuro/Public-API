
using log4net;
using OrderTracking.Dao.Domain;

namespace OrderTrackingAdmin
{
    public static class Logging
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Log));
        const string Message = "log";

        public static void Test(this string externalId)
        {
        }

        public static void LogNotice(string methodName, string externalStoreId, string severity, string message, string result)
        {
            log4net.GlobalContext.Properties["storeId"] = externalStoreId;
            log4net.GlobalContext.Properties["method"] = methodName;
            log4net.GlobalContext.Properties["severity"] = severity;


            if (externalStoreId == null && severity == null)
            {
                //note: this should catch random errors
                Logger.Error(message);
            }

            Logger.Error(result);
        }

    }
}
