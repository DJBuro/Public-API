using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdminDataAccess.DataAccess;
using System.Reflection;
using AndroAdmin.Helpers;

namespace AndroAdmin.DataAccess
{
    public class AndroAdminControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            IController controller = null;

            if (controllerType != null)
            {
                // Instantiate a controler object and pass it the DAO object in the constructor
                controller = (IController)Activator.CreateInstance(controllerType, null);

                BaseController baseController = (BaseController)controller;
            }

            return controller;
        }
    }
}