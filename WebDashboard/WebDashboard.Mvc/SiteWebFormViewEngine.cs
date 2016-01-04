using System.Web.Mvc;

namespace WebDashboard.Mvc
{
    public class SiteWebFormViewEngine : WebFormViewEngine
    {
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new SiteWebFormView(controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new SiteWebFormView(controllerContext, partialPath, null);
        }
    }
}
