using System;
using System.Diagnostics;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Framework.Contexts
{
    /// <summary>
    /// Single instance of work context per user call that loads in the current state of the application running.
    /// </summary>
    public interface IWorkContext : IDependency
    {
        /// <summary>
        /// Gets the localization context.
        /// </summary>
        /// <value>The localization context.</value>
        ILocalizationContext LocalizationContext { get; }

        /// <summary>
        /// Gets or sets the current chain.
        /// </summary>
        /// <value>The current chain.</value>
        ICurrentChain CurrentChain { get; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>The current user.</value>
        ICurrentUser CurrentUser { get; }

        /// <summary>
        /// Gets or sets the current site.
        /// </summary>
        /// <value>The current site.</value>
        ICurrentSite CurrentSite { get; }

        /// <summary>
        /// Gets the current request data.
        /// </summary>
        /// <value>The current request.</value>
        ICurrentRequest CurrentRequest { get; }

        Func<IDateServices> DateServicesFactory { get; }

        Version Version { get; }
    }
}
