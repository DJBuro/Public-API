using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Core
{
    public static class Authorisation
    {
        public static Results FailedLogin(this Results results)
        {
            //todo: logging from here???
            results.Success = false;

            if(results.ErrorMessages == null)
            {
                results.ErrorMessages = new List<string>();
            }

            results.ErrorMessages.Add("Invalid accountName, userName, or password");

            return results;
        }

        public static Results ItemCannotBeFoundError(this Results results, string itemName, string variable)
        {
            results.Success = false;

            if (results.ErrorMessages == null)
            {
                results.ErrorMessages = new List<string>();
            }

            results.ErrorMessages.Add(string.Format("{0} '{1}' cannot be found", itemName, variable));

            return results;
        }

        public static Results AddHocError(this Results results, string error)
        {
            results.Success = false;

            if (results.ErrorMessages == null)
            {
                results.ErrorMessages = new List<string>();
            }

            

            results.ErrorMessages.Add(string.Format("error: {0}", error));

            return results;
        }

        public static Results AddHocWarning(this Results results, string warning)
        {
            results.Success = true;

            if (results.WarningMessages == null)
            {
                results.WarningMessages = new List<string>();
            }

            results.WarningMessages.Add(string.Format("warning: {0}", warning));

            return results;
        }

    }
}
