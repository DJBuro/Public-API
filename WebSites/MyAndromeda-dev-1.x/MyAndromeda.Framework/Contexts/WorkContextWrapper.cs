using System;
using System.Configuration;
using System.Diagnostics;
using MyAndromeda.Core;
using System.Web;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Framework.Contexts
{
    [DebuggerStepThrough]
    public class WorkContextWrapper : IDependency
    {
        private readonly Lazy<IWorkContext> buildWorkContext;
        private readonly Lazy<IAuthorizer> authorizer;
        private readonly HttpContext httpContext;

        private static readonly string debugModeKey = "StagingApplicationMode";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkContextWrapper" /> class.
        /// </summary>
        /// <param name="workContext">The work context.</param>
        public WorkContextWrapper(Lazy<IWorkContext> workContext, Lazy<IAuthorizer> authorizer)
        {
            try
            {
                HttpContext context = (HttpContext)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(HttpContext));
                httpContext = context;
            }
            catch { }

            this.buildWorkContext = workContext;
            this.authorizer = authorizer;
        }

        public HttpContext HttpContext 
        {
            get 
            {
                if (System.Web.HttpContext.Current == null) 
                {
                    return httpContext;
                }

                return System.Web.HttpContext.Current;
            } 
        
        }

        private IWorkContext _current;
        public IWorkContext Current 
        {
            get 
            {
                if(this._current != null)
                {
                    return this._current;
                }

                if (this.HttpContext == null) 
                { 
                    return null;
                }

                this._current = this.buildWorkContext.Value;
    
                Trace.WriteLine(string.Format("Build work context: [ chainId: {0}, storeId: {1}, user: {2} ]", 
                    _current.CurrentChain.Available ? _current.CurrentChain.Chain.Id.ToString(): " -NA- ",
                    _current.CurrentSite.Available ? _current.CurrentSite.SiteId.ToString(): " - NA - ",
                    _current.CurrentUser.Available ? _current.CurrentUser.User.Username: "- NA - "
                ));

                return this._current;
            }
        }

        private IAuthorizer _authorizer;
        public IAuthorizer Authorizer 
        {
            get
            {
                if (_authorizer != null) { return _authorizer; }

                this._authorizer = this.authorizer.Value;

                return _authorizer;
            }
        }

        /// <summary>
        /// Is the context available .
        /// </summary>
        /// <value>The available.</value>
        public bool Available
        {
            get
            {
                return this.Current != null;
            }
        }

        /// <summary>
        /// Checks if its debug mode. 
        /// ConfigurationManager.AppSettings["StagingApplicationMode"]
        /// </summary>
        /// <value>The debug mode.</value>
        public bool DebugMode
        {
            get
            {
                var value = ConfigurationManager.AppSettings[debugModeKey];

                if (value == null)
                    return false;

                return Convert.ToBoolean(value);
            }
        }
    }
}