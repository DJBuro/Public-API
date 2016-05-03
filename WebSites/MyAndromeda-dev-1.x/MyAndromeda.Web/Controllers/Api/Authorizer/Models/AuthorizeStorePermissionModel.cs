using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Web.Controllers.Api.Authorizer.Models
{
    public class AuthorizeStorePermissionModel
    {
        public AuthorizeStoreRequestModel Store { get; set; }
        public PermissionRequestModel Permission { get; set; }
    }

    public class AuthorizeStoreByAnyPermissionModel
    {
        public AuthorizeStoreRequestModel Store { get; set; }
        public List<PermissionRequestModel> Permissions { get; set; }
    }

    public class AuthorizeStoresPermissionModel
    {
        public List<AuthorizeStoreRequestModel> Stores { get; set; }
        public PermissionRequestModel Permission { get; set; }
    }

    public class AuthorizeStoresByAnyPermissionModel
    {
        public List<AuthorizeStoreRequestModel> Stores { get; set; }
        public List<PermissionRequestModel> Permissions { get; set; }
    }

    public class AuthorizeStoreRequestModel
    {

    }

    public class PermissionRequestModel
    {
        public string Name { get; set; }
    }

    public class AuthorizedResultModel
    {
        public PermissionRequestModel Permission { get; set; }
    }

    public class AuthorizeStoresResultModel
    {

    }

}
