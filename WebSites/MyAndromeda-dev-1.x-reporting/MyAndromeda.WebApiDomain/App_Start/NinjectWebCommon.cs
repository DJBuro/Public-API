[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MyAndromeda.WebApiServices.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MyAndromeda.WebApiServices.App_Start.NinjectWebCommon), "Stop")]
namespace MyAndromeda.WebApiServices.App_Start
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using MyAndromeda.Core;
    using MyAndromeda.Framework;
    using MyAndromeda.Framework.Contexts;
    using MyAndromeda.Framework.Conventions;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using MyAndromedaDataAccess;
    using MyAndromedaDataAccessEntityFramework;

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

            kernel.Load<FrameworkModule>();
            kernel.Load<MyAndromeda.Data.DataWarehouse.DatawarehouseModule>();

            //some reason the scanners need to be placed separately 
            kernel.Bind(scanner =>
            {
                scanner.FromAssembliesMatching("MyAndromeda*")
                       .SelectAllClasses()
                       .InheritedFrom<IDependency>()
                       .BindWith(new DependencyConventionBinder())
                       .Configure(e => e.InRequestScope());
            });

            kernel.Bind(scanner =>
            {
                scanner.FromAssembliesMatching("MyAndromeda*")
                       .SelectAllClasses()
                       .InheritedFrom<ITransientDependency>()
                       .BindWith(new TransientDependencyConventionBinder())
                       .Configure(e => e.InTransientScope());
            });

            kernel.Bind(scanner =>
            {
                scanner.FromAssembliesMatching("MyAndromeda*")
                       .SelectAllClasses()
                       .InheritedFrom<ISingletonDependency>()
                       .BindWith(new SingletonDependencyConventionBinder())
                       .Configure(e => e.InSingletonScope());
            });

            RegisterServices(kernel);

            //GlobalHost.DependencyResolver = new MyAndromedaSignalRNinjectDependencyResolver(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //to work when the app needs a logger that isn't injected through the constructor
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

            kernel.Bind((scan) => scan.FromThisAssembly()
                                      .SelectAllClasses()
                                      .InheritedFrom<IController>()
                                      .BindToSelf()
                                      .Configure(config => config.InTransientScope().OnActivation((item) =>
                                      {
                                          var host = kernel.Get<ControllerContextHost>();
                                          host.SetController(item as ControllerBase);
                                      })));
            
            kernel.Bind<IDataAccessFactory>().To<EntityFrameworkDataAccessFactory>().InRequestScope();

            kernel.Bind<Lazy<ControllerContext>>()
                  .ToMethod((c) => new Lazy<ControllerContext>(() => kernel.Get<ControllerContextHost>().GetContext()))
                  .InRequestScope();
            kernel.Bind<RequestContext>()
                  .ToMethod((c) => HttpContext.Current.Request.RequestContext)
                  .InRequestScope();
            kernel.Bind<HttpContextWrapper>()
                  .ToMethod((c) => HttpContext.Current == null ? null : new HttpContextWrapper(HttpContext.Current))
                  .InTransientScope();

            //is already done apparently 
            //kernel.Bind<RouteCollection>().ToMethod((c) => RouteTable.Routes).InRequestScope();

            kernel.Bind<UrlHelper>().ToSelf().InRequestScope();
            kernel.Bind<ITempDataProvider>().To<SessionStateTempDataProvider>().InRequestScope();
            //kernel.Bind<IFilterProvider>().To<NinjectFilterProvider>().InRequestScope();
            kernel.Bind<Lazy<IWorkContext>>().ToMethod((c) => new Lazy<IWorkContext>(() => kernel.Get<IWorkContext>())).InTransientScope();

            kernel.Bind<RouteData>().ToMethod((c) =>
            {
                if (HttpContext.Current == null)
                    return null;

                var httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
                var routeCollection = //kernel.Get<RouteCollection[]>();
                kernel.GetAll<RouteCollection>();
                return routeCollection.FirstOrDefault().GetRouteData(httpContextWrapper);
            }).InTransientScope();

            //Filter that I want to run before most 
            //kernel.BindFilter<MyAndromedaAuthorizeAttribute>(FilterScope.Global, 100);
            //kernel.Bind<Postal.IEmailService>().To<Postal.EmailService>();
            //public static Func<IEmailService> CreateEmailService;
            //Postal.Email.CreateEmailService = () => { return System.Web.Mvc.DependencyResolver.Current.GetService<MyAndromeda.SendGridService.IMyAndromedaEmailService>(); };
        }
    }
}