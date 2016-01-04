using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.Domain;
using AA = AndroAdminDataAccess.EntityFramework;
using AndroAdminDataAccess.DataAccess;

namespace Andromeda.GPSIntegration
{
    public class ErrorHelper
    {
        public static void LogError(string method, string message, string storeId = null)
        {
            try
            {
                // Get an object that can talk to the database
                AA.EntityFrameworkDataAccessFactory entityFrameworkDataAccessFactory = new AA.EntityFrameworkDataAccessFactory();
                
                // Create the log
                AndroAdminDataAccess.Domain.Log log = new AndroAdminDataAccess.Domain.Log();
                log.Source = "AndroAdmin";
                log.Method = method;
                log.Severity = "ERROR";
                log.StoreId = storeId;
                log.Created = DateTime.UtcNow;
                log.Message = message;

                if (log.Message.Length > 3499)
                {
                    log.Message = log.Message.Substring(0, 3499);
                }

                // Add the log
                entityFrameworkDataAccessFactory.LogDAO.Add(log);
            }
            catch { }
        }

        public static void LogInfo(string method, string info, string storeId = null)
        {
            try
            {
                // Get an object that can talk to the database
                AA.EntityFrameworkDataAccessFactory entityFrameworkDataAccessFactory = new AA.EntityFrameworkDataAccessFactory();

                // Create the log
                AndroAdminDataAccess.Domain.Log log = new AndroAdminDataAccess.Domain.Log();
                log.Source = "AndroAdmin";
                log.Method = method;
                log.Severity = "INFO";
                log.StoreId = storeId;
                log.Created = DateTime.UtcNow;
                log.Message = info;

                if (log.Message.Length > 3499)
                {
                    log.Message = log.Message.Substring(0, 3499);
                }

                // Add the log
                entityFrameworkDataAccessFactory.LogDAO.Add(log);
            }
            catch { }
        }
    }
}