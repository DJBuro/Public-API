using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AndroWebAdmin.Mvc
{
    public class SiteWebFormViewEngine : WebFormViewEngine
    {
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new SiteWebFormView(viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new SiteWebFormView(partialPath, null);
        }
    }
}
