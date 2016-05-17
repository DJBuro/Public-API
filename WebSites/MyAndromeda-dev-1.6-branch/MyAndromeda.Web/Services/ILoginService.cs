using MyAndromeda.Core;
using MyAndromeda.Framework.Mvc;
using System.Web.Mvc;

namespace MyAndromeda.Web.Services
{

    public interface ILoginService : IDependency
    {
        ActionResult LoggedIn(IRedirectController controller, string userName, string returnUrl);
    }

}