using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public interface IFtpDownloadDataService : IDependency 
    {
        /// <summary>
        /// Sets the download flag.
        /// </summary>
        /// <param name="siteMenuFtp">The site menu FTP.</param>
        /// <param name="value">The value.</param>
        void SetDownloadTaskStatus(SiteMenu siteMenu, TaskStatus status);

        /// <summary>
        /// Sets the last download date.
        /// </summary>
        /// <param name="siteMenu">The site menu.</param>
        /// <param name="dateUtc">The date UTC.</param>
        void SetLastDownloadDate(SiteMenu siteMenu, DateTime dateUtc);

        /// <summary>
        /// Found a the new publish date on the ftp.
        /// </summary>
        /// <param name="siteMenu">The site menu.</param>
        /// <param name="menuPublishDate">The menu publish date.</param>
        void FoundNewPublishDate(SiteMenu siteMenu, DateTime? menuPublishDate);
    }
}