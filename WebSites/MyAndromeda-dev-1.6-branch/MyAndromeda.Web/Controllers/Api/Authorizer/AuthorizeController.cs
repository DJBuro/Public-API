using MyAndromeda.Web.Controllers.Api.Authorizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyAndromeda.Web.Controllers.Api
{
    [Authorize]
    public class AuthorizeController : ApiController 
    {
        [Route("authorize/store/permission")]
        public AuthorizedResultModel AuthorizePermission(AuthorizeStorePermissionModel model)
        {
            return null;
        }

        [Route("authorize/store/anyPermission")]
        public AuthorizedResultModel AuthorizeAnyPermission(AuthorizeStoreByAnyPermissionModel model)
        {
            return null; 
        }

        [Route("authorize/stores/permission")]
        public AuthorizeStoresResultModel AuthorizeStoresByPermission(AuthorizeStoresPermissionModel model)
        {
            return null;
        }

        [Route("authorize/stores/anyPermission")]
        public AuthorizeStoresResultModel AuthorizeStoresByAnyPermission(AuthorizeStoresByAnyPermissionModel model)
        {
            return null;
        }
    }
}
