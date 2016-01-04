using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Ninject.Modules;

namespace Ninject.Extensions.MyAndromeda
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

    public class MyAndromedaSignalRNinjectDependencyResolver : DefaultDependencyResolver 
    {
        private readonly IKernel _kernel;

        public MyAndromedaSignalRNinjectDependencyResolver(IKernel kernel)
        {
            this._kernel = kernel;
        }
 
        public override object GetService(Type serviceType)
        {
            return this._kernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }
 
        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return this._kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}
