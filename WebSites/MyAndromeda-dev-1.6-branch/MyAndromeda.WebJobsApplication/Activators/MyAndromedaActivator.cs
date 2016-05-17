using System;
using System.Linq;
using Microsoft.Azure.WebJobs.Host;
using MyAndromeda.WebJobs.EventMarketing.Startup;
using Ninject;

namespace MyAndromeda.WebJobs.EventMarketing.Activators
{
    public class MyAndromedaActivator : IJobActivator
    {
        private readonly Ninject.IKernel kernel;

        public MyAndromedaActivator() 
        {
            this.kernel = Startup.NinjectStartup.CreateKernel();
            this.kernel.AddModules();
        }

        public T CreateInstance<T>()
        {
            //if (this.kernal.CanResolve<T>()) 
            try
            {
                return this.kernel.Get<T>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Could not create instance");
                System.Diagnostics.Trace.WriteLine(ex.Message);
                throw;
            }
        }
    }

}
