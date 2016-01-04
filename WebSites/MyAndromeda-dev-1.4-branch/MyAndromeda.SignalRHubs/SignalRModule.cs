using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Ninject;
using Ninject.Modules;

namespace MyAndromeda.SignalRHubs
{
    public class SignalRModule : NinjectModule
    {
        public override void Load()
        {
            //GlobalHost.DependencyResolver = new MyAndromedaNinjectDependencyResolver(this.Kernel);
            //new NinjectDependencyResolver(this.Kernel);
            //SignalR.Hosting.AspNet.AspNetHost.SetResolver(new SignalR.Ninject.NinjectDependencyResolver(kernel));
        }
    }
}
