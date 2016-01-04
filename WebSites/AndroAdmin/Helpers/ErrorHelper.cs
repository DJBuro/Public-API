﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdmin.DataAccess;
using AndroAdminDataAccess.DataAccess;

namespace AndroAdmin.Helpers
{
    public class ErrorHelper
    {
        public static void LogError(string method, Exception exception)
        {
            try
            {
                // Get an object that can talk to the database
                ILogDAO logDAO = AndroAdminDataAccessFactory.GetLogDAO();

                // Create the log
                AndroAdminDataAccess.Domain.Log log = new AndroAdminDataAccess.Domain.Log();
                log.Source = "AndroAdmin";
                log.Method = method;
                log.Severity = "ERROR";
                log.StoreId = "";
                log.Created = DateTime.UtcNow;

                if (exception != null)
                {
                    log.Message = exception.ToString();
                    if (log.Message.Length > 3499)
                    {
                        log.Message = log.Message.Substring(0, 3499);
                    }
                }

                // Add the log
                logDAO.Add(log);
            }
            catch { }
        }

        public static void LogInfo(string method, string info)
        {
            try
            {
                // Get an object that can talk to the database
                ILogDAO logDAO = AndroAdminDataAccessFactory.GetLogDAO();

                // Create the log
                AndroAdminDataAccess.Domain.Log log = new AndroAdminDataAccess.Domain.Log();
                log.Source = "AndroAdmin";
                log.Method = method;
                log.Severity = "INFO";
                log.StoreId = "";
                log.Created = DateTime.UtcNow;
                log.Message = info;

                if (log.Message.Length > 3499)
                {
                    log.Message = log.Message.Substring(0, 3499);
                }

                // Add the log
                logDAO.Add(log);
            }
            catch { }
        }
    }
}