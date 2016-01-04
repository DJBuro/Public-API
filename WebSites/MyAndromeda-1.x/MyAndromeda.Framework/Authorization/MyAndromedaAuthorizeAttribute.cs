using MyAndromeda.Core;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using System;
using System.Web.Mvc;
using MyAndromeda.Logging;

namespace MyAndromeda.Framework.Authorization
{
    [AttributeUsageAttribute(AttributeTargets.Class|AttributeTargets.Method)]
    public class MyAndromedaAuthorizeAttribute : AuthorizeAttribute, IDependencyFilter 
    {
        public MyAndromedaAuthorizeAttribute() : base()
        {
        }

        private IWorkContext _workContext;
        public IWorkContext WorkContext 
        { 
            get 
            {
                return _workContext ?? (_workContext = DependencyResolver.Current.GetService<IWorkContext>());
            }
        }

        private IAuthorizer _authorizer;
        public IAuthorizer Authorizer
        {
            get
            {
                return _authorizer ?? (_authorizer = DependencyResolver.Current.GetService<IAuthorizer>());
            }
        }

        private INotifier _notifier;
        private INotifier Notifier 
        {
            get 
            {
                return _notifier ?? (_notifier = DependencyResolver.Current.GetService<INotifier>());
            }
        }

        private IMyAndromedaLogger _logger;
        public IMyAndromedaLogger Logger 
        {
            get 
            {
                return _logger ?? (_logger = DependencyResolver.Current.GetService<IMyAndromedaLogger>());
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            
            var allowAnonymous =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            
            if(allowAnonymous)
                return;

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
            var baseAuthroized = base.AuthorizeCore(httpContext);
            if (!baseAuthroized)
                return false;

            return true;
        }

        private bool ValidateSiteAgainstChain() 
        {
            if (this.WorkContext.CurrentChain.Available && !this.WorkContext.CurrentSite.Available)
                return true;

            if (!this.WorkContext.CurrentChain.Available && !this.WorkContext.CurrentSite.Available)
                return true;
            
            return this.WorkContext.CurrentSite.Site.ChainId == this.WorkContext.CurrentChain.Chain.Id;
        }

        private bool AuthorizeLocation(AuthorizationContext filterContext) 
        {
            string redirectReason = "You do not have access to this location";
            var authorizedToChainLevel = this.Authorizer.AuthorizedForChainAndStore();

            //not browsing anything significant
            if (authorizedToChainLevel.NotAccessingChain && authorizedToChainLevel.NotAccessingSite)
                return true;

            //this person should be here if at chain level but not entered a site
            if (authorizedToChainLevel.IsUserAllowedAtChainLevel && authorizedToChainLevel.NotAccessingSite)
                return true;

            //this person should be here if at chain level and at a site
            if (authorizedToChainLevel.IsUserAllowedAtChainLevel && authorizedToChainLevel.IsUserAllowedToSiteWithinChain)
                return this.ValidateSiteAgainstChain();

            //ie they have entered another site id that doesn't belong to chain 
            if (authorizedToChainLevel.IsUserAllowedAtChainLevel && !authorizedToChainLevel.IsUserAllowedToSiteWithinChain) 
            {
                this.Logger.Error("User : {0} is trying to access a site that doesn't belong to their chain.", this.WorkContext.CurrentUser.User.Username);
                this.Notifier.Notify(redirectReason);
                
                return false;
            }

            //force the chain id to be correct 
            if (!authorizedToChainLevel.IsUserAllowedAtChainLevel && authorizedToChainLevel.IsUserAllowedToSiteWithinChain) 
            {
                this.Logger.Error("Unauthorized reason: The site is correct but the chain isn't -> fix by redirect | user : {0}", this.WorkContext.CurrentUser.User.Username);
                this.Notifier.Notify(redirectReason);

                return false;
            }

            return true;
        }
    }
}