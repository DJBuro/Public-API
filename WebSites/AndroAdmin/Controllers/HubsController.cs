using System;
using System.Linq;
using System.Web.Mvc;
using AndroAdmin.Helpers;
using AndroAdmin.Services;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.Controllers
{
    [Security(Permissions = Permissions.ReadHubs)]
    public class HubsController : BaseController
    {
        private readonly HubControllerService hubService = new HubControllerService();
 
        public ActionResult Index()
        {
            var data = hubService.GetHubs();

            return View(data);
        }

        [Security(Permissions = Permissions.EditHubs)]
        public ActionResult Create() 
        {
            var model = new HubItem();
            
            return View("Edit", model);
        }

        [ActionName("Create")]
        [HttpPost]
        [Security(Permissions = Permissions.EditHubs)]
        public ActionResult CreatePost(HubItem model) 
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            this.hubService.AddHub(model);

            return RedirectToAction("Index");
        }

        [Security(Permissions = Permissions.EditHubs)]
        public ActionResult Edit(Guid id) 
        {
            var hub = hubService.GetHub(id);

            return View("Edit", hub);
        }

        [ActionName("Edit")]
        [HttpPost]
        [Security(Permissions = Permissions.EditHubs)]
        public ActionResult EditPost(HubItem model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            this.hubService.UpdateHub(model);

            return RedirectToAction("Index");
        }

        public ActionResult Enable(Guid id) 
        {
            var model = this.hubService.GetHub(id);
            model.Active = true;

            this.hubService.UpdateHub(model);

            return this.RedirectToAction("Index");
        }

        public ActionResult Disable(Guid id) 
        {
            var model = this.hubService.GetHub(id);
            model.Active = false;

            this.hubService.UpdateHub(model);

            return this.RedirectToAction("Index");
        }
    }
}
