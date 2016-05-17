using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public interface IFtpMaintainenceDataService : IDependency 
    {
        /// <summary>
        /// Resets the upload tasks.
        /// </summary>
        /// <param name="notFinishedBy">The not finished by.</param>
        IEnumerable<SiteMenu> ResetUploadTasks(DateTime notFinishedBy);

        /// <summary>
        /// Resets the download tasks.
        /// </summary>
        /// <param name="notUploadedBy">The not uploaded by.</param>
        IEnumerable<SiteMenu> ResetDownloadTasks(DateTime notUploadedBy);
    }
}