using MyAndromeda.Core;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using System;
using System.Web.Mvc;
using MyAndromeda.Logging;
using Ninject;

namespace MyAndromeda.Framework.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyAndromedaAuthorizeAttribute : AuthorizeAttribute, IDependencyFilter
    {
        public MyAndromedaAuthorizeAttribute()
            : base()
        {

        }

        private IWorkContext workContext;

        [Inject]
        public IWorkContext WorkContext
        {
            get { return this.workContext; }
            set { this.workContext = value; }
        }

        private IAuthorizer authorizer;
        [Inject]
        public IAuthorizer Authorizer
        {
            get { return this.authorizer; }
            set { this.authorizer = value; }
        }

        private INotifier notifier;
        [Inject]
        private INotifier Notifier
        {
            get
            {
                return notifier;
            }
            set { this.notifier = value; }
        }

        private IMyAndromedaLogger logger;
        [Inject]
        public IMyAndromedaLogger Logger
        {
            get { return this.logger; }
            set { this.logger = value; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException(paramName: "filterContext");

            bool allowAnonymous =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (allowAnonymous)
            {
                return;
            }

            if (!this.AuthorizeCore(filterContext.HttpContext))
            {
                this.HandleUnauthorizedRequest(filterContext);
                return;
            }

            //authorize user location 
            if (!this.AuthorizeLocation(filterContext))
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            bool baseAuthroized = base.AuthorizeCore(httpContext);
            if (!baseAuthroized)
                return false;

            return true;
        }

        private bool ValidateSiteAgainstChain()
        {
            if (this.WorkContext.CurrentChain.Available && !this.WorkContext.CurrentSite.Available)
            {
                return true;
            }

            if (!this.WorkContext.CurrentChain.Available && !this.WorkContext.CurrentSite.Available)
            {
                return true;
            }

            return this.WorkContext.CurrentSite.Site.ChainId == this.WorkContext.CurrentChain.Chain.Id;
        }

        private bool AuthorizeLocation(AuthorizationContext filterContext)
        {
            string redirectReason = "You do not have access to this location";
            ChainAndSiteAuthorization authorizedToChainLevel = this.Authorizer.AuthorizedForChainAndStore();

            //not browsing anything significant
            if (authorizedToChainLevel.NotAccessingChain && authorizedToChainLevel.NotAccessingSite)
            {
                return true;
            }

            //this person should be here if at chain level but not entered a site
            if (authorizedToChainLevel.IsUserAllowedAtChainLevel && authorizedToChainLevel.NotAccessingSite)
            {
                return true;
            }

            //this person should be here if at chain level and at a site
            if (authorizedToChainLevel.IsUserAllowedAtChainLevel && authorizedToChainLevel.IsUserAllowedToSiteWithinChain)
            {
                return this.ValidateSiteAgainstChain();
            }

            //ie they have entered another site id that doesn't belong to chain 
            if (authorizedToChainLevel.IsUserAllowedAtChainLevel && !authorizedToChainLevel.IsUserAllowedToSiteWithinChain)
            {
                this.Logger.Error(format: "User : {0} is trying to access a site that doesn't belong to their chain.", arg0: this.WorkContext.CurrentUser.User.Username);
                this.Notifier.Notify(redirectReason);

                return false;
            }

            //force the chain id to be correct 
            if ((!authorizedToChainLevel.IsUserAllowedToSiteWithinChain))
            {
                this.Logger.Error(format: "Unauthorized reason: The site is correct but the chain isn't -> fix by redirect | user : {0}", arg0: this.WorkContext.CurrentUser.User.Username);
                this.Notifier.Notify(redirectReason);

                return false;
            }

            return true;
        }
    }
}