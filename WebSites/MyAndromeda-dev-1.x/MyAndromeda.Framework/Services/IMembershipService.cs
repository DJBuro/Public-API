using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Core;
using MyAndromeda.Core.User;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Framework.Services
{
    public interface IMembershipService : IDependency
    {
        /// <summary>
        /// Checks the user over.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelState">State of the model.</param>
        void ValidateUserModel(MyAndromedaUser model, ModelStateDictionary modelState, bool isUpdate);
        void ValidatePassword(string password, ModelStateDictionary modelState);

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="userNameOrEmail">The user name or email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        MyAndromedaUser ValidateUserLogin(string userNameOrEmail, string password);

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        MyAndromedaUser CreateUser(MyAndromedaUser user, string password);

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        MyAndromedaUser UpdateUser(MyAndromedaUser user);

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        MyAndromedaUser GetUser(string username);

        /// <summary>
        /// Gets the user record.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        UserRecord GetUserRecord(string username);

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="password">The password.</param>
        bool SetPassword(UserRecord entity, string password, bool saveChanges = true);
    }
}