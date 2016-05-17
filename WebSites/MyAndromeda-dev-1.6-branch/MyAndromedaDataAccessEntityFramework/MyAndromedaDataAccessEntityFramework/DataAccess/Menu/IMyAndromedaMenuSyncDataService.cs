using System;
using System.Linq;
using MyAndromeda.Core;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public interface IMyAndromedaMenuSyncDataService : IDependency 
    {
        /// <summary>
        /// Syncs the menu thumbnails.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="json">The json.</param>
        void SyncMenuThumbnails(int andromedaSiteId, string xml, string json);

        /// <summary>
        /// Syncs the actual menu.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="json">The json.</param>
        void SyncActualMenu(int andromedaSiteId, string xml, string json, int? version);

        /// <summary>
        /// Check if the 'compiled json or xml data is out of date.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        bool IsThereAnyPointSyncing(int andromedaSiteId);
    }
}