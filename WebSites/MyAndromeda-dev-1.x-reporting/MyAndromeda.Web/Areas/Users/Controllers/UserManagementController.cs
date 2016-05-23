using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using MyAndromeda.Data.DataAccess.Chains;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Areas.Users.ViewModels;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Web.Areas.Users.Services;
using System.Threading.Tasks;
using MyAndromeda.Web.Models.Emails;
using MyAndromeda.Core.Authorization;
using Kendo.Mvc.UI;
using MyAndromeda.Core.User;
using System.Collections.Generic;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Web.Areas.Users.Controllers
{
    public class UserManagementController : Controller
    {
        /* Utility services */
        private readonly IAuthorizer authorizer;
        private readonly INotifier notifier;
        private readonly ITranslator translator;

        /* Context variables */
        private readonly ICurrentUser currentUser;
        private readonly ICurrentChain currentChain;
        
        private readonly IUserManagementService userManagementService;
        private readonly IUserChainsDataService userChainsDataService;
        private readonly IChainDataService chainDataService;
        private readonly IUserDataService userDataService;
        private readonly IUserRoleDataService userRoleDataService;
        private readonly IUserSitesDataService userSiteDataService;

        public UserManagementController(
            IUserManagementService userManagementService, 
            ICurrentUser currentUser,
            ICurrentChain currentChain,
            IUserChainsDataService userChainsDataService, 
            IUserSitesDataService userSiteDataService, 
            IChainDataService chainDataService, 
            IUserDataService userDataService, 
            INotifier notifier, 
            IUserRoleDataService userRoleDataService, IAuthorizer authorizer, ITranslator translator)
        {
            this.translator = translator;
            this.userManagementService = userManagementService;
            this.authorizer = authorizer;
            this.userRoleDataService = userRoleDataService;
            this.notifier = notifier;
            this.userDataService = userDataService;
            this.chainDataService = chainDataService;
            this.currentChain = currentChain;
            this.currentUser = currentUser;
            this.userChainsDataService = userChainsDataService;
            this.userSiteDataService = userSiteDataService;
        }

        public ActionResult Index()
        {
            if (!this.authorizer.Authorize(UserManagementUserPermissions.ListUsers)) 
            {
                this.notifier.Error(translator.T("You do not have permissions to view users"));
                return new HttpUnauthorizedResult();
            }

            var viewModel = new UserListViewModel();

            viewModel.Authorizer = this.authorizer;
            viewModel.CurrentChain = currentChain;
            viewModel.CurrentUser = currentUser;
            viewModel.Users = //select the chain's users list if chain view is open 
                this.currentChain.Available
                ? this.userChainsDataService.FindUsersDirectlyBelongingToChain(currentChain.Chain.Id) 
                : this.userDataService.QueryForUsers(e=> true);

            return View(viewModel);
        }

        public ActionResult Create()
        {
            if (!this.authorizer.Authorize(UserManagementUserPermissions.ListUsers))
            {
                this.notifier.Error(translator.T(text: "You do not have permission to create users"));
                return new HttpUnauthorizedResult();
            }

            var viewModel = new CreateEditUserViewModel();

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<ActionResult> CreatePost(CreateEditUserViewModel viewModel)
        {
            if (!this.authorizer.Authorize(UserManagementUserPermissions.ListUsers))
            {
                this.notifier.Error(translator.T(text: "You do not have permission to create users"));
                return new HttpUnauthorizedResult();
            }

            if ((viewModel.SelectedChainIds == null || viewModel.SelectedChainIds.Count == 0) && (viewModel.SelectedStoreIds == null || viewModel.SelectedStoreIds.Count == 0))
            {
                this.notifier.Error(translator.T(text: "A chain or store is needed for the user"));
                return this.View(viewModel);
            }

            var model = new MyAndromedaUser()
            {
                Firstname = viewModel.FirstName,
                Surname = viewModel.LastName,
                Username = viewModel.UserName
            };

            this.userManagementService.CheckOverUser(model, this.ModelState, false);

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            if (viewModel.NotifyUser.GetValueOrDefault())
            {
                var emailModel = new NewUserEmail() 
                {
                    Chain = this.currentChain.Chain,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    UserName = viewModel.UserName
                };
                
                await emailModel.SendAsync();
            }

            this.userManagementService.CreateUser(model, viewModel.Password);
            if (model.Id > 0)
            {
                if (viewModel.SelectedChainIds != null && viewModel.SelectedChainIds.Count > 0)
                {
                    foreach (int chainId in viewModel.SelectedChainIds)
                    {
                        var chain = chainDataService.Get(chainId);
                        this.userChainsDataService.AddChainLinkToUser(chain, model.Id);
                    }
                }
                if (viewModel.SelectedStoreIds != null && viewModel.SelectedStoreIds.Count > 0)
                {
                    foreach (int storeId in viewModel.SelectedStoreIds)
                    {
                        this.userSiteDataService.AddStoreLinkToUser(storeId, model.Id);
                    }
                }
            }

            return this.RedirectToAction(actionName: "EditRoles", routeValues: new { UserId = model.Id });
        }



        public ActionResult EditUser(int userId)
        {
            if (!this.authorizer.Authorize(UserManagementUserPermissions.ListUsers))
            {
                this.notifier.Error(translator.T(text: "You do not have permission to create users"));
                return new HttpUnauthorizedResult();
            }

            var viewModel = new EditUserViewModel();
            
            UserRecord user = userDataService.GetByUserId(userId);
            
            viewModel.FirstName = user.FirstName;
            viewModel.LastName = user.LastName;
            viewModel.UserName = user.Username;
            viewModel.SelectedChainIds = userChainsDataService.GetChainsForUser(userId).Select(e => e.Id).ToList<int>();
            viewModel.SelectedStoreIds = userSiteDataService.GetSitesDirectlyLinkedToTheUser(userId).Select(e => e.Id).ToList<int>();
            
            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("EditUser")]
        public async Task<ActionResult> EditUserPost(int userId, EditUserViewModel viewModel)
        {
            if (!this.authorizer.Authorize(UserManagementUserPermissions.ListUsers))
            {
                this.notifier.Error(translator.T(text: "You do not have permission to edit users"));
                return new HttpUnauthorizedResult();
            }

            if ((viewModel.SelectedChainIds == null || viewModel.SelectedChainIds.Count == 0) && (viewModel.SelectedStoreIds == null || viewModel.SelectedStoreIds.Count == 0))
            {
                this.notifier.Error(translator.T(text: "A chain or store is needed for the user"));
                return this.View(viewModel);
            }

            var model = new MyAndromedaUser()
            {
                Id = userId,
                Firstname = viewModel.FirstName,
                Surname = viewModel.LastName,
                Username = viewModel.UserName,
            };

            this.userManagementService.CheckOverUser(model, this.ModelState, true);

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }
            this.userManagementService.UpdateUser(model);

            if (model.Id > 0)
            {
                List<int> chainsInDB = this.userChainsDataService.GetChainsForUser(model.Id).Select(i => i.Id).ToList<int>();
                List<int> sitesInDB = this.userSiteDataService.GetSitesDirectlyLinkedToTheUser(model.Id).Select(i => i.Id).ToList<int>();

                if (viewModel.SelectedChainIds == null && chainsInDB.Count > 0)
                    this.RemoveChains(chainsInDB, model.Id);
                if (viewModel.SelectedStoreIds == null && sitesInDB.Count > 0)
                    this.RemoveSites(sitesInDB, model.Id);
                   
                if (viewModel.SelectedChainIds != null)
                {
                    List<int> addedChains = viewModel.SelectedChainIds.Except(chainsInDB).ToList<int>();
                    List<int> removedChains = chainsInDB.Except(viewModel.SelectedChainIds).ToList<int>();

                    if (addedChains.Count > 0)
                        this.AddChains(addedChains, model.Id);
                    if (removedChains.Count > 0)
                        this.RemoveChains(removedChains, model.Id);
                }

                if (viewModel.SelectedStoreIds != null)
                {
                    List<int> addedSites = viewModel.SelectedStoreIds.Except(sitesInDB).ToList<int>();
                    List<int> removedSites = sitesInDB.Except(viewModel.SelectedStoreIds).ToList<int>();

                    if (addedSites.Count > 0)
                        this.AddSites(addedSites, model.Id);
                    if (removedSites.Count > 0)
                        this.RemoveSites(removedSites, model.Id);
                    
                }
            }

            return this.RedirectToAction(actionName: "Index");
        }

        private void RemoveSites(List<int> removedSites, int userId)
        {
            foreach (int storeId in removedSites)
            {
                this.userSiteDataService.RemoveStoreLinkToUser(userId, storeId);
            }
        }

        private void AddSites(List<int> addedSites, int userId)
        {
            foreach (int storeId in addedSites)
            {
                this.userSiteDataService.AddStoreLinkToUser(storeId, userId);
            }
        }

        private void AddChains(List<int> addedChains, int userId)
        {
            foreach (int chainId in addedChains)
            {
                ChainDomainModel chain = chainDataService.Get(chainId);
                this.userChainsDataService.AddChainLinkToUser(chain, userId);
            }
        }

        private void RemoveChains(List<int> removedChains, int userId)
        {
            foreach (int chainId in removedChains)
            {
                ChainDomainModel chain = chainDataService.Get(chainId);
                this.userChainsDataService.RemoveChainLinkToUser(userId, chain.Id);
            }
        }

        public ActionResult AddUserToChain(int chainId, int userId)
        {
            if (!this.authorizer.Authorize(UserManagementUserPermissions.CreateUsers))
            {
                this.notifier.Error(translator.T("You do not have permissions to create users"));
                return new HttpUnauthorizedResult();
            }

            //cant add a user to a chain that the current user doesn't belong to first.
            if (!currentUser.FlattenedChains.Any(e => e.Id == chainId)) 
            {
                this.notifier.Error(translator.T(text: "You do not have permission to add users to this chain"));
                return new HttpUnauthorizedResult();
            }

            UserRecord user = userDataService.Query(e => e.Id == userId).Single();
            ChainDomainModel chain = chainDataService.Get(chainId);

            this.userChainsDataService.AddChainLinkToUser(chain, userId);

            this.notifier.Notify(string.Format(translator.T(text: "{0} has been added to {1}"), user.Username, chain.Name));

            return RedirectToAction(actionName: "Index");
        }

        public ActionResult EditRoles(int userId)
        {
            MyAndromedaUser user = userDataService.QueryForUsers(e => e.Id == userId).Single();
            IEnumerable<IUserRole> userRoles = userRoleDataService.ListRolesForUser(user.Id);
            IEnumerable<IUserRole> allRoles = userRoleDataService.List();
            IEnumerable<IUserRole> availableRoles = allRoles;
            
            //var availableRoles = allRoles.Where(e => e.Name != ExpectedRoles.Administrator || e.Name != ExpectedRoles.SuperAdministrator);

            //remove roles that the user is not allowed to apply. 
            if(!authorizer.Authorize( UserManagementUserPermissions.AssignAdministratorRole))
            {
                availableRoles = availableRoles.Where(e => e.Name != ExpectedUserRoles.Administrator);
            }

            if (!authorizer.Authorize(UserManagementUserPermissions.AssignSuperAdministratorRole)) 
            {
                availableRoles = availableRoles.Where(e => e.Name != ExpectedUserRoles.SuperAdministrator);
            }

            if (!authorizer.Authorize(UserManagementUserPermissions.AssignChainAdministratorRole)) 
            {
                availableRoles = availableRoles.Where(e => e.Name != ExpectedUserRoles.ChainAdministrator);
            }

            if (!authorizer.Authorize(UserManagementUserPermissions.AssignStoreAdministratorRole)) 
            {
                availableRoles = availableRoles.Where(e => e.Name != ExpectedUserRoles.StoreAdministrator);
            }

            var viewModel = new UserRoleViewModel()
            {
                AvailableRoles = availableRoles.ToArray(), 
                //cannot add permissions higher than self ? - what about more modular roles 
                User = user,
                SelectedRoles = userRoles.Select(e => e.Id).ToArray()
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("EditRoles")]
        public ActionResult EditRolesPost(int userId, ViewModels.UserRoleViewModel viewModel)
        {
            MyAndromedaUser user = userDataService.QueryForUsers(e => e.Id == userId).Single();

            IEnumerable<IUserRole> allRoles = userRoleDataService.List();
            IEnumerable<IUserRole> availableRoles = allRoles;
            //var availableRoles = allRoles.Where(e => e.Name != ExpectedRoles.Administrator || e.Name != ExpectedRoles.SuperAdministrator);

            if (!authorizer.Authorize(UserManagementUserPermissions.AssignAdministratorRole))
            {
                availableRoles = availableRoles.Where(e => e.Name != ExpectedUserRoles.Administrator);
            }

            if (!authorizer.Authorize(UserManagementUserPermissions.AssignSuperAdministratorRole))
            {
                availableRoles = availableRoles.Where(e => e.Name != ExpectedUserRoles.SuperAdministrator);
            }

            if (!this.ModelState.IsValid)
            {
                viewModel.User = user;
                viewModel.AvailableRoles = availableRoles.ToArray();

                return this.View(viewModel);
            }

            IEnumerable<IUserRole> selectedRoles = viewModel.SelectedRoles == null 
                ? Enumerable.Empty<IUserRole>() 
                : availableRoles.Where(e=> viewModel.SelectedRoles.Contains(e.Id));

            this.userRoleDataService.AddRolesToUser(userId, selectedRoles);

            notifier.Notify(string.Format(translator.T(text: "The roles have been added to {0}"), user.Username));

            return this.RedirectToAction(actionName: "Index");
        }

        public ActionResult ChangePassword(int userId) 
        {
            if (!authorizer.Authorize(UserManagementUserPermissions.ResetPassword)) 
            {
                this.notifier.Error(translator.T(text: "You do not have permissions to reset password"));
                return new HttpUnauthorizedResult();
            }
            
            MyAndromedaUser user = userDataService.QueryForUsers(e => e.Id == userId).Single();

            return View(user);
        }

        [ActionName("ChangePassword"), ValidateAntiForgeryToken]
        public ActionResult ChangePasswordPost(int userId, string password) 
        {
            if (!authorizer.Authorize(UserManagementUserPermissions.ResetPassword))
            {
                this.notifier.Error(translator.T(text: "You do not have permissions to reset this users password"));
                return new HttpUnauthorizedResult();
            }

            this.userManagementService.ResetPassword(userId, password);

            return RedirectToAction(actionName: "Index");
        }

        [HttpPost]
        public async Task<ActionResult> AddUserToChain(string userName, bool? notifyUser)
        {
            if (!this.authorizer.Authorize(UserManagementUserPermissions.CreateUsers))
            {
                this.notifier.Error(translator.T(text: "You do not have permissions to create add users"));
                return new HttpUnauthorizedResult();
            }

            MyAndromedaUser user = this.userDataService.GetByUserName(userName);
            if (user == null)
            {
                this.ModelState.AddModelError(key: "userName", errorMessage: translator.T(text: "No user exists with that user name"));
                this.notifier.Notify(translator.T(text: "No user exists with the username: ")+ userName);

                return RedirectToAction(actionName: "Create");
            }

            this.userChainsDataService.AddChainLinkToUser(this.currentChain.Chain, user.Id);
            this.notifier.Notify(string.Format(translator.T(text: "The user '{0}' has been added to the chain"), userName));

            var emailModel = new NewUserEmail() 
            {
                UserName = userName,
                FirstName = user.Firstname,
                LastName = user.Surname,
                Chain = this.currentChain.Chain
            };

            if (notifyUser.HasValue) { 
                await emailModel.SendAsync();
            }

            return RedirectToAction(actionName: "Index");
        }

        public JsonResult Connections([DataSourceRequest]DataSourceRequest request, int? userId)
        {
            IEnumerable<ChainDomainModel> userList = userChainsDataService.FindChainsDirectlyBelongingToUser(userId.Value);
            IEnumerable<UserChainViewModel> viewModels = userList.Select(e => new UserChainViewModel {
                ChainId = e.Id,
                Name = e.Name,
                UserId = userId.GetValueOrDefault(0)
            });

            return Json(viewModels.ToDataSourceResult(request, this.ModelState));
        }

        public JsonResult StoreConnections([DataSourceRequest]DataSourceRequest request, int? userId)
        {
            IEnumerable<SiteDomainModel> userList = userSiteDataService.GetSitesDirectlyLinkedToTheUser(userId.Value);
            IEnumerable<UserStoreViewModel> viewModels = userList.Select(e => new UserStoreViewModel
            {
                StoreId = e.Id,
                Name = e.ClientSiteName,
                AndromedaSiteId = e.AndromediaSiteId,
                UserId = userId.GetValueOrDefault(0)
            });

            return Json(viewModels.ToDataSourceResult(request, this.ModelState));
        }

        public JsonResult Roles([DataSourceRequest]DataSourceRequest request, int userId)
        {
            IEnumerable<IUserRole> roles = this.userRoleDataService.ListRolesForUser(userId);

            return Json(roles.ToDataSourceResult(request, this.ModelState, e => new RoleViewModel { Name = e.Name }));
        }

        public ActionResult RemoveUser(int userId)
        {
            if (!authorizer.Authorize(UserManagementUserPermissions.RemoveUserFromChain))
            {
                this.notifier.Error(translator.T(text: "You do not have permissions to remove this user"));
                return new HttpUnauthorizedResult();
            }

            this.userChainsDataService.RemoveChainLinkToUser(userId, this.currentChain.Chain.Id);

            this.notifier.Notify(this.translator.T(text: "The user has been removed"));

            return RedirectToAction(actionName: "Index");
        }

        public ActionResult RemoveUserComposite(string id) 
        {
            if (!authorizer.Authorize(UserManagementUserPermissions.RemoveUserFromChain))
            {
                this.notifier.Error(translator.T(text: "You do not have permissions to remove this user"));
                return new HttpUnauthorizedResult();
            }

            string[] parts = id.Split('|');
            int chainId = Convert.ToInt32(parts[0]);
            int userId = Convert.ToInt32(parts[1]);

            this.userChainsDataService.RemoveChainLinkToUser(userId, chainId);
            this.notifier.Notify(this.translator.T(text: "The user has been removed"));

            return new HttpStatusCodeResult(statusCode: 200);
        }

        public ActionResult RemoveUserStoreComposite(string id)
        {
            if (!authorizer.Authorize(UserManagementUserPermissions.RemoveUserFromStore))
            {
                this.notifier.Error(translator.T(text: "You do not have permissions to remove this user"));
                return new HttpUnauthorizedResult();
            }

            string[] parts = id.Split('|');
            int storeId = Convert.ToInt32(parts[0]);
            int userId = Convert.ToInt32(parts[1]);

            this.userSiteDataService.RemoveStoreLinkToUser(userId, storeId);
            this.notifier.Notify(this.translator.T(text: "The user has been removed"));

            return new HttpStatusCodeResult(statusCode: 200);
        }

        [ActionName("Enable")]
        public ActionResult EnableUser(int userId) 
        {
            if (!authorizer.Authorize(UserManagementUserPermissions.RemoveUserFromChain))
            {
                this.notifier.Error(translator.T(text: "You do not have permissions to enable this user"));
                return new HttpUnauthorizedResult();
            }

            UserRecord user = this.userDataService.GetByUserId(userId);

            user.IsEnabled = true;

            this.userDataService.Update(user);

            return RedirectToAction(actionName: "Index");
        }

        [ActionName("Disable")]
        public ActionResult DisableUser(int userId) 
        {
            if (!authorizer.Authorize(UserManagementUserPermissions.RemoveUserFromChain))
            {
                this.notifier.Error(translator.T(text: "You do not have permissions to enable this user"));
                return new HttpUnauthorizedResult();
            }

            UserRecord user = this.userDataService.GetByUserId(userId);

            user.IsEnabled = false;

            this.userDataService.Update(user);

            return RedirectToAction(actionName: "Index");
        }
    }
}