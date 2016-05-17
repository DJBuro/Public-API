using System;
using System.Diagnostics;
using MyAndromeda.Framework.Dates;
using System.Reflection;
using System.Web.Compilation;

namespace MyAndromeda.Framework.Contexts
{
    //[DebuggerStepThrough]
    [DebuggerDisplay("CurrentChain available: {CurrentChain.Available} | CurrentSite available: {CurrentSite.Available}")]
    public class WorkContext : IWorkContext 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkContext" /> class.
        /// </summary>
        /// <param name="currentSite">The current site.</param>
        public WorkContext(
            ILocalizationContext localizationContext,
            ICurrentChain currentChain, 
            ICurrentSite currentSite, 
            ICurrentUser currentUser, 
            ICurrentRequest currentRequest,
            Func<IDateServices> dateServiceFactory)
        { 
            this.LocalizationContext = localizationContext;
            this.CurrentChain = currentChain;
            this.CurrentSite = currentSite;
            this.CurrentUser = currentUser;
            this.CurrentRequest = currentRequest;
            this.DateServicesFactory = dateServiceFactory;
        }

        /// <summary>
        /// Gets or sets the localization context.
        /// </summary>
        /// <value>The localization context.</value>
        public ILocalizationContext LocalizationContext { get; private set; }

        /// <summary>
        /// Gets or sets the current chain.
        /// </summary>
        /// <value>The current chain.</value>
        public ICurrentChain CurrentChain { get; private set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>The current user.</value>
        public ICurrentUser CurrentUser { get; private set; }

        /// <summary>
        /// Gets or sets the current site.
        /// </summary>
        /// <value>The current site.</value>
        public ICurrentSite CurrentSite { get; private set; }

        /// <summary>
        /// Gets or sets the current request.
        /// </summary>
        /// <value>The current request.</value>
        public ICurrentRequest CurrentRequest { get; private set; }
        
        /// <summary>
        /// Gets or sets the date services factory.
        /// </summary>
        /// <value>The date services factory.</value>
        public Func<IDateServices> DateServicesFactory { get; private set; }

        private Version version; 

        public Version Version 
        {
            get 
            {
                if(!this.CurrentRequest.Available){ return null; }

                return version ?? (version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version); 
            } 
        }
    }
}