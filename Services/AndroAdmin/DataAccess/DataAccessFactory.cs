using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.DataAccess;

namespace AndroAdmin.DataAccess
{
    public class DataAccessFactory
    {
        public static IAMSServerDAO GetAMSServerDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.AMSServerDAO();
        }

        public static IFTPSiteDAO GetFTPSiteDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.FtpSiteDAO();
        }

        public static IStoreDAO GetStoreDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreDAO();
        }

        public static ILogDAO GetLogDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.LogDAO();
        }

        public static IStoreAMSServerDAO GetStoreAMSServerDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreAMSServerDAO();
        }

        public static IStoreAMSServerFTPSiteDAO GetStoreAMSServerFTPSiteDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreAMSServerFtpSiteDAO();
        }

        public static IAddressDAO GetAddressDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.AddressDAO();
        }

        public static IStoreStatusDAO GetStoreStatusDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.StoreStatusDAO();
        }

        public static ICountryDAO GetCountryDAO()
        {
            return new AndroAdminDataAccess.EntityFramework.DataAccess.CountryDAO();
        }
    }
}