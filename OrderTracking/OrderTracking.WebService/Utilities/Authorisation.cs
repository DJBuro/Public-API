using System.Collections.Generic;
using OrderTracking.Dao.Domain;
using OrderTracking.WebService.Tracking;

namespace OrderTracking.Core
{
    public static class Authorisation
    {
        public static Results FailedLogin(this Results results, string username, string password, string method)
        {
            results.Success = false;

            if(results.ErrorMessages == null)
            {
                results.ErrorMessages = new List<string>();
            }

            results.ErrorMessages.Add(string.Format("Invalid userName '{0}', or password '{1}'",username,password));

            Logging.LogError(method + " > Login", Logging.GetStoreId(username), string.Format("Invalid userName '{0}', or password '{1}'", username, password));
//            Logging.LogNotice("Login", null, results);

            return results;
        }

        public static Results ItemCannotBeFoundError(this Results results, string methodName, string itemName, string variable, Store store)
        {
            results.Success = false;

            if (results.ErrorMessages == null)
            {
                results.ErrorMessages = new List<string>();
            }

            results.ErrorMessages.Add(string.Format("{0}: {1} '{2}' cannot be found", methodName, itemName, variable));

            Logging.LogNotice(methodName,store,results);

            return results;
        }

        public static Results AddHocError(this Results results, string methodName, string error, Store store)
        {
            results.Success = false;

            if (results.ErrorMessages == null)
            {
                results.ErrorMessages = new List<string>();
            }

            results.ErrorMessages.Add(string.Format("{0}: {1}", methodName , error));

            Logging.LogNotice(methodName, store, results);

            return results;
        }

        public static Results AddHocWarning(this Results results, string methodName, string warning, Store store)
        {
            results.Success = true;

            if (results.WarningMessages == null)
            {
                results.WarningMessages = new List<string>();
            }

            results.WarningMessages.Add(string.Format("{0}: warning: {1}", methodName, warning));

            Logging.LogNotice(methodName, store, results);

            return results;
        }

        public static Results AddHocWarning(this Results results, string methodName, string warning, string storeMonitor)
        {
            var store = new Store {ExternalStoreId = storeMonitor};

            return AddHocWarning(results, methodName, warning, store);
            
        }

        public static Results AddHocDebug(this Results results, string methodName, string debugInfo, Store store)
        {
            Logging.LogDebug(methodName, store != null ? store.ExternalStoreId : "?", debugInfo);

            return results;
            }

    }
}
