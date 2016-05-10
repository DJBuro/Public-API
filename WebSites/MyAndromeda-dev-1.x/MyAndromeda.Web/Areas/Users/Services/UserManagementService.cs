using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using MyAndromeda.Core;
using MyAndromeda.Core.User;
using MyAndromedaMembershipProvider.Services;
using MyAndromeda.Data.DataAccess.Users;

namespace MyAndromeda.Web.Areas.Users.Services
{

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