using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Menu
{
    public interface IMyAndromedaSiteMediaServerDataService : IDependency
    {
        /// <summary>
        /// Gets the default Media Server.
        /// </summary>
        /// <returns></returns>
        SiteMenuMediaServer GetDefault();

        /// <summary>
        /// Gets the media server, with default if there is no alternative attached.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        SiteMenuMediaServer GetMediaServerWithDefault(int andromedaSiteId);
    }
}