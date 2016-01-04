using AndroAdminDataAccess.Domain;
using AA = AndroAdminDataAccess.EntityFramework;
using DataWarehouseDataAccess.Domain;
using DW = DataWarehouseDataAccessEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration
{
    public class GPSIntegrationServices
    {
        /// <summary>
        /// Gets a stores third party provider configuration details for the specified store from the AndroAdmin db
        /// </summary>
        /// <param name="andromedaStoreId">An Andromeda store id</param>
        /// <param name="store">The third party provider login details</param>
        /// <returns></returns>
        public virtual ResultEnum GetStoreByAndromedaStoreId(int andromedaStoreId, out Model.StoreConfiguration store)
        {
            ResultEnum result = ResultEnum.UnknownError;
            store = null;

            // Get the GPS store settings from the database
            AA.EntityFrameworkDataAccessFactory entityFrameworkDataAccessFactory = new AA.EntityFrameworkDataAccessFactory();
            StoreGPSSettings storeBringgSettings = entityFrameworkDataAccessFactory.StoreGPSSettingsDAO.GetById(andromedaStoreId);

            if (storeBringgSettings == null)
            {
                result = ResultEnum.NoStoreSettings;
            }
            else
            {
                store = new Model.StoreConfiguration()
                {
                    AndromedaStoreId = storeBringgSettings.Id,
                    MaxDrivers = storeBringgSettings.MaxDrivers,
                    PartnerConfiguration = storeBringgSettings.PartnerConfig
                };

                result = ResultEnum.OK;
            }

            return result;
        }

        /// <summary>
        /// Updates the third party providers configuration in the AndroAdmin db
        /// </summary>
        /// <param name="store">The third party provider login details to update</param>
        /// <returns></returns>
        public virtual ResultEnum UpdateStore(Model.StoreConfiguration store)
        {
            // See if there are already Bringg settings for this store
            Model.StoreConfiguration model = null;
            ResultEnum result = this.GetStoreByAndromedaStoreId(store.AndromedaStoreId, out model);

            if (result == ResultEnum.NoStoreSettings)
            {
                // There are no partner settings for this store
                // Add the store partner settings to the db
                AA.EntityFrameworkDataAccessFactory entityFrameworkDataAccessFactory = new AA.EntityFrameworkDataAccessFactory();
                AndroAdminDataAccess.Domain.StoreGPSSettings storeBringgSettings = new AndroAdminDataAccess.Domain.StoreGPSSettings()
                {
                    Id = store.AndromedaStoreId,
                    MaxDrivers = store.MaxDrivers,
                    PartnerConfig = store.PartnerConfiguration
                };

                bool success = entityFrameworkDataAccessFactory.StoreGPSSettingsDAO.Add(storeBringgSettings);

                if (success) result = ResultEnum.OK;
                else result = ResultEnum.UnknownError;
            }
            else
            {                
                // Update the store GPS settings in the db
                AA.EntityFrameworkDataAccessFactory entityFrameworkDataAccessFactory = new AA.EntityFrameworkDataAccessFactory();
                AndroAdminDataAccess.Domain.StoreGPSSettings storeBringgSettings = new AndroAdminDataAccess.Domain.StoreGPSSettings()
                {
                    Id = store.AndromedaStoreId,
                    MaxDrivers = store.MaxDrivers,
                    PartnerConfig = store.PartnerConfiguration
                };

                bool success = entityFrameworkDataAccessFactory.StoreGPSSettingsDAO.Update(storeBringgSettings);

                if (success) result = ResultEnum.OK;
                else result = ResultEnum.UnknownError;
            }

            return result;
        }
    }
}
