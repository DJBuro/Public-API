using System.Web.Mvc;
using MyAndromeda.Core;
using MyAndromeda.Core.User;

namespace MyAndromeda.Web.Areas.Users.Services
{
    public interface IUserManagementService : IDependency
    {
        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="password">The password.</param>
        void CreateUser(MyAndromedaUser model, string password);

        /// <summary>
        /// Checks the user over.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelState">State of the model.</param>
        void CheckOverUser(MyAndromedaUser model, ModelStateDictionary modelState, bool isUpdate);

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="password">The password.</param>
        void ResetPassword(int userId, string password);

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="model"></param>
        void UpdateUser(MyAndromedaUser model);
    }


}