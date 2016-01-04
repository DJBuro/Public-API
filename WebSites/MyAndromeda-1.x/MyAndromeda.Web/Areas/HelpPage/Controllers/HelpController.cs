using System;
using System.Web.Http;
using System.Web.Mvc;
using MyAndromeda.Web.Areas.HelpPage.ModelDescriptions;
using MyAndromeda.Web.Areas.HelpPage.Models;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private readonly IAuthorizer authorizer;

        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HelpController(HttpConfiguration config, IAuthorizer authorizer) : this(config)
        { 
            this.authorizer = authorizer;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            if (!authorizer.Authorize(Permissions.ViewApiHelpSection))
            {
                return new HttpUnauthorizedResult();
            }


            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!authorizer.Authorize(Permissions.ViewApiHelpSection))
            {
                return new HttpUnauthorizedResult();
            }

            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!authorizer.Authorize(Permissions.ViewApiHelpSection))
            {
                return new HttpUnauthorizedResult();
            }

            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}