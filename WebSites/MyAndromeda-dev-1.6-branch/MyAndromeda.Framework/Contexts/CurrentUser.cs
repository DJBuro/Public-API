using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Core.User;
using MyAndromeda.Data.Domain;
using MyAndromeda.Data.DataAccess.Permissions;

namespace MyAndromeda.Framework.Contexts
{

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

        ChainDomainModel[] _accessibleChains;

        public ChainDomainModel[] AccessibleChains
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

        ChainDomainModel[] _flatternedChains;

        public ChainDomainModel[] FlattenedChains
        {
            get
            {
                if (this.User == null)
                    return null;

                return _flatternedChains ?? (_flatternedChains = CurrentUser.FlatternChains(this.AccessibleChains).ToArray());
            }
        }

        MyAndromeda.Data.Domain.SiteDomainModel[] _accessibleSites;

        public SiteDomainModel[] AccessibleSites
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

        private IEnumerable<SiteDomainModel> GetAccessibleSites() 
        {
            MyAndromedaUser user = this.User;

            return usersSiteService.GetSitesDirectlyLinkedToTheUser(user.Id);
        }

        public static IEnumerable<ChainDomainModel> FlatternChains(IEnumerable<ChainDomainModel> accessibleChains)
        {
            Func<IEnumerable<ChainDomainModel>, IEnumerable<ChainDomainModel>> flatern = null;
            
            flatern = (nodes) => {
                return nodes.Union(nodes.SelectMany(e=> flatern(e.Items)));
            };

            ChainDomainModel[] all = accessibleChains.Union(accessibleChains.SelectMany(e => flatern(e.Items))).ToArray();

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