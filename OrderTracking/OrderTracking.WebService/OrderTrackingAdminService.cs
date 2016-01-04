using System.Configuration;
using OrderTracking.Core;
using OrderTracking.Dao;
using OrderTracking.WebService.Dao;
using OrderTracking.WebService.Gps;
using OrderTracking.Dao.Domain;

namespace OrderTracking
{
    public class OrderTrackingAdminService : IOrderTrackingAdmin
    {
        #region IDAOs

        public IAccountDao AccountDao { get; set; }
        public IStoreDao StoreDao { get; set; }
        public ICoordinatesDao CoordinatesDao { get; set; }
        
        #endregion

        #region IOrderTrackingAdmin Members

        public Results AddTrackersToStore(string userName, string password)
        {
            var results = new Results { Success = true };

            Account account;
            if (AccountDao.Verify(userName, password, out account))
            {
                results.AddHocError("AddTrackerToStore", "Sorry, this isn't implemented yet", account.Store);
            }

            return results;
        }

        public Results AddUpdateStore(string userName, string password, StoreDetails storeDetails)
        {
            var results = new Results { Success = true };

            Account account;
            if (AccountDao.Verify(userName, password, out account))
            {
                if (account.Store.ExternalStoreId != storeDetails.ExternalStoreId)
                    return results.AddHocError("AddUpdateStore", "The externalStoreId is incorrect for this account",
                                        account.Store);

                UpdateStore(account, results, storeDetails);

                return results;
            }

            return results.FailedLogin(userName, password, "AddUpdateStore");
        }

        /// <summary>
        /// Used for OrderTracking Admin and AndroDB
        /// </summary>
        /// <param name="storeDetails"></param>
        /// <returns></returns>
        public Results AddUpdateAccount(StoreDetails storeDetails)
        {
            var results = new Results { Success = true };

            Account account;
            AccountDao.Verify(storeDetails.AccountUserName, storeDetails.AccountPassword, out account);

            if(account == null)//new Account!
            {
                 var coordinates = CoordinatesDao.Create(new Coordinates());

                 results = storeDetails.Geocode(results, ref coordinates, ConfigurationManager.AppSettings.Get("Deployed"));

                var store = new Store
                                {
                                    Name = storeDetails.StoreName,
                                    ExternalStoreId = storeDetails.ExternalStoreId,
                                    DeliveryRadius = storeDetails.DeliveryRadius,
                                    Coordinates = coordinates
                                };

                StoreDao.Save(store); 

                account = new Account
                              {
                                  Store = store,
                                  GpsEnabled = storeDetails.GpsEnabled,
                                  UserName = storeDetails.AccountUserName,
                                  Password = storeDetails.AccountPassword
                              };
            }
            else //updating a store account
            {
                var coordinates = account.Store.Coordinates;

                results = storeDetails.Geocode(results, ref coordinates, ConfigurationManager.AppSettings.Get("Deployed"));

                account.Store.Name = storeDetails.StoreName;
                account.Store.ExternalStoreId = storeDetails.ExternalStoreId;
                account.Store.DeliveryRadius = storeDetails.DeliveryRadius;
                account.Store.Coordinates = coordinates;

                account.Store = account.Store;
                account.GpsEnabled = storeDetails.GpsEnabled;
                account.UserName = storeDetails.AccountUserName;
                account.Password = storeDetails.AccountPassword;
            }

            AccountDao.Save(account);

            return results;
        }

        //TODO: do we need this anymore???
        private static Results UpdateStore(Account account, Results results, StoreDetails storeDetails)
        {
            //Note: this is only updating names and GPS
            account.GpsEnabled = storeDetails.GpsEnabled;
            account.Store.Name = storeDetails.StoreName;
            account.Store.DeliveryRadius = storeDetails.DeliveryRadius;

            if (account.Store.DeliveryRadius == 0)
                account.Store.DeliveryRadius = 10;

            //log it regardless...
            results.AddHocWarning("UpdateStore", "Update Store", account.Store);

            return results;
        }


        #endregion
    }
}
