using System;
using System.Linq;
using Andromeda.GPSIntegration.Model;
using Newtonsoft.Json;
using Andromeda.GPSIntegration.Bringg.APIModel;

namespace Andromeda.GPSIntegration.Bringg
{
    /// <summary>
    /// The Bringg implementation of GPS integration
    /// </summary>
    public class BringgGPSIntegrationServices : GPSIntegrationServices, IGPSIntegrationServices
    {
        private BringgAPI BringgAPI = new BringgAPI();

        /// <summary>
        /// A simple call to Bringg to check that the login details work
        /// </summary>
        /// <param name="storeConfiguration">The Bringg login details to test</param>
        /// <returns></returns>
        public ResultEnum ValidateCredentials(Model.StoreConfiguration storeConfiguration)
        {
            ResultEnum result = ResultEnum.UnknownError;

            ErrorHelper.LogInfo("BringgGPSIntegrationServices.ValidateCredentials", "", storeConfiguration.AndromedaStoreId.ToString());

            // Get the bringg config
            GPSConfig bringgConfig = this.GetBringgConfig(storeConfiguration.PartnerConfiguration);

            // Call the Bringg API - try and get the company
            string errorMessage = bringgConfig.partnerConfig.apiCallsEnabled ? BringgAPI.ValidateCredentials(bringgConfig.partnerConfig) : null;

            if (!String.IsNullOrEmpty(errorMessage))
            {
                ErrorHelper.LogError("BringgGPSIntegrationServices.ValidateCredentials", errorMessage, storeConfiguration.AndromedaStoreId.ToString());
                result = ResultEnum.UnknownError;
            }
            else
            {
                result = ResultEnum.OK;
            }

            return result;
        }

        /// <summary>
        /// Sends an order to Bringg
        /// </summary>
        /// <param name="andromedaStoreId">The Andromeda store id at which the order was placed</param>
        /// <param name="customer">Details of the customer that placed the order</param>
        /// <param name="newOrder">The order that was placed</param>
        /// <returns></returns>
        public ResultEnum CustomerPlacedOrder(int andromedaStoreId, Model.Customer customer, Model.Order newOrder, Action<string, DebugLevel> log)
        {
            ResultEnum result = ResultEnum.UnknownError;

            ErrorHelper.LogInfo("BringgGPSIntegrationServices.CustomerPlacedOrder", "New order " + newOrder.AndromedaOrderId, andromedaStoreId.ToString());

            // Lookup the Bringg config
            Model.StoreConfiguration store = null;
            result = this.GetStoreByAndromedaStoreId(andromedaStoreId, out store);

            // Get the bringg config
            GPSConfig bringgConfig = null;
            if (result == ResultEnum.OK)
            {
                bringgConfig = this.GetBringgConfig(store.PartnerConfiguration);
            }

            if (!bringgConfig.isEnabled)
            {
                ErrorHelper.LogError("BringgGPSIntegrationServices.CustomerPlacedOrder", "Attempt to send order to disabled Bringg.  Store " + andromedaStoreId.ToString(), andromedaStoreId.ToString());
                return ResultEnum.Disabled;
            }

            // Add/update customer
            if (result == ResultEnum.OK)
            {
                result = this.AddUpdateCustomer(andromedaStoreId, bringgConfig, customer);
            }

            if (result == ResultEnum.OK)
            {
                // Send order to Bringg
                string errorMessage =
                    bringgConfig.partnerConfig.apiCallsEnabled ? BringgAPI.AddTask(bringgConfig.partnerConfig, customer, newOrder, log) : null;

                if (!String.IsNullOrEmpty(errorMessage))
                {
                    ErrorHelper.LogError("BringgGPSIntegrationServices.CustomerPlacedOrder", errorMessage, andromedaStoreId.ToString());
                    result = ResultEnum.UnknownError;
                }
            }

            ErrorHelper.LogInfo(
                "BringgGPSIntegrationServices.CustomerPlacedOrder", 
                "Completed with task id " + ((newOrder.BringgTaskId == null) ? "" : newOrder.BringgTaskId ), 
                andromedaStoreId.ToString()); 

            return result;
        }

        /// <summary>
        /// Assigns a driver to an existing order in Bringg
        /// </summary>
        /// <param name="andromedaStoreId">The Andromeda store id at which the order was placed</param>
        /// <param name="externalOrderId">The order id returned by Bringg when the order was originally created</param>
        /// <param name="driver">The driver that should be assigned to the order</param>
        /// <returns></returns>
        public ResultEnum AssignDriverToOrder(int andromedaStoreId, string bringgTaskId, int? bags, Model.Driver driver, Action<string, DebugLevel> log)
        {
            ResultEnum result = ResultEnum.UnknownError;

            ErrorHelper.LogInfo("BringgGPSIntegrationServices.AssignDriverToOrder", "", andromedaStoreId.ToString());

            // Lookup the Bringg config
            Model.StoreConfiguration store = null;
            result = this.GetStoreByAndromedaStoreId(andromedaStoreId, out store);

            // Get the bringg config
            GPSConfig bringgConfig = null;
            if (result == ResultEnum.OK)
            {
                bringgConfig = this.GetBringgConfig(store.PartnerConfiguration);
            }

            if (!bringgConfig.isEnabled)
            {
                ErrorHelper.LogError("BringgGPSIntegrationServices.AssignDriverToOrder", "Attempt to send order to disabled Bringg.  Store " + andromedaStoreId.ToString(), andromedaStoreId.ToString());
                return ResultEnum.Disabled;
            }

            // Get or add the driver in Bringg
            if (result == ResultEnum.OK)
            {
                string errorMessage =
                    bringgConfig.partnerConfig.apiCallsEnabled ? BringgAPI.GetOrAddUser(bringgConfig.partnerConfig, driver, log) : null;

                if (!String.IsNullOrEmpty(errorMessage))
                {
                    log("BringgAPI.GetOrAddUser" + errorMessage, DebugLevel.Error);
                    ErrorHelper.LogError("BringgGPSIntegrationServices.AssignDriverToOrder", errorMessage, andromedaStoreId.ToString());
                    result = ResultEnum.UnknownError;
                }
            }

            // Get the Bringg task
            BringgTask bringgTask = null;
            BringgTaskDetailModel detailTask = null;
            if (result == ResultEnum.OK)
            {
                string errorMessage =
                    bringgConfig.partnerConfig.apiCallsEnabled 
                    ? BringgAPI.GetTask(bringgConfig.partnerConfig, bringgTaskId, out bringgTask, out detailTask) 
                    : null;

                if (!String.IsNullOrEmpty(errorMessage))
                {
                    log("BringgAPI.GetTask" + errorMessage, DebugLevel.Error);
                    ErrorHelper.LogError("BringgGPSIntegrationServices.AssignDriverToOrder", errorMessage, andromedaStoreId.ToString());
                    result = ResultEnum.UnknownError;
                }
            }

            // Assign the driver to the task
            if (result == ResultEnum.OK)
            {
                string message = string.Format("Assign driver: {0} to task: {1}", driver.ExternalId, bringgTask.id);
                log(message, DebugLevel.Notify);
                // Assign the driver to the task
                bringgTask.user_id = driver.ExternalId;

                //assign bags
                if (bags.HasValue) 
                { 
                    bringgTask.note = "Bags: " + bags.Value;
                }

                // Update the Bringg task
                string errorMessage =
                    bringgConfig.partnerConfig.apiCallsEnabled 
                    ? BringgAPI.UpdateTask(bringgConfig.partnerConfig, bringgTaskId, bringgTask, log) 
                    : null;

                if (!String.IsNullOrEmpty(errorMessage))
                {
                    ErrorHelper.LogError("BringgGPSIntegrationServices.AssignDriverToOrder", errorMessage, andromedaStoreId.ToString());
                    result = ResultEnum.UnknownError;
                }
            }

            // Start the Bringg task
            if (result == ResultEnum.OK)
            {
                string errorMessage =
                    bringgConfig.partnerConfig.apiCallsEnabled ? BringgAPI.StartTask(bringgConfig.partnerConfig, bringgTaskId) : null;

                if (!String.IsNullOrEmpty(errorMessage))
                {
                    ErrorHelper.LogError("BringgGPSIntegrationServices.AssignDriverToOrder", errorMessage, andromedaStoreId.ToString());
                    result = ResultEnum.UnknownError;
                }
            }

            // Bring the driver on shift
            //if (result == ResultEnum.OK)
            //{
            //    string errorMessage =
            //        bringgConfig.partnerConfig.apiCallsEnabled ? BringgAPI.ClockDriverIn(bringgConfig.partnerConfig, driver.ExternalId) : null;

            //    if (!String.IsNullOrEmpty(errorMessage))
            //    {
            //        ErrorHelper.LogError("BringgGPSIntegrationServices.AssignDriverToOrder", errorMessage, andromedaStoreId.ToString());
            //        result = ResultEnum.UnknownError;
            //    }
            //}

            return result;
        }

        private GPSConfig GetBringgConfig(string configJson)
        {
            return JsonConvert.DeserializeObject<GPSConfig>(configJson);
        }

        private ResultEnum AddUpdateCustomer(int andromedaStoreId, GPSConfig bringgConfig, Model.Customer customer)
        {
            ResultEnum result = ResultEnum.OK;

    //        DW.EntityFrameworkDataAccessFactory entityFrameworkDataAccessFactory = new DW.EntityFrameworkDataAccessFactory();

    //        CustomerGPS customerGPS = null;
    //        if (result == ResultEnum.OK)
   //         {
                // Get the Bringg customer details from the database
   //             entityFrameworkDataAccessFactory.CustomerGPSDataAccess.GetById(customer.Id, out customerGPS);
   //         }

    //        if (result == ResultEnum.OK)
   //         {
                //if (customerGPS == null)
                //{
                    // Customer has not already been created in Bringg
     //               ErrorHelper.LogInfo("BringgGPSIntegrationServices.AddUpdateCustomer", "Customer NOT in Bringg", andromedaStoreId.ToString());

                    // Create customer in Bringg 
                    // Note that the customer object returned by AddCustomer will contain the Bringg customer id (partnerId)
                    string errorMessage =
                        bringgConfig.partnerConfig.apiCallsEnabled ? BringgAPI.AddCustomer(bringgConfig.partnerConfig, customer) : null;

                    if (!String.IsNullOrEmpty(errorMessage))
                    {
                        ErrorHelper.LogError("BringgGPSIntegrationServices.AddUpdateCustomer", errorMessage, andromedaStoreId.ToString());
                        result = ResultEnum.UnknownError;
                    }
          //          else
         //           {
                        // Update the customer in the datawarehouse
         //               customerGPS = new CustomerGPS()
         //               {
         //                   CustomerId = customer.Id,
         //                   PartnerId = customer.PartnerId
         //               };
     //                   entityFrameworkDataAccessFactory.CustomerGPSDataAccess.Add(customerGPS);
         //           }
                //}
                //else
                //{
                //    // We've already got the Bringg customer id
                //    ErrorHelper.LogInfo("BringgGPSIntegrationServices.AddUpdateCustomer", "Customer " + customerGPS.PartnerId + " already in Bringg", andromedaStoreId.ToString());

                //    customer.PartnerId = customerGPS.PartnerId;

                //    // Update the customer in Bringg
                //    string errorMessage =
                //        bringgConfig.partnerConfig.apiCallsEnabled ? BringgAPI.UpdateCustomer(bringgConfig.partnerConfig, customer) : null;

                //    if (!String.IsNullOrEmpty(errorMessage))
                //    {
                //        ErrorHelper.LogError("BringgGPSIntegrationServices.AddUpdateCustomer", errorMessage, andromedaStoreId.ToString());
                //        result = ResultEnum.UnknownError;
                //    }
                //}
     //       }

     //       if (result == ResultEnum.OK)
     //       {
                // We've either created or gotten the customer - we need the Bringg customer id to tag the order against
     //           customer.PartnerId = customerGPS.PartnerId;
     //       }

            return result;
        }

        public ResultEnum CancelOrder(int andromedaStoreId, string bringgTaskId)
        {
            ResultEnum result = ResultEnum.UnknownError;

            ErrorHelper.LogInfo("BringgGPSIntegrationServices.CancelOrder", "", bringgTaskId);

            // Lookup the Bringg config
            Model.StoreConfiguration store = null;
            result = this.GetStoreByAndromedaStoreId(andromedaStoreId, out store);

            // Get the bringg config
            GPSConfig bringgConfig = null;
            if (result == ResultEnum.OK)
            {
                bringgConfig = this.GetBringgConfig(store.PartnerConfiguration);
            }

            if (!bringgConfig.isEnabled)
            {
                ErrorHelper.LogError("BringgGPSIntegrationServices.CancelOrder", "Attempt to cancel order in disabled Bringg.  Store " + andromedaStoreId.ToString(), andromedaStoreId.ToString());
                return ResultEnum.Disabled;
            }

            // Cancel the Bringg task
            if (result == ResultEnum.OK)
            {
                string errorMessage =
                    bringgConfig.partnerConfig.apiCallsEnabled ? BringgAPI.CancelTask(bringgConfig.partnerConfig, bringgTaskId) : null;

                if (!String.IsNullOrEmpty(errorMessage))
                {
                    ErrorHelper.LogError("BringgGPSIntegrationServices.CancelOrder", errorMessage, andromedaStoreId.ToString());
                    result = ResultEnum.UnknownError;
                }
            }

            return result;
        }
    }
}
