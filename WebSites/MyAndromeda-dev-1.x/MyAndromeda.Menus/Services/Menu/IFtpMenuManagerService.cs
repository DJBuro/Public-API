using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Menus.Context.Ftp;

namespace MyAndromeda.Menus.Services.Menu
{
    public interface IFtpMenuManagerService : ITransientDependency, IDisposable
    {
        /// <summary>
        /// Downloads the menu.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        FtpMenuContext DownloadMenu(SiteMenu siteMenu);

        /// <summary>
        /// Uploads the menu.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        FtpMenuContext UploadMenu(int andromedaSiteId);

        /// <summary>
        /// Gets the menu.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        SiteMenu GetMenu(int andromedaSiteId);

        /// <summary>
        /// Adds the task to lookup the menu.
        /// </summary>
        /// <param name="siteMenu">The site menu.</param>
        /// <param name="throttleTime">The throttle time.</param>
        /// <returns></returns>
        bool AddTaskToDownloadTheMenu(SiteMenu siteMenu, TimeSpan throttleTime);

        /// <summary>
        /// Updates the task to completed.
        /// </summary>
        /// <param name="siteMenu">The site menu.</param>
        void UpdateDownloadTaskToCompleted(SiteMenu siteMenu);

        /// <summary>
        /// Updates the task to failed.
        /// </summary>
        /// <param name="siteMenu">The site menu.</param>
        void UpdateDownloadTaskToFailed(SiteMenu siteMenu);

        /// <summary>
        /// Asks to publish ... and update that property
        /// </summary>
        /// <param name="siteMenuFtp">The site menu.</param>
        void CreatePublishFtpTask(SiteMenu siteMenu, DateTime? dateTimeUtc);

        /// <summary>
        /// done the publish part and update the flag.
        /// </summary>
        /// <param name="siteMenuFtp">The site menu FTP.</param>
        //void PublishTask(int andromedaSiteId, SiteMenu siteMenu, bool inProgress);
        void UpdatePublishTask(SiteMenu siteMenu, TaskStatus status);

        /// <summary>
        /// Gets the queue to download.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SiteMenu> GetQueueToDownload();

        /// <summary>
        /// Gets the queue to upload.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SiteMenu> GetQueueToUpload();

        /// <summary>
        /// Sets the version.
        /// </summary>
        /// <param name="version">The version.</param>
        void SetVersion(SiteMenu siteMenu);
    }
}
