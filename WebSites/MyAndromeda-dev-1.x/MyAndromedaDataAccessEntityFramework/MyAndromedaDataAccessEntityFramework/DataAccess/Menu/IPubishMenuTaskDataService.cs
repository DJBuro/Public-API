using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Logging;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public interface IPubishMenuTaskDataService : IDependency 
    {
        /// <summary>
        /// Sets the acs upload menu data task status.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="status">The status.</param>
        void SetAcsUploadMenuDataTaskStatus(SiteMenu menu, TaskStatus status, DateTime? date = null);

        /// <summary>
        /// Gets the publish tasks.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        IEnumerable<SiteMenu> GetPublishTasks(DateTime time);

        /// <summary>
        /// Adds the history log.
        /// </summary>
        /// <param name="sitemenu">The sitemenu.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="publishAll">The publish all.</param>
        /// <param name="publishMenu">The publish menu.</param>
        /// <param name="publishThumbnails">The publish thumbnails.</param>
        /// <param name="publishOnUtc">The publish on UTC.</param>
        void AddHistoryLog(SiteMenu sitemenu, string userName, bool publishAll, bool publishMenu, bool publishThumbnails, DateTime? publishOnUtc);
    }
}