using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Ninject;
using System.Diagnostics;

namespace MyAndromeda.SignalRHubs
{
    public class MyAndromedaSignalRNinjectDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel kernal; 

        public MyAndromedaSignalRNinjectDependencyResolver(IKernel kernal)
        {
            this.kernal = kernal;
        }

        public override object GetService(Type serviceType)
        {
            var tryDependency = this.kernal.TryGet(serviceType);
            
            if (tryDependency == null)
            {
                try
                {
                    //Trace.WriteLine("Failed grabbing by try ... try base");
                    return base.GetService(serviceType);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Failed getting dependency: " + serviceType.Name);
                    throw;
                }
            }

            return tryDependency;
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            //return base.GetServices(serviceType);
            return this.kernal.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}