using System.Collections.Generic;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccess.Domain;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Core.User;

namespace MyAndromeda.Web.Areas.Users.ViewModels
{
    public class UserListViewModel
    {
        public ICurrentUser CurrentUser { get; set; }

        public ICurrentChain CurrentChain { get; set; }

        public IAuthorizer Authorizer { get; set; }

        public IEnumerable<MyAndromedaUser> Users { get; set; }
    }
}