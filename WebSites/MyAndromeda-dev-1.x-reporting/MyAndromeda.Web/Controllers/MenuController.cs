using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.Authorization.ViewModels;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Web.Controllers
{

    [ChildActionOnly]
    public class MenuController : Controller
    {
        private readonly IWorkContext siteContext;
        private readonly IAuthorizer authorizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuController" /> class.
        /// </summary>
        /// <param name="siteContext">The site context.</param>
        /// <param name="authorizer">The authorizer.</param>
        public MenuController(IWorkContext siteContext, IAuthorizer authorizer) 
        {
            this.siteContext = siteContext;
            this.authorizer = authorizer;
        }

        [ActionName("MainMenu")]
        public PartialViewResult MainMenu() 
        {
            var viewModel = new NavigationViewModel() {
                Authorizer = authorizer,
                WorkContext = siteContext
            };

            return PartialView(viewModel);
        }
    }
}