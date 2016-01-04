[assembly: WebActivator.PreApplicationStartMethod(typeof(MyAndromeda.Web.AppStart.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MyAndromeda.Web.AppStart.NinjectWebCommon), "Stop")]
namespace MyAndromeda.Web.AppStart
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using MyAndromeda.Framework.Authorization;
    using MyAndromeda.Framework.Conventions;
    using MyAndromedaDataAccessEntityFramework;
    using Ninject;
    using Ninject.Web.Common;
    using MyAndromedaDataAccess;
    using Ninject.Extensions.Conventions;
    using MyAndromeda.Framework;
    using System.Web.Routing;
    using System.Web.Mvc;
    using Ninject.Web.Mvc.Filter;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using MyAndromeda.Framework.Contexts;
    using MyAndromeda.Core;

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
            //var settings = new NinjectSettings() { };
            var kernel = new StandardKernel();
            
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            kernel.Bind(scanner =>
            {
                scanner.FromAssembliesMatching("*")
                    .SelectAllClasses()
                    .InheritedFrom<IDependency>().BindWith(new DependencyConventionBinder())
                    .Configure(e=> e.InRequestScope());
            });

            kernel.Bind(scanner => {
                scanner.FromAssembliesMatching("*")
                    .SelectAllClasses()
                    .InheritedFrom<ITransientDependency>().BindWith(new TransientDependencyConventionBinder())
                    .Configure(e => e.InTransientScope());
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
            //could already be done
            //kernel.Bind<IFilterProvider>().To<NinjectFilterProvider>();

            //kernel.Bind<log4net.ILog>().ToMethod(context => log4net.LogManager.GetLogger(context.Request.Target.Member.ReflectedType));
            kernel.Unbind<Ninject.Extensions.Logging.ILogger>();
            kernel.Bind<Ninject.Extensions.Logging.ILogger>().ToMethod(context =>
            {
                var typeForLogger = context.Request.Target != null
                                        ? context.Request.Target.Member.DeclaringType
                                        : context.Request.Service;
                return context.Kernel.Get<Ninject.Extensions.Logging.ILoggerFactory>().GetLogger(typeForLogger);
            }).InTransientScope();

            kernel.Bind<WorkContextWrapper>().ToSelf().InTransientScope();
            kernel.Bind<ControllerContextHost>().ToSelf().InRequestScope();
            
            kernel.Bind((scan) => 
                scan.FromThisAssembly()
                    .SelectAllClasses()
                    .InheritedFrom<IController>()
                    .BindToSelf()
                    .Configure(config => config.InTransientScope().OnActivation((item) => {
                        var host = kernel.Get<ControllerContextHost>();
                        host.SetController(item as ControllerBase);
                    })
                )
            );


            kernel.Bind<IDataAccessFactory>().To<EntityFrameworkDataAccessFactory>().InRequestScope();
            kernel.Bind<Lazy<ControllerContext>>().ToMethod((c) => new Lazy<ControllerContext>(() =>  kernel.Get<ControllerContextHost>().GetContext()));
            kernel.Bind<RequestContext>().ToMethod((c) => HttpContext.Current.Request.RequestContext).InRequestScope();
            kernel.Bind<HttpContextWrapper>().ToMethod((c) => HttpContext.Current == null ? null : new HttpContextWrapper(HttpContext.Current));

            //is already done apparently 
            //kernel.Bind<RouteCollection>().ToMethod((c) => RouteTable.Routes).InRequestScope();
            
            kernel.Bind<UrlHelper>().ToSelf().InRequestScope();
            kernel.Bind<ITempDataProvider>().To<SessionStateTempDataProvider>().InRequestScope();
            kernel.Bind<IFilterProvider>().To<NinjectFilterProvider>();
            kernel.Bind<Lazy<IWorkContext>>().ToMethod((c) => new Lazy<IWorkContext>(() => kernel.Get<IWorkContext>())).InTransientScope();
            //Filter that I want to run before most 
            kernel.BindFilter<MyAndromedaAuthorizeAttribute>(FilterScope.Global, 100);
        }

    }
}
