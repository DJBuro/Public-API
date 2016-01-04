using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAndromeda.Framework
{
    public class ControllerContextHost
    {
        private ControllerBase controller;

        public void SetController(ControllerBase controller)
        {
            //if (this.controller != null)
            //{
            //    throw new InvalidOperationException("Controller was already set!");
            //}
            //maybe child action
            if (this.controller != null) { return; }
            this.controller = controller;
        }


        public ControllerContext GetContext()
        {
            if (controller == null)
            {
                return null;
            }

            return controller.ControllerContext;
        }

    }
}
