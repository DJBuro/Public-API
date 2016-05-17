using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAndromeda.Framework.Mvc
{
    public interface IRedirectController 
    {
        RedirectToRouteResult RedirectToAction(string p, string p1, object p2);

        RedirectResult Redirect(string returnUrl);
    }
}
