using MyAndromeda.Core;
using MyAndromeda.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccess;

namespace MyAndromeda.Web.Areas.Reporting.Context
{
    public interface IReportingContext : IDependency 
    {
        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        /// <value>The site id.</value>
        int SiteId { get; }

        /// <summary>
        /// Gets or sets the chain id.
        /// </summary>
        /// <value>The chain id.</value>
        int ChainId { get; }

        /// <summary>
        /// Gets or sets the show day.
        /// </summary>
        /// <value>The show day.</value>
        DateTime? ShowDay { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        /// <value>The date from.</value>
        DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        /// <value>The date to.</value>
        DateTime? DateTo { get; set; }

        //string ExternalSiteId { get; }
        
        /// <summary>
        /// Gets the acs application ids.
        /// </summary>
        /// <value>The acs application ids.</value>
        IEnumerable<int> AcsApplicationIds { get; }

        /// <summary>
        /// Gets the external id.
        /// </summary>
        /// <value>The external id.</value>
        string ExternalId { get; }

        /// <summary>
        /// Gets the name of the site.
        /// </summary>
        /// <value>The name of the site.</value>
        string SiteName { get; }
    }
    
    public class ReportingContext : IReportingContext
    {
        private readonly WorkContextWrapper workContextWrapper;

        public ReportingContext(WorkContextWrapper workContextWrapper)
        {
            this.workContextWrapper = workContextWrapper;
        }

        /// <summary>
        /// Gets or sets the show day.
        /// </summary>
        /// <value>The show day.</value>
        public DateTime? ShowDay { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        /// <value>The date from.</value>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        /// <value>The date to.</value>
        public DateTime? DateTo { get; set; }

        public string SiteName
        {
            get 
            {
                return this.workContextWrapper.Current.CurrentSite.Site.ClientSiteName;
            } 
        }

        public string ExternalId
        {
            get
            {
                return this.workContextWrapper.Current.CurrentSite.Site.ExternalSiteId;
            }
        }

        public IEnumerable<int> AcsApplicationIds
        {
            get 
            {
                return this.workContextWrapper.Current.CurrentSite.AcsApplicationIds;
            }   
        }

        public IEnumerable<string> AcsExternalApplicationIds 
        {
            get 
            {
                return this.workContextWrapper.Current.CurrentSite.AcsExternalApplicationIds;
            }
        }

        /// <summary>
        /// Gets the site id.
        /// </summary>
        /// <value>The site id.</value>
        public int SiteId
        {
            get
            {
                return this.workContextWrapper.Current.CurrentSite.Site.Id;
            }
        }

        /// <summary>
        /// Gets the external site id.
        /// </summary>
        /// <value>The external site id.</value>
        public string ExternalSiteId
        {
            get 
            {
                return this.workContextWrapper.Current.CurrentSite.Site.ExternalSiteId;
            }
        }

        /// <summary>
        /// Gets the chain id.
        /// </summary>
        /// <value>The chain id.</value>
        public int ChainId
        {
            get
            {
                return this.workContextWrapper.Current.CurrentChain.Chain.Id;
            }
        }
    }
}