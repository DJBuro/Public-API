using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Data.Domain;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Web.Areas.Authorization.Controllers
{
    public class UserAuthorizationController : Controller
    {
        private readonly WorkContextWrapper workContextWrapper;

        public UserAuthorizationController(WorkContextWrapper workContextWrapper) 
        {
            this.workContextWrapper = workContextWrapper;
        }

        public ActionResult Index() 
        {
            return View();
        }

        [HttpPost]
        public JsonResult ReadChains(int? parentId) 
        {
            var currentUser = workContextWrapper.Current.CurrentUser;
            IEnumerable<ChainDomainModel> chains = null;
            
            if(parentId.HasValue) 
            {
                var currentNode = currentUser.AccessibleChains.Where(e=> e.Id == parentId.GetValueOrDefault()).SingleOrDefault();
                chains = currentNode.Items;
            } 
            else
            {
                chains = currentUser.AccessibleChains;   
            }

            return Json(chains.Select(e => new { e.Id, e.Name, HasChildren = e.Items.Count() > 0 }));
        }

        [HttpPost]
        public JsonResult ReadStores(int? parentId) 
        {
            return Json(new { });
        }
    }
}