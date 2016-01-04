using System;
using System.Web;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromedaDB = MyAndromedaDataAccess.Domain;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;
using System.Collections.Generic;
using MyAndromedaDataAccessEntityFramework.DataAccess.Permissions;
using MyAndromeda.Core.User;

namespace MyAndromeda.Framework.Contexts
{
    public interface ICurrentUser : IDependency 
    {
        bool Available { get; }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>The user.</value>
        MyAndromedaUser User { get; }

        /// <summary>
        /// Gets the accessible chains to the user.
        /// </summary>
        /// <value>The accessible chains.</value>
        MyAndromeda.Data.Domain.Chain[] AccessibleChains { get; }

        /// <summary>
        /// flattened chain list based on the tree(s) the user has access to.
        /// </summary>
        /// <value>The flattened chains.</value>
        MyAndromeda.Data.Domain.Chain[] FlattenedChains { get; }

        /// <summary>
        /// Gets the accessible sites.
        /// </summary>
        /// <value>The accessible sites.</value>
        MyAndromeda.Data.Domain.Site[] AccessibleSites { get; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        IUserRole[] Roles { get; }

        //bool HasAndroWebOrderingSites { get; }
    }

    public class CurrentUser : ICurrentUser 
    {
        private readonly HttpContextWrapper httpContextWrapper;
        private readonly IUserDataService userDataService;
        private readonly IUserChainsDataService userChainDataService;
        private readonly IUserSitesDataService usersSiteService;
        private readonly IPermissionDataAccessService permissionDataAccess;

        public CurrentUser(HttpContextWrapper httpContextWrapper,
            IUserDataService userDataService,
            IUserChainsDataService userChainDataService,
            IUserSitesDataService usersSiteService,
            IPermissionDataAccessService permissionDataAccess)
        {
            this.permissionDataAccess = permissionDataAccess;
            this.usersSiteService = usersSiteService;
            this.userChainDataService = userChainDataService;
            this.userDataService = userDataService;
            this.httpContextWrapper = httpContextWrapper;
        }

        public bool Available { get { return this.User != null; } }

        MyAndromeda.Data.Domain.Chain[] _accessibleChains;

        public MyAndromeda.Data.Domain.Chain[] AccessibleChains
        {
            get
            {
                if (this.User == null)
                    return null;

                return _accessibleChains ?? (_accessibleChains = userChainDataService.GetChainsForUser(this.User.Id).ToArray());
            }
        }
        
        //bool _hasAndroWebOrderingSites;
        //public bool HasAndroWebOrderingSites
        //{
        //    get
        //    {
        //        return _hasAndroWebOrderingSites = (_hasAndroWebOrderingSites = userChainDataService.GetAndroWebOrderingSitesForUser(this.User.Id).ToArray().Count() > 0 ? true : false);
        //    }
        //}

        MyAndromeda.Data.Domain.Chain[] _flatternedChains;

        public MyAndromeda.Data.Domain.Chain[] FlattenedChains
        {
            get
            {
                if (this.User == null)
                    return null;

                return _flatternedChains ?? (_flatternedChains = CurrentUser.FlatternChains(this.AccessibleChains).ToArray());
            }
        }

        MyAndromeda.Data.Domain.Site[] _accessibleSites;

        public MyAndromeda.Data.Domain.Site[] AccessibleSites
        {
            get
            {
                if (this.User == null)
                    return null;

                return _accessibleSites ?? (_accessibleSites = this.GetAccessibleSites().ToArray());
            }
        }

        IUserRole[] _userRoles;
        public IUserRole[] Roles
        {
            get
            {
                if (this.User == null)
                    return null;

                return _userRoles ?? (_userRoles = this.GetUserRoles().ToArray());
            }
        }

        private IEnumerable<IUserRole> GetUserRoles()
        {
            IEnumerable<IUserRole> userRoles = this.permissionDataAccess.GetUserRolePermissions(this._user.Id);

            return userRoles;
        }

        private IEnumerable<MyAndromeda.Data.Domain.Site> GetAccessibleSites() 
        {
            var user = this.User;

            return usersSiteService.GetSitesDirectlyLinkedToTheUser(user.Id);
        }

        public static IEnumerable<MyAndromeda.Data.Domain.Chain> FlatternChains(IEnumerable<MyAndromeda.Data.Domain.Chain> accessibleChains)
        {
            Func<IEnumerable<MyAndromeda.Data.Domain.Chain>, IEnumerable<MyAndromeda.Data.Domain.Chain>> flatern = null;
            
            flatern = (nodes) => {
                return nodes.Union(nodes.SelectMany(e=> flatern(e.Items)));
            };

            var all = accessibleChains.Union(accessibleChains.SelectMany(e => flatern(e.Items))).ToArray();

            return all;
        }

        MyAndromedaUser _user;
        public MyAndromedaUser User
        {
            get
            {
                return _user ?? (_user = this.LoadUser());
            }
        }
  
        private MyAndromedaUser LoadUser()
        {
            if (httpContextWrapper == null) { return null; }
            if (httpContextWrapper.User == null) { return null; }
            if (httpContextWrapper.User.Identity == null) { return null; }
            if (!httpContextWrapper.User.Identity.IsAuthenticated) { return null; }

            var userName = httpContextWrapper.User.Identity.Name;
            var model = this.userDataService.GetByUserName(userName);
            
            return model;
        }
    }
}