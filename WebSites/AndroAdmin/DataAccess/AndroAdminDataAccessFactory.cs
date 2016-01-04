using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.DataAccess;

namespace AndroAdmin.DataAccess
{
    public class AndroAdminDataAccessFactory
    {
        public static IAndroWebOrderingSubscriptionDAO GetAndroWebOrderingSubscriptionDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.AndroWebOrderingSubscriptionDAO();
        }

        public static IEnvironmentsDAO GetEnvironmentsDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.EnvironmentsDAO();
        }

        public static IAMSServerDAO GetAMSServerDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.AMSServerDAO();
        }

        public static IFTPSiteDAO GetFTPSiteDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.FtpSiteDAO();
        }

        public static IFTPSiteTypeDAO GetFTPSiteTypeDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.FtpSiteTypeDAO();
        }

        public static IStoreDAO GetStoreDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreDAO();
        }

        public static IStoreStatusDAO GetStoreStatusDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreStatusDAO();
        }

        public static IStoreAMSServerDAO GetStoreAMSServerDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreAMSServerDAO();
        }

        public static IStoreAMSServerFTPSiteDAO GetStoreAMSServerFTPSiteDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreAMSServerFtpSiteDAO();
        }
        
        public static ILogDAO GetLogDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.LogDAO();
        }

        public static IPartnerDAO GetPartnerDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.PartnerDAO();
        }

        public static IACSApplicationDAO GetACSApplicationDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.ACSApplicationDAO();
        }

        public static IACSApplicationEFDAO GetACSApplicationEFDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.ACSApplicationEFDAO();
        }

        public static IHostDAO GetHostDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.HostDAO();
        }

        public static ICountryDAO GetCountryDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.CountryDAO();
        }

        public static ISettingsDAO GetSettingsDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.SettingsDAO();
        }

        public static IStorePaymentProviderDAO GetStorePaymentProviderDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StorePaymentProviderDAO();
        }

        public static IOpeningHoursDAO GetOpeningHoursDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.OpeningHoursDAO();
        }

        public static IChainDAO GetChainDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.ChainDAO();
        }

        /// <summary>
        /// Gets the hub data service.
        /// </summary>
        /// <returns></returns>
        public static IHubDataService GetHubDataService()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.HubDataService();
        }

        /// <summary>
        /// Gets the store hub data service.
        /// </summary>
        /// <returns></returns>
        public static IStoreHubDataService GetStoreHubDataService() 
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreHubDataService();
        }

        /// <summary>
        /// Hubs the reset data service.
        /// </summary>
        /// <returns></returns>
        public static IHubResetDataService GetHubResetDataService()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.HubDataService();
        }

        /// <summary>
        /// Gets the host v2 data service.
        /// </summary>
        /// <returns></returns>
        public static IHostV2DataService GetHostV2DataService() 
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.HostV2DataConnectionService();
        }

        /// <summary>
        /// Gets the host type data service.
        /// </summary>
        /// <returns></returns>
        public static IHostTypesDataService GetHostTypeDataService() 
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.HostTypesDataService();
        }

        public static IAndroWebOrderingThemeDAO GetAndroWebOrderingThemeDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.AndroWebOrderingThemeDAO();
        }

        public static IAndroWebOrderingWebsiteDAO GetAndroWebOrderingWebSiteDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.AndroWebOrderingWebsiteDAO();
        }

        internal static IHostV2ForStoreDataService GetHostV2ForStoreDataService()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.HostV2ForStoreDataService();
        }

        internal static IHostV2ForApplicationDataService GetHostV2ForApplicationDataService() 
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.HostV2ForApplicationDataService();
        }
    }
}