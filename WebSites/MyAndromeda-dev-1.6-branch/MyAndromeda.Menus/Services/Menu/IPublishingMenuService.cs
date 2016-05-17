using System;
using System.Collections.Generic;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Menus.Services.Menu
{
    public interface IPublishingMenuService : ITransientDependency
    {
        /// <summary>
        /// Creates the synchronization task.
        /// </summary>
        /// <param name="siteMenu">The site menu.</param>
        void CreateSynchronizationTask(SiteMenu siteMenu);

        /// <summary>
        /// Publishes the thumbnails only.
        /// </summary>
        /// <param name="siteMenu">The site menu.</param>
        void PublishThumbnailJsonAndXml(SiteMenu siteMenu);

        /// <summary>
        /// Publishes the menu later.
        /// </summary>
        /// <param name="sitemenu">The sitemenu.</param>
        /// <param name="dateUtc">The date UTC.</param>
        void PublishMenuLater(SiteMenu sitemenu, DateTime? dateUtc);

        /// <summary>
        /// Publishes the now.
        /// </summary>
        /// <param name="sitemenu">The sitemenu.</param>
        void PublishNow(SiteMenu sitemenu);

        /// <summary>
        /// Started the task.
        /// </summary>
        /// <param name="sitemenu">The sitemenu.</param>
        void StartedTask(SiteMenu sitemenu);
        
        /// <summary>
        /// Completed the task.
        /// </summary>
        /// <param name="sitemenu">The sitemenu.</param>
        void CompletedTask(SiteMenu sitemenu);

        /// <summary>
        /// Failed the task.
        /// </summary>
        /// <param name="sitemenu">The sitemenu.</param>
        void FailedTask(SiteMenu sitemenu);

        /// <summary>
        /// Gets the menus to publish.
        /// </summary>
        /// <param name="dateUtc">The date UTC.</param>
        /// <returns></returns>
        IEnumerable<SiteMenu> GetMenusToPublish(DateTime dateUtc);

        /// <summary>
        /// Adds the history log.
        /// </summary>
        /// <param name="sitemenu">The sitemenu.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="publishAll">The publish all.</param>
        /// <param name="publishMenu">The publish menu.</param>
        /// <param name="publishThumbnails">The publish thumbnails.</param>
        void AddHistoryLog(SiteMenu sitemenu, string userName, bool publishAll, bool publishMenu, bool publishThumbnails, DateTime? publishOn);
    }
}