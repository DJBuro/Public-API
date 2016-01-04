using System.Web.Mvc;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;

namespace MyAndromeda.Web.Areas.Authorization.Controllers
{
    public class UserRoleAuthorizationController : Controller
    {
        private readonly IUserRoleDataService userRoleDataService;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;
        private readonly ITranslator translator; 

        public UserRoleAuthorizationController(IUserRoleDataService userRoleDataService, 
            INotifier notifier, IAuthorizer authorizer, ITranslator translator) 
        {
            this.translator = translator;
            this.authorizer = authorizer;
            this.userRoleDataService = userRoleDataService;
            this.notifier = notifier;
        }

        public ActionResult Levels() 
        {
            if (!authorizer.Authorize(Permissions.ChangeRoles))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));
                return new HttpUnauthorizedResult();
            }


            var data = userRoleDataService.List();
            
            return View(data);
        }

        public ActionResult Create() 
        {
            if (!authorizer.Authorize(Permissions.ChangeRoles))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));
                return new HttpUnauthorizedResult();
            }

            var viewModel = new ViewModels.UserRoleViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreatePost(ViewModels.UserRoleViewModel viewModel)
        {
            if (!authorizer.Authorize(Permissions.ChangeRoles))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));
                return new HttpUnauthorizedResult();
            }

            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            this.userRoleDataService.CreateORUpdate(viewModel);
            this.notifier.Notify(string.Format("Create: {0}", viewModel.Name));

            return RedirectToAction("Levels");
        }
    }
}