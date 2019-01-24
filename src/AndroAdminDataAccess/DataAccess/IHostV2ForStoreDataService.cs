using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AndroAdminDataAccess.EntityFramework;

namespace AndroAdminDataAccess.DataAccess
{
    public interface IHostV2ForStoreDataService 
    {
        /// <summary>
        /// Lists the host connections.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<HostStoreConnection> ListHostConnections(Expression<Func<Store, bool>> query);

        /// <summary>
        /// Lists the by host id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IEnumerable<Store> ListByHostId(Guid id);

        /// <summary>
        /// Lists the connected hosts for site.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        IEnumerable<HostV2> ListConnectedHostsForSite(int storeId);

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <param name="selectServerListIds">The select server list ids.</param>
        void AddCompleteRange(int storeId, IEnumerable<Guid> selectServerListIds);

        /// <summary>
        /// Clears all.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        void ClearAll(int storeId);
    }

    public interface IHostV2ForApplicationDataService
    {
        /// <summary>
        /// Lists the by host id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        IEnumerable<ACSApplication> ListByHostId(Guid id);

        /// <summary>
        /// Lists the connected hosts for application.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        IEnumerable<HostV2> ListConnectedHostsForApplication(int applicationId);

        /// <summary>
        /// Lists the host connections.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<HostApplicationConnection> ListHostConnections(Expression<Func<ACSApplication, bool>> query); 

        /// <summary>
        /// Adds the complete range.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="selectServerListIds">The select server list ids.</param>
        void AddCompleteRange(int applicationId, IEnumerable<Guid> selectServerListIds);

        /// <summary>
        /// Clears all.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        void ClearAll(int applicationId);
    }
    
    public class HostApplicationConnection 
    {
        public Guid HostId { get; set; }
        public int ApplicationId { get; set; }
    }

    public class HostStoreConnection
    {
        public Guid HostId { get; set; }
        public int AndromedaSiteId { get; set; }
    }
}