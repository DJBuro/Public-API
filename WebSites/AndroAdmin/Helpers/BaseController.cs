using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdmin.Services.Notifications;
using AndroAdminDataAccess.DataAccess;
using AndroAdmin.DataAccess;
using AndroUsersDataAccess.DataAccess;
using System.Configuration;
using DataWarehouseDataAccess.DataAccess;
using System.Reflection;

namespace AndroAdmin.Helpers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// All the permissions that the current user has
        /// </summary>
        protected List<string> permissions { get; set; }

        public string AndroAdminConnectionStringOverride { get; set; }
        public string AndroUsersConnectionStringOverride { get; set; }

        public BaseController()
        {
            string showMachineDetails = ConfigurationManager.AppSettings["ShowMachineDetails"];

            if (showMachineDetails != null && showMachineDetails.Length > 0)
            {
                ViewBag.ShowMachineDetails = true;
                ViewBag.Machine = showMachineDetails;
                ViewBag.DBServer = AndroAdminDataAccess.EntityFramework.EntityFrameworkDataAccessFactory.DBServer;
                ViewBag.DBName = AndroAdminDataAccess.EntityFramework.EntityFrameworkDataAccessFactory.DBName;
                ViewBag.Version = string.Format("v{0}.{1}", AssemblyVersion.Major, AssemblyVersion.Minor);
            }
            else
            {
                ViewBag.ShowMachineDetails = false;
            }

            this.Notifier = new Notifier(this);
        }

        private static Version assemblyVersion;
        public Version AssemblyVersion
        {
            get
            {
                return assemblyVersion ?? (assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version);
            }
        }

        public Notifier Notifier { get; set; }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // Get a list of permissions that the user has.  We only want to do this once and share the results with the master page/s, view and controller.
            AndroUsersDataAccess.DataAccess.IPermissionDAO permissionsDAO = AndroAdmin.DataAccess.AndroUsersDataAccessFactory.GetPermissionsDAO();
            //           IPermissionDAO permissionsDAO.ConnectionStringOverride = this.AndroUsersConnectionStringOverride;

            // Is the user logged in
            if (this.User != null && this.User.Identity != null && this.User.Identity.Name != null)
            {
                // Get a list of all the users permissions
                this.permissions = permissionsDAO.GetNamesByUserName(User.Identity.Name);

                // Make the users permissions available to master pages and views - TempData is not ideal but still a lot faster than accessing the db over and over
                ViewBag.Permissions = this.permissions;
            }
            else
            {
                // Not logged in - return an empty collection so we don't have to arse about testing for null :)
                ViewBag.Permissions = new List<string>();
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is UnauthorizedAccessException)
            {
                //
                // Manage the Unauthorized Access exceptions
                // by redirecting the user to Home page.
                //
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Home", "Index");
            }
            //
            base.OnException(filterContext);
        }
        
        /// <summary>
        /// Store DAO
        /// </summary>
        public virtual IStoreDAO StoreDAO
        { 
            get
            {
                if (this.storeDAO == null)
                {
                    this.storeDAO  = AndroAdminDataAccessFactory.GetStoreDAO();
                    this.storeDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.storeDAO;
            }
            set { this.storeDAO = value; }
        }
        private IStoreDAO storeDAO = null;

        /// <summary>
        /// Store status DAO
        /// </summary>
        public virtual IStoreStatusDAO StoreStatusDAO
        {
            get
            {
                if (this.storeStatusDAO == null)
                {
                    this.storeStatusDAO = AndroAdminDataAccessFactory.GetStoreStatusDAO();
                    this.storeStatusDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.storeStatusDAO;
            }
            set { this.storeStatusDAO = value; }
        }
        private IStoreStatusDAO storeStatusDAO = null;

        /// <summary>
        /// Store AMS Server DAO
        /// </summary>
        public virtual IStoreAMSServerDAO StoreAMSServerDAO
        {
            get
            {
                if (this.storeAMSServerDAO == null)
                {
                    this.storeAMSServerDAO = AndroAdminDataAccessFactory.GetStoreAMSServerDAO();
                    this.storeAMSServerDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.storeAMSServerDAO;
            }
            set { this.storeAMSServerDAO = value; }
        }
        private IStoreAMSServerDAO storeAMSServerDAO = null;

        /// <summary>
        /// Store AMS ServerFTP site DAO
        /// </summary>
        public virtual IStoreAMSServerFTPSiteDAO StoreAMSServerFTPSiteDAO
        {
            get
            {
                if (this.storeAMSServerFTPSiteDAO == null)
                {
                    this.storeAMSServerFTPSiteDAO = AndroAdminDataAccessFactory.GetStoreAMSServerFTPSiteDAO();
                    this.storeAMSServerFTPSiteDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.storeAMSServerFTPSiteDAO;
            }
            set { this.storeAMSServerFTPSiteDAO = value; }
        }
        private IStoreAMSServerFTPSiteDAO storeAMSServerFTPSiteDAO = null; 

        /// <summary>
        /// AMS Server DAO
        /// </summary>
        public virtual IAMSServerDAO AMSServerDAO
        {
            get
            {
                if (this.amsServerDAO == null)
                {
                    this.amsServerDAO = AndroAdminDataAccessFactory.GetAMSServerDAO();
                    this.amsServerDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.amsServerDAO;
            }
            set { this.amsServerDAO = value; }
        }
        private IAMSServerDAO amsServerDAO = null;

        /// <summary>
        /// FTP Site DAO
        /// </summary>
        public virtual IFTPSiteDAO FTPSiteDAO
        {
            get
            {
                if (this.ftpSiteDAO == null)
                {
                    this.ftpSiteDAO = AndroAdminDataAccessFactory.GetFTPSiteDAO();
                    this.ftpSiteDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.ftpSiteDAO;
            }
            set { this.ftpSiteDAO = value; }
        }
        private IFTPSiteDAO ftpSiteDAO = null;

        /// <summary>
        /// FTP Site Type DAO
        /// </summary>
        public virtual IFTPSiteTypeDAO FTPSiteTypeDAO
        {
            get
            {
                if (this.ftpSiteTypeDAO == null)
                {
                    this.ftpSiteTypeDAO = AndroAdminDataAccessFactory.GetFTPSiteTypeDAO();
                    this.ftpSiteTypeDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.ftpSiteTypeDAO;
            }
            set { this.ftpSiteTypeDAO = value; }
        }
        private IFTPSiteTypeDAO ftpSiteTypeDAO = null;

        /// <summary>
        /// Log DAO
        /// </summary>
        public virtual ILogDAO LogDAO
        {
            get
            {
                if (this.logDAO == null)
                {
                    this.logDAO = AndroAdminDataAccessFactory.GetLogDAO();
                    this.logDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.logDAO;
            }
            set { this.logDAO = value; }
        }
        private ILogDAO logDAO = null;

        /// <summary>
        /// Partner DAO
        /// </summary>
        public virtual IPartnerDAO PartnerDAO
        {
            get
            {
                if (this.partnerDAO == null)
                {
                    this.partnerDAO = AndroAdminDataAccessFactory.GetPartnerDAO();
                    this.partnerDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.partnerDAO;
            }
            set { this.partnerDAO = value; }
        }
        private IPartnerDAO partnerDAO = null;

        /// <summary>
        /// ACSApplication DAO
        /// </summary>
        public virtual IACSApplicationDAO ACSApplicationDAO
        {
            get
            {
                if (this.acsApplicationDAO == null)
                {
                    this.acsApplicationDAO = AndroAdminDataAccessFactory.GetACSApplicationDAO();
                    this.acsApplicationDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.acsApplicationDAO;
            }
            set { this.acsApplicationDAO = value; }
        }
        private IACSApplicationDAO acsApplicationDAO = null;

        /// <summary>
        /// ACSApplicationEF DAO
        /// </summary>
        public virtual IACSApplicationEFDAO ACSApplicationEFDAO
        {
            get
            {
                if (this.acsApplicationEFDAO == null)
                {
                    this.acsApplicationEFDAO = AndroAdminDataAccessFactory.GetACSApplicationEFDAO();
                    this.acsApplicationEFDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.acsApplicationEFDAO;
            }
            set { this.acsApplicationEFDAO = value; }
        }
        private IACSApplicationEFDAO acsApplicationEFDAO = null;

        /// <summary>
        /// Host DAO
        /// </summary>
        public virtual IHostDAO HostDAO
        {
            get
            {
                if (this.hostDAO == null)
                {
                    this.hostDAO = AndroAdminDataAccessFactory.GetHostDAO();
                    this.hostDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.hostDAO;
            }
            set { this.hostDAO = value; }
        }
        private IHostDAO hostDAO = null;

        /// <summary>
        /// Country DAO
        /// </summary>
        public virtual ICountryDAO CountryDAO
        {
            get
            {
                if (this.countryDAO == null)
                {
                    this.countryDAO = AndroAdminDataAccessFactory.GetCountryDAO();
                    this.countryDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.countryDAO;
            }
            set { this.countryDAO = value; }
        }
        private ICountryDAO countryDAO = null;

        /// <summary>
        /// StorePaymentProvider DAO
        /// </summary>
        public virtual IStorePaymentProviderDAO StorePaymentProviderDAO
        {
            get
            {
                if (this.storePaymentProvider == null)
                {
                    this.storePaymentProvider = AndroAdminDataAccessFactory.GetStorePaymentProviderDAO();
                    this.storePaymentProvider.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.storePaymentProvider;
            }
            set { this.storePaymentProvider = value; }
        }
        private IStorePaymentProviderDAO storePaymentProvider = null;

        /// <summary>
        /// AndroUser DAO
        /// </summary>
        public virtual IAndroUserDAO AndroUserDAO
        {
            get
            {
                if (this.androUserDAO == null)
                {
                    this.androUserDAO = AndroUsersDataAccessFactory.GetAndroUserDAO();
                    this.androUserDAO.ConnectionStringOverride = this.AndroUsersConnectionStringOverride;
                }

                return this.androUserDAO;
            }
            set { this.androUserDAO = value; }
        }
        private IAndroUserDAO androUserDAO = null;

        /// <summary>
        /// SecurityGroup DAO
        /// </summary>
        public virtual ISecurityGroupDAO SecurityGroupDAO
        {
            get
            {
                if (this.securityGroupDAO == null)
                {
                    this.securityGroupDAO = AndroUsersDataAccessFactory.GetAndroSecurityGroupDAO();
                    this.securityGroupDAO.ConnectionStringOverride = this.AndroUsersConnectionStringOverride;
                }

                return this.securityGroupDAO;
            }
            set { this.securityGroupDAO = value; }
        }
        private ISecurityGroupDAO securityGroupDAO = null;

        /// <summary>
        /// Permissions DAO
        /// </summary>
        public virtual IPermissionDAO PermissionsDAO
        {
            get
            {
                if (this.permissionsDAO == null)
                {
                    this.permissionsDAO = AndroUsersDataAccessFactory.GetPermissionsDAO();
                    this.permissionsDAO.ConnectionStringOverride = this.AndroUsersConnectionStringOverride;
                }

                return this.permissionsDAO;
            }
            set { this.permissionsDAO = value; }
        }
        private IPermissionDAO permissionsDAO = null;

        /// <summary>
        /// Opening hours DAO
        /// </summary>
        public virtual IOpeningHoursDAO OpeningHoursDAO
        {
            get
            {
                if (this.openingHoursDAO == null)
                {
                    this.openingHoursDAO = AndroAdminDataAccessFactory.GetOpeningHoursDAO();
                    this.openingHoursDAO.ConnectionStringOverride = this.AndroUsersConnectionStringOverride;
                }

                return this.openingHoursDAO;
            }
            set { this.openingHoursDAO = value; }
        }
        private IOpeningHoursDAO openingHoursDAO = null;

        /// <summary>
        /// Chain DAO
        /// </summary>
        public virtual IChainDAO ChainDAO
        {
            get
            {
                if (this.chainDAO == null)
                {
                    this.chainDAO = AndroAdminDataAccessFactory.GetChainDAO();
                    this.chainDAO.ConnectionStringOverride = this.AndroUsersConnectionStringOverride;
                }

                return this.chainDAO;
            }
            set { this.chainDAO = value; }
        }
        private IChainDAO chainDAO = null;

        /// <summary>
        /// OrderMetrics DAO
        /// </summary>
        public virtual IOrderMetricsDataAccess OrderMetricsDAO
        {
            get
            {
                if (this.orderMetricsDAO == null)
                {
                    this.orderMetricsDAO = DataWarehouseDataAccessFactory.GetOrderMetricsDataAccess();
                    this.orderMetricsDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }

                return this.orderMetricsDAO;
            }
            set { this.orderMetricsDAO = value; }
        }
        private IOrderMetricsDataAccess orderMetricsDAO = null;

        /// <summary>
        /// WebOrdering Theme DAO
        /// </summary>
        public virtual IAndroWebOrderingThemeDAO AndroWebOrderingThemeDAO
        {
            get
            {
                if (this.androWebOrderingThemeDAO == null)
                {
                    this.androWebOrderingThemeDAO = AndroAdminDataAccessFactory.GetAndroWebOrderingThemeDAO();
                    this.androWebOrderingThemeDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }
                return this.androWebOrderingThemeDAO;
            }
            set { this.androWebOrderingThemeDAO = value; }
        }
        private IAndroWebOrderingThemeDAO androWebOrderingThemeDAO = null;


        /// <summary>
        /// WebOrdering website DAO
        /// </summary>
        public virtual IAndroWebOrderingWebsiteDAO AndroWebOrderingWebSiteDAO
        {
            get
            {
                if (this.androWebOrderingWebSiteDAO == null)
                {
                    this.androWebOrderingWebSiteDAO = AndroAdminDataAccessFactory.GetAndroWebOrderingWebSiteDAO();
                    this.androWebOrderingWebSiteDAO.ConnectionStringOverride = this.AndroAdminConnectionStringOverride;
                }
                return this.androWebOrderingWebSiteDAO;
            }
            set { this.androWebOrderingWebSiteDAO = value; }
        }
        private IAndroWebOrderingWebsiteDAO androWebOrderingWebSiteDAO = null;

        private IAndroWebOrderingSubscriptionDAO androWebOrderingSubscriptionDAO = null;

        public virtual IAndroWebOrderingSubscriptionDAO AndroWebOrderingSubscriptionDAO 
        {
            get 
            {
                if (this.androWebOrderingSubscriptionDAO == null) 
                {
                    this.androWebOrderingSubscriptionDAO = AndroAdminDataAccessFactory.GetAndroWebOrderingSubscriptionDAO();
                }

                return this.androWebOrderingSubscriptionDAO;
            }
        }

        private IEnvironmentsDAO environmentDAO = null;
        public virtual IEnvironmentsDAO EnvironmentDAO
        {
            get
            {
                if (this.environmentDAO == null)
                {
                    this.environmentDAO = AndroAdminDataAccessFactory.GetEnvironmentsDAO();
                }

                return this.environmentDAO;
            }
        }

        internal void ValidateSync()
        {
            string errorMessage = CloudSync.SyncHelper.ServerSync();

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                this.ModelState.AddModelError("Id", "The host could not be synced.");
            }
        }
    }
}