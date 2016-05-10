using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.User;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Users
{
    public interface IUserDataService : IDependency
    {
        /// <summary>
        /// returns a new instance of a database model
        /// </summary>
        /// <returns></returns>
        UserRecord New();

        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Add(UserRecord user);

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Update(UserRecord user);

        /// <summary>
        /// Gets the name of the by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        MyAndromedaUser GetByUserName(string userName);

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserRecord GetByUserId(int userId);

        /// <summary>
        /// Queries for users.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<MyAndromedaUser> QueryForUsers(Expression<Func<UserRecord, bool>> query);

        /// <summary>
        /// Queries the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<UserRecord> Query(Expression<Func<UserRecord, bool>> query);
    }
}
