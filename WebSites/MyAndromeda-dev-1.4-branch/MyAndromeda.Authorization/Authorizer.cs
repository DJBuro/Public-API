using System.Linq;
using MyAndromeda.Authorization.Services;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;

namespace MyAndromeda.Authorization
{
    public class Authorizer : IAuthorizer
    {
        private readonly IMyAndromedaLogger logging;
        private readonly IWorkContext workContext;
        private readonly ICurrentPermissionService currentPermissionSerivce;
        private PermissionGroup currentEffectivePermissions;

        public Authorizer(ICurrentPermissionService currentPermissionSerivce, IWorkContext workContext, IMyAndromedaLogger logging)
        {
            this.workContext = workContext;
            this.currentPermissionSerivce = currentPermissionSerivce;
            this.logging = logging;
        }

        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <value>The site.</value>
        public ICurrentSite Site
        {
            get
            {
                return this.workContext.CurrentSite;
            }
        }

        /// <summary>
        /// Gets the chain.
        /// </summary>
        /// <value>The chain.</value>
        public ICurrentChain Chain
        {
            get
            {
                return this.workContext.CurrentChain;
            }
        }

        /// <summary>
        /// Gets the current effective permissions once.
        /// </summary>
        /// <value>The current effective permissions.</value>
        private PermissionGroup CurrentEffectivePermissions 
        {
            get 
            {
                if (this.currentEffectivePermissions == null) 
                {
                    this.currentEffectivePermissions = this.currentPermissionSerivce.GetEffectivePermissions();
                }

                return this.currentEffectivePermissions;
            }
        }

        public ChainAndSiteAuthorization AuthorizedForChainAndStore()
        {
            var chainAvailable = this.workContext.CurrentChain.Available;
            var siteAvailable = this.workContext.CurrentChain.Available;

            var state = new ChainAndSiteAuthorization()
            {
                NotAccessingChain = !chainAvailable,
                NotAccessingSite = !siteAvailable
            };

            if (chainAvailable)
            {
                state.IsUserAllowedAtChainLevel = this.Chain.AuthorizedAtChainLevel;
            }

            if (siteAvailable)
            {
                state.IsUserAllowedToSiteWithinChain = this.AuthorizedForSiteAccess();
            }

            return state;
        }

        public bool Authorize(Permission permission)
        {
            var permissions = this.CurrentEffectivePermissions;
            bool valid = false;
            
            if (permission.PermissionType == PermissionType.StoreEnrolement) 
            {
                valid = permissions.StorePermissions.Any(e => e.Name == permission.Name && e.Category == permission.Category);
            }

            if (permission.PermissionType == PermissionType.UserRole) 
            {
                valid = permissions.UserPermissions.Any(e => e.Name == permission.Name && e.Category == permission.Category);
            }
            
            if (!valid) 
            {
                if (permission.PermissionType == PermissionType.StoreEnrolement) 
                {
                    if (this.workContext.CurrentSite.Available)
                    { 
                        this.logging.Debug("Store:{0} - {1} does not have permission for : {2}", 
                            this.workContext.CurrentSite.AndromediaSiteId,
                            this.workContext.CurrentSite.Site.ExternalName, 
                            permission.Name);
                    }
                }
                if (permission.PermissionType == PermissionType.UserRole) 
                {
                    if (this.workContext.CurrentUser.Available)
                    { 
                        this.logging.Debug("User: {0} does not have permission for : {1}", this.workContext.CurrentUser.User.Username, permission.Name);
                    }
                }
            }

            return valid;
        }

        public bool AuthorizeAny(params Permission[] anyPermission)
        {
            var permissions = this.CurrentEffectivePermissions;

            bool valid = false;

            if(anyPermission.Any(e=> e.PermissionType == PermissionType.StoreEnrolement))
            { 
                valid = anyPermission
                    .Where(e => e.PermissionType == PermissionType.StoreEnrolement)
                    .Any(exist => permissions.StorePermissions
                        .Any(e => e.Name == exist.Name && e.Category == exist.Category)
                    );
            }

            if (anyPermission.Any(e => e.PermissionType == PermissionType.UserRole)) 
            {
                valid = valid || anyPermission
                    .Where(e => e.PermissionType == PermissionType.UserRole)
                    .Any(exist => permissions.UserPermissions.Any(e => e.Name == exist.Name && e.Category == exist.Category));
            }

            if (!valid)
            {
                this.logging.Debug("User: {0} does not have permission for any: {1}",
                    this.workContext.CurrentUser.User.Username,
                    string.Join(", ", anyPermission.Select(e => e.Name)));
            }

            return valid;
        }

        public bool AuthorizeAll(params Permission[] requiedPermissions)
        {
            var permissions = this.CurrentEffectivePermissions;
            bool valid = true;
            //var valid = requiedPermissions.All(exist => permissions.Any(e => e.Name == exist.Name && e.Category == exist.Category));

            if(requiedPermissions.Any(e=> e.PermissionType == PermissionType.StoreEnrolement))
            {
                var requiredStorePermissions = requiedPermissions.Where(e => e.PermissionType == PermissionType.StoreEnrolement).ToArray();

                valid = valid && requiredStorePermissions.All(exist => permissions.StorePermissions.Any(e => e.Name == exist.Name && e.Category == exist.Category));
            }
            if (requiedPermissions.Any(e => e.PermissionType == PermissionType.UserRole)) 
            {
                var requiredUserPermission = requiedPermissions.Where(e => e.PermissionType == PermissionType.UserRole).ToArray();

                valid = valid && requiredUserPermission.All(exist => permissions.UserPermissions.Any(e => e.Name == exist.Name && e.Category == exist.Category));
            }

            if (!valid)
            {
                this.logging.Debug("User: {0} does not have permission for all of: {1}",
                    this.workContext.CurrentUser.User.Username,
                    string.Join(", ", requiedPermissions.Select(e => e.Name)));
            }

            return valid;
        }

        /// <summary>
        /// Authorized for site?
        /// </summary>
        /// <returns></returns>
        public bool AuthorizedForSiteAccess()
        {
            if (!this.workContext.CurrentSite.Available)
            {
                //nothing site wise to access 
                return true;
            }

            return this.workContext.CurrentSite.AuthorizedAtSiteLevel;
        }

        /// <summary>
        /// Authorized for chain?
        /// </summary>
        /// <returns></returns>
        public bool AuthorizedForChainAccess()
        {
            //check user is allowed 
            return this.workContext.CurrentChain.AuthorizedAtChainLevel;
        }
    }
}
