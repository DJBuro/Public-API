using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Users
{
    public interface IUserSitesDataService : IDependency 
    {
        /// <summary>
        /// Gets the sites for user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<Site> GetSitesDirectlyLinkedToTheUser(int userId);

        IEnumerable<Site> GetSitesDirectlyLinkedToTheUser(int userId, Expression<Func<Store, bool>> query);

        /// <summary>
        /// Gets the sites for user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        IEnumerable<Site> GetSitesForUserAndChain(int userId, int chainId);

        /// <summary>
        /// Gets the sites for user and chain.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="chainId">The chain id.</param>
        /// <param name="deepSearch">The deep search.</param>
        /// <returns></returns>
        //IEnumerable<Site> GetSitesForUserAndChain(int userId, int chainId, bool deepSearch);
        void RemoveStoreLinkToUser(int userId, int storeId);

        /// <summary>
        /// Adds store to user
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="userId"></param>
        void AddStoreLinkToUser(int storeId, int userId);
    }
}