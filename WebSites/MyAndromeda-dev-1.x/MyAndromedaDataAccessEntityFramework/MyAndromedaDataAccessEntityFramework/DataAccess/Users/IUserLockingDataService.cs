using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Users
{
    public interface IUserLockingDataService : IDependency
    {
        /// <summary>
        /// Creates or update restrictions.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="ipRanges">The ip ranges.</param>
        void CreateOrUpdateRestrictionsByUserId(int id, string ipRanges);

        /// <summary>
        /// Gets the lock by user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        UserIpLock GetLockByUserName(string userName);

        /// <summary>
        /// Gets the lock by user id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        UserIpLock GetLockByUserId(int id);

        /// <summary>
        /// Lists the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<UserIpLock> List(Expression<Func<UserIpLock, bool>> query);
    }
}