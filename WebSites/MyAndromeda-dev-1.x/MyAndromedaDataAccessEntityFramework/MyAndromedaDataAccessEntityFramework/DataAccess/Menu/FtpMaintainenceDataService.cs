using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public class FtpMaintainenceDataService : IFtpMaintainenceDataService 
    {
        public IEnumerable<SiteMenu> ResetUploadTasks(DateTime notFinishedBy)
        {
            IEnumerable<SiteMenu> results = Enumerable.Empty<SiteMenu>();

            using (var dbContext = new MyAndromedaDbContext())
            {
                var query = dbContext.QueryMenusWithTasks(e =>
                                                              e.SiteMenuFtpBackupUploadTask.TaskStarted &&
                                                              e.SiteMenuFtpBackupUploadTask.LastTriedUtc < notFinishedBy);

                results = query.ToArray();

                foreach (var menu in results)
                {
                    menu.SiteMenuFtpBackupUploadTask.TaskStarted = false;
                    menu.SiteMenuFtpBackupUploadTask.TryTask = true;
                }

                dbContext.SaveChanges();
            }

            return results;
        }

        public IEnumerable<SiteMenu> ResetDownloadTasks(DateTime notFinishedBy)
        {
            IEnumerable<SiteMenu> results = Enumerable.Empty<SiteMenu>();

            using (var dbContext = new MyAndromedaDbContext())
            {
                var query = dbContext.QueryMenusWithTasks(
                    e =>
                        e.SiteMenuFtpBackupDownloadTask.TaskStarted &&
                        e.SiteMenuFtpBackupDownloadTask.LastTriedUtc < notFinishedBy);
                
                results = query.ToArray();

                foreach (var menu in results)
                {
                    menu.SiteMenuFtpBackupDownloadTask.TaskStarted = false;
                    menu.SiteMenuFtpBackupDownloadTask.TryTask = true;
                }

                dbContext.SaveChanges();
            }

            return results;
        }
    }
}