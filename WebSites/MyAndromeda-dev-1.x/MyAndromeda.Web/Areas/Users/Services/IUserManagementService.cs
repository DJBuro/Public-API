using System.Web.Mvc;
using MyAndromeda.Core;
using System;
using System.Linq;
using MyAndromeda.Core.User;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;
using MyAndromedaMembershipProvider.Services;

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

    public class UserManagementService : IUserManagementService 
    {
        private readonly IMembershipService membershiptService;
        private readonly IUserDataService userDataService;

        public UserManagementService(IMembershipService membershiptService, IUserDataService userDataService)
        {
            this.userDataService = userDataService;
            this.membershiptService = membershiptService;
        }

        public void CreateUser(MyAndromedaUser model, string password)
        {
            this.membershiptService.CreateUser(model, password);
        }

        public void UpdateUser(MyAndromedaUser model)
        {
            this.membershiptService.UpdateUser(model);
        }

        public void CheckOverUser(MyAndromedaUser model, ModelStateDictionary modelState, bool isUpdate)
        {
            this.membershiptService.ValidateUserModel(model, modelState, isUpdate);
        }

        public void ResetPassword(int userId, string password)
        {
            var user = userDataService.Query(e => e.Id == userId).Single();

            this.membershiptService.SetPassword(user, password, true);
        }
    }


}