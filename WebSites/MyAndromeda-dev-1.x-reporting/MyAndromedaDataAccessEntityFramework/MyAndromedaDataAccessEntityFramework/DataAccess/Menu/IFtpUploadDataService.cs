using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public interface IFtpUploadDataService : IDependency 
    {
        /// <summary>
        /// Sets the upload flag.
        /// </summary>
        /// <param name="siteMenuFtp">The site menu FTP.</param>
        /// <param name="value">The value.</param>
        void SetUploadTaskStatus(SiteMenu siteMenu, TaskStatus status);
    }
}