using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Andromeda.GPSIntegration;
using Andromeda.GPSIntegration.Model;
using Andromeda.GPSIntegration.Bringg;
using System.Collections.Generic;
using AndroAdminDataAccess.Domain;
using AndroAdminDataAccess.EntityFramework.DataAccess;
using AndroAdminDataAccess.EntityFramework;

namespace GPSIntegrationUnitTests
{
    [TestClass]
    public class Bringg
    {
        public static int andromedaStoreId = 877;
        public static IGPSIntegrationServices gpsIntegrationServices = new BringgGPSIntegrationServices();

        [TestInitialize]
        public void InitializeTest()
        {
            // DON'T DO THIS NOW THAT OTHER PEOPLE ARE USING IT

            // Delete all drivers and GPS stores
            //using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            //{
            //    entitiesContext.Database.ExecuteSqlCommand("delete from [StoreDrivers]");
            //    entitiesContext.Database.ExecuteSqlCommand("delete from [StoreGPSSettings]");
            //}
        }

        [TestMethod]
        public void UpdateStore()
        {
            this.UpdateStoreInternal();
        }

        private Andromeda.GPSIntegration.Model.StoreConfiguration UpdateStoreInternal()
        {
            // Check that there are no settings
  //          Andromeda.GPSIntegration.Model.StoreConfiguration checkStore = null;
            //          ResultEnum result = Bringg.gpsIntegrationServices.GetStoreByAndromedaStoreId(Bringg.andromedaStoreId, out checkStore);

  //          Assert.AreEqual(ResultEnum.NoStoreSettings, result);

            ResultEnum result = ResultEnum.OK;

            // Enable bringg
            Andromeda.GPSIntegration.Model.StoreConfiguration enableStore = new Andromeda.GPSIntegration.Model.StoreConfiguration()
            {
                AndromedaStoreId = Bringg.andromedaStoreId,
                MaxDrivers = 5,
                PartnerConfiguration =
                    "{" +
                        "\"partnerName\":\"Bringg\", " +
                        "\"isEnabled\":true," +
                        "\"partnerConfig\":" +
                        "{" +
                            "\"companyId\":" + Bringg.andromedaStoreId + ", " +
                            "\"apiUrl\":\"http://developer-api.bringg.com/partner_api\", " +
                            "\"accessToken\":\"yoHdcByr7iGsA6BnbNZU\", " +
                            "\"secretKey\":\"Wdkj5Mmm-ctcsGQ7pjqn\", " +
                            "\"apiCallsEnabled\":true, " +
                            "\"testMode\":true," +
                            "\"clockInAPIUrl\":\"https://admin-api.bringg.com/services/94kfjr8rj/41b00a6b-2eb6-4bf9-9fe3-43ec43e8943a/2b89c70c-3ce6-4929-969d-89ee85fe0b89/\"" +
                        "}" +
                    "}"
            };

            //     result = Bringg.gpsIntegrationServices.UpdateStore(enableStore);

            //      Assert.AreEqual(ResultEnum.OK, result);

            // Check that the configuration was applied correctly
            //     this.ValidateStoreSettings(enableStore);

            // Modify the store configuration
            //Andromeda.GPSIntegration.Model.StoreConfiguration modifyBringgStore = new Andromeda.GPSIntegration.Model.StoreConfiguration()
            //{
            //    AndromedaStoreId = Bringg.andromedaStoreId,
            //    MaxDrivers = 10,
            //    PartnerConfiguration = enableStore.PartnerConfiguration
            //};
            //result = Bringg.gpsIntegrationServices.UpdateStore(modifyBringgStore);

            //Assert.AreEqual(ResultEnum.OK, result);

            //// Check that the configuration was applied correctly
            //this.ValidateStoreSettings(modifyBringgStore);

            return enableStore;
        }

        [TestMethod]
        public void PlaceOrder()
        {
            this.PlaceOrderInternal();
        }

        private string PlaceOrderInternal()
        {
            // We need to make sure that there is a store first
            this.UpdateStore();

            Customer customer = new Customer()
            {
                Email = "",
                Id = Guid.Parse("35459B3C-ACC5-4E71-B37F-0011A2DE9734"),
                Lat = null,
                Lng = null,
                //Name = "Test customer " + Guid.NewGuid().ToString(),
                //Phone = "+447971444444"
                Name = "Bobbob",
                Phone = "+447775724172"
            };

            Order newOrder = new Order()
            {
                Address = new Andromeda.GPSIntegration.Model.Address()
                {
                    Country = "UK",
                    County = "Surrey",
                //    Directions = "Directions",
                    Lat = null,
                    Locality = "Locality",
                    Long = null,
                    Org1 = "Org1",
                    Org2 = "Org2",
                    Org3 = "Org4",
                    Postcode = "SM6 0DA",
                    Prem1 = "Prem1",
                    Prem2 = "Prem2",
                    Prem3 = "Prem3",
                    Prem4 = "Prem4",
                    Prem5 = "Prem5",
                    Prem6 = "Prem6",
                    RoadName = "The High Street",
                    RoadNum = "45",
                    State = "State",
                    Town = "Wallington"
                },
                BringgTaskId = "",
                Note = "Give me my food",
                ScheduledAt = DateTime.UtcNow,
                TotalPrice = 10.50m,
                DeliveryFee = 0m,
                AndromedaOrderId = "666",
                HasBeenPaid = true // CASH OR CARD!!!
            };

            Action<string, DebugLevel> action = this.Dummy;
 
            ResultEnum result = Bringg.gpsIntegrationServices.CustomerPlacedOrder
                (
                    Bringg.andromedaStoreId, 
                    customer, 
                    newOrder,
                    action
                ); // ANDRO test store debugging only

            Assert.AreEqual(ResultEnum.OK, result);

            return newOrder.BringgTaskId;
        }

        private void Dummy(string x, DebugLevel y)
        {

        }

        [TestMethod]
        public void ValidateCredentials()
        {
            Andromeda.GPSIntegration.Model.StoreConfiguration storeConfiguration = new Andromeda.GPSIntegration.Model.StoreConfiguration()
            {
                AndromedaStoreId = Bringg.andromedaStoreId,
                MaxDrivers = 5,
                PartnerConfiguration =
                    "{" +
                        "\"partnerName\":\"Bringg\", " +
                        "\"isEnabled\":true," +
                        "\"partnerConfig\":" +
                        "{" +
                            "\"companyId\":8758, " +
                            "\"apiUrl\":\"http://developer-api.bringg.com/partner_api\", " +
                            "\"accessToken\":\"yoHdcByr7iGsA6BnbNZU\", " +
                            "\"secretKey\":\"Wdkj5Mmm-ctcsGQ7pjqn\", " +
                            "\"apiCallsEnabled\":true, " +
                            "\"testMode\":false" +
                        "}" +
                    "}"
            };

            // Get the new driver so we delete it
            ResultEnum result = Bringg.gpsIntegrationServices.ValidateCredentials(storeConfiguration);
            Assert.AreEqual(ResultEnum.OK, result);
        }

        private void ValidateStoreSettings(Andromeda.GPSIntegration.Model.StoreConfiguration expectedStoreSettings)
        {
            Andromeda.GPSIntegration.Model.StoreConfiguration actualStoreSettings = null;
            ResultEnum result = Bringg.gpsIntegrationServices.GetStoreByAndromedaStoreId(expectedStoreSettings.AndromedaStoreId, out actualStoreSettings);

            Assert.AreEqual(ResultEnum.OK, result);

            //        Assert.AreEqual(expectedStoreSettings.PartnerStoreId, actualStoreSettings.PartnerStoreId);
            //        Assert.AreEqual(expectedStoreSettings.IsPartnerIntegrationEnabled, actualStoreSettings.IsPartnerIntegrationEnabled);
            Assert.AreEqual(expectedStoreSettings.MaxDrivers, actualStoreSettings.MaxDrivers);
        }

        [TestMethod]
        public void AssignOrderToDriver()
        {
            string externalId = this.PlaceOrderInternal();

            Driver driver = new Driver()
            {                
                //Name = "Driver " + Guid.NewGuid().ToString(),
               // Phone = "+447971444454"
                Name = "Shaun Brazendale",
                Phone = "+447775724172"
            };

            Action<string, DebugLevel> action = this.Dummy;
            ResultEnum result = Bringg.gpsIntegrationServices.AssignDriverToOrder(Bringg.andromedaStoreId, externalId, 4, driver, action); // ANDRO test store debugging only

            Assert.AreEqual(ResultEnum.OK, result);
        }

        [TestMethod]
        public void CancelOrder()
        {
            string externalId = this.PlaceOrderInternal();

            ResultEnum result = Bringg.gpsIntegrationServices.CancelOrder(Bringg.andromedaStoreId, externalId); // ANDRO test store debugging only

            Assert.AreEqual(ResultEnum.OK, result);
        }

        [TestMethod]
        public void UpdateOrder()
        {
            // We need to make sure that there is a store first
            this.UpdateStore();

            string taskId = this.PlaceOrderInternal();

            Customer customer = new Customer()
            {
                Email = "",
                Id = Guid.Parse("35459B3C-ACC5-4E71-B37F-0011A2DE9734"),
                Lat = null,
                Lng = null,
                //Name = "Test customer " + Guid.NewGuid().ToString(),
                //Phone = "+447971444444"
                Name = "Ich heiße Bob",
                Phone = "+447775724172"
            };

            Order newOrder = new Order()
            {
                Address = new Andromeda.GPSIntegration.Model.Address()
                {
                    Country = "UK",
                    County = "Surrey",
                 //   Directions = "New Directions",
                    Lat = null,
                    Locality = "New Locality",
                    Long = null,
                    Org1 = "New Org1",
                    Org2 = "New Org2",
                    Org3 = "New Org4",
                    Postcode = "SM6 0DE",
                    Prem1 = "New Prem1",
                    Prem2 = "New Prem2",
                    Prem3 = "New Prem3",
                    Prem4 = "New Prem4",
                    Prem5 = "New Prem5",
                    Prem6 = "New Prem6",
                    RoadName = "The High Street",
                    RoadNum = "45",
                    State = "State",
                    Town = "Wallington"
                },
                BringgTaskId = taskId,
                Note = "For crying out loud, where's my food???",
                ScheduledAt = DateTime.UtcNow.AddHours(2),
                TotalPrice = 10.50m,
                DeliveryFee = 0m,
                AndromedaOrderId = "12345",
                HasBeenPaid = true // CASH OR CARD!!!
            };

            Action<string, DebugLevel> action = this.Dummy;
            ResultEnum result = Bringg.gpsIntegrationServices.CustomerPlacedOrder(Bringg.andromedaStoreId, customer, newOrder, action); // ANDRO test store debugging only
            Assert.AreEqual(ResultEnum.OK, result);
        }
    }
}
