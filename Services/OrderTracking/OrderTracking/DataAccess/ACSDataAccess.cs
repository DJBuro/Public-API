using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderTracking.DataAccess
{
    public class ACSDataAccess
    {
        internal static void LogEvent(string message, Exception exception, string level)
        {
            using (ACSEntities acsEntities = new ACSEntities())
            {
                ACSLog acsLog = new ACSLog()
                {
                    Date = DateTime.UtcNow,
                    Exception = exception.Message,
                    Level = level,
                    Logger = "OrderTracking",
                    Message = message
                };

                acsEntities.ACSLogs.Add(acsLog);

                acsEntities.SaveChanges();
            }
        }

        internal static List<int> GetApplicationStores(string applicationId)
        {
            List<int> acsApplicationStores = new List<int>();

            using (ACSEntities acsEntities = new ACSEntities())
            {
                var applicationStoresQuery = from applicationSite in acsEntities.ACSApplicationSites
                                             join application in acsEntities.ACSApplications
                                                  on applicationSite.ACSApplicationId equals application.Id
                                             join site in acsEntities.Sites
                                                on applicationSite.SiteId equals site.ID
                                             where application.ExternalApplicationId == applicationId
                                             select site;

                if (applicationStoresQuery != null)
                {
                    foreach (Site site in applicationStoresQuery)
                    {
                        acsApplicationStores.Add(site.AndroID);
                    }
                }
            }

            return acsApplicationStores;
        }
    }
}