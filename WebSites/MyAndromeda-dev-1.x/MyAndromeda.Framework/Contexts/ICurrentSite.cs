using MyAndromeda.Core;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.AndroAdmin;
using Domain = MyAndromedaDataAccess.Domain;
using System.Collections.Generic;

namespace MyAndromeda.Framework.Contexts
{
    public interface ICurrentSite : IDependency
    {
        /// <summary>
        /// Is a site's information available.
        /// </summary>
        /// <value>The available.</value>
        bool Available { get; }

        /// <summary>
        /// Gets the site id.
        /// </summary>
        /// <value>The site id.</value>
        int SiteId { get; }

        /// <summary>
        /// Gets the Andromeda id.
        /// </summary>
        /// <value>The Andromeda id.</value>
        int AndromediaSiteId { get; }

        /// <summary>
        /// Gets the external site id.
        /// </summary>
        /// <value>The external site id.</value>
        string ExternalSiteId { get; }

        /// <summary>
        /// Gets the chain id of the store.
        /// </summary>
        /// <value>The chain id.</value>
        int ChainId { get; }

        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <value>The site.</value>
        MyAndromeda.Data.Domain.Site Site { get; }

        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        Store Store { get; }

        /// <summary>
        /// Gets the acs application ids.
        /// </summary>
        /// <value>The acs application ids.</value>
        IEnumerable<int> AcsApplicationIds { get; }

        /// <summary>
        /// Gets the acs external application ids.
        /// </summary>
        /// <value>The acs external application ids.</value>
        IEnumerable<string> AcsExternalApplicationIds { get; }

        /// <summary>
        /// Gets the enrolment level.
        /// </summary>
        /// <value>The enrolment level.</value>
        IEnumerable<IEnrolmentLevel> EnrolmentLevels { get; }

        /// <summary>
        /// Gets the authorized at site level.
        /// </summary>
        /// <value>The authorized at site level.</value>
        bool AuthorizedAtSiteLevel { get; }

        IEnumerable<AndroWebOrderingWebsite> AndroWebOrderingSites { get; }
        
    }
}