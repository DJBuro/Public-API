using MyAndromeda.Data;
using MyAndromeda.Framework.Conventions;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Conventions;
using MyAndromeda.Core;
using MyAndromeda.Logging;
namespace MyAndromeda.WebJobs.EventMarketing.Startup
{
    public static class NinjectStartup
    {
        public static IKernel CreateKernel() 
        {
            var kernel = new StandardKernel();
            //some reason the scanners need to be placed separately 

            kernel.Bind<IMyAndromedaLogger>().To<Services.MiniLogger>();

            kernel.Bind(scanner =>
            {
                scanner.FromAssembliesMatching("MyAndromeda*")
                    .SelectAllClasses()
                    .InheritedFrom<IDependency>().BindWith(new DependencyConventionBinder())
                    //purposeful switch for transient in web-jobs
                    .Configure(e => e.InTransientScope());
            });

            kernel.Bind(scanner =>
            {
                scanner.FromAssembliesMatching("MyAndromeda*")
                    .SelectAllClasses()
                    .InheritedFrom<ITransientDependency>().BindWith(new TransientDependencyConventionBinder())
                    .Configure(e => e.InTransientScope());
            });

            kernel.Bind(scanner =>
            {
                scanner.FromAssembliesMatching("MyAndromeda*")
                    .SelectAllClasses()
                    .InheritedFrom<ISingletonDependency>().BindWith(new SingletonDependencyConventionBinder())
                    .Configure(e => e.InSingletonScope());
            });

            return kernel;
        }

    }

    public static class NinjectExtensions 
    {
        public static void AddModules(this IKernel kernel) 
        {
            kernel.Load<Modules.WebTaskModule>();
            //kernel.Load<LoggingModule>();

            kernel.Load<MyAndromedaDbModule>();
        }
    }
}
