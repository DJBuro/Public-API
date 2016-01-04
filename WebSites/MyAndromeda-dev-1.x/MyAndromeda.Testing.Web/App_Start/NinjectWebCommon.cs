[assembly: WebActivator.PreApplicationStartMethod(typeof(MyAndromeda.Testing.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MyAndromeda.Testing.Web.App_Start.NinjectWebCommon), "Stop")]

namespace MyAndromeda.Testing.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using MyAndromeda.Framework.Conventions;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using Ninject.Web.Mvc.Filter;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using MyAndromeda.Core;
    using MyAndromeda.Framework;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Load<FrameworkModule>();

            kernel.Bind(scanner =>
            {
                scanner.FromAssembliesMatching("MyAndromeda*")
                    .SelectAllClasses()
                    .InheritedFrom<IDependency>().BindWith(new DependencyConventionBinder())
                    .Configure(e => e.InRequestScope());
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

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}
