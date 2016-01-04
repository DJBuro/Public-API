using System.Linq;
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
        private readonly ICoreUserRoles coreUserRoles;
        private readonly IUserRoleDataService userRoleDataService;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;
        private readonly ITranslator translator; 

        public UserRoleAuthorizationController(IUserRoleDataService userRoleDataService,
            INotifier notifier,
            IAuthorizer authorizer,
            ITranslator translator,
            ICoreUserRoles coreUserRoles) 
        {
            this.coreUserRoles = coreUserRoles;
            this.translator = translator;
            this.authorizer = authorizer;
            this.userRoleDataService = userRoleDataService;
            this.notifier = notifier;
        }

        public ActionResult Levels() 
        {
            if (!authorizer.Authorize(UserRolesUserPermissions.ViewUserRoleDefinitions))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));
                return new HttpUnauthorizedResult();
            }

            var data = userRoleDataService.List();
            var defaultList = this.coreUserRoles.GetRoles();

            bool anyAdded = false;
            foreach (var item in defaultList) 
            {
                if (data.Any(e => e.Name.Equals(item, System.StringComparison.InvariantCultureIgnoreCase))) { continue; }

                this.userRoleDataService.CreateORUpdate(new ViewModels.UserRoleViewModel() { Name = item });
                anyAdded = true;
            }

            if (anyAdded) { return RedirectToAction("Levels"); }

            return View(data);
        }

        public ActionResult Create() 
        {
            if (!authorizer.Authorize(UserRolesUserPermissions.EditUserRoleDefinitions))
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
            if (!authorizer.Authorize(UserRolesUserPermissions.EditUserRoleDefinitions))
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

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult Delete(string name) 
        {
            if (!authorizer.Authorize(UserRolesUserPermissions.EditUserRoleDefinitions))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));
                return new HttpUnauthorizedResult();
            }

            var role = this.userRoleDataService.Get(name);

            this.userRoleDataService.Delete(role);

            this.notifier.Notify(translator.T("The role has been deleted"));

            return RedirectToAction("Levels");
        }
    }
}