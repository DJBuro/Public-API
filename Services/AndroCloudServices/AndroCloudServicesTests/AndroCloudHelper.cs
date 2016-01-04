using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using AndroCloudDataAccess;
using AndroCloudDataAccess.Domain;
using AndroCloudHelper;

namespace AndroCloudServicesTests
{
    public class AndroCloudHelper
    {
        private const ACSApplication defaultACSApplication = null;

        public static Mock<IDataAccessFactory> GetMockDataAccessFactory(ACSApplication acsApplication = defaultACSApplication)
        {
            // Mock up the data access
            Mock<IDataAccessFactory> mock = new Mock<IDataAccessFactory>();

            // Mock the Audit data access
            mock.Setup
            (
                m => m.AuditDataAccess.Add
                (
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()
                )
            ).Returns
            (
                ""
            );

            // Mock the ACSApplication data access
            mock.Setup
            (
                m => m.AcsApplicationDataAccess.GetByExternalId
                (
                    It.IsAny<string>(),
                    out acsApplication
                )
            ).Returns
            (
                ""
            );

            return mock;
        }

        public static void MockSitesDataAccess(
            Mock<IDataAccessFactory> mock, 
            bool isValidSiteId, 
            bool isAllowedToAccessSite,
            string licenseKey,
            List<Site> sites)
        {
            // Mock the Sites data access
            Site site = new Site();
            site.EstDelivTime = 30;
            site.IsOpen = true;
            site.MenuVersion = 43;
            site.Name = "test site";
            site.LicenceKey = licenseKey;
            site.ExternalId = "15a8ac10-0ac0-444f-9b47-7db2cc342d10";

            Site getByExternalSiteId = isValidSiteId ? site : null;

            mock.Setup
            (
                m => m.SiteDataAccess.GetByExternalSiteId
                (
                    It.IsAny<string>(),
                    out getByExternalSiteId
                )
            ).Returns
            (
                ""
            );

            Site getByIdAndPartner = isAllowedToAccessSite ? site : null;

            mock.Setup
            (
                m => m.SiteDataAccess.GetByIdAndApplication
                (
                    It.IsAny<int>(),
                    It.IsAny<Guid>(),
                    out getByIdAndPartner
                )
            ).Returns
            (
                ""
            );

            if (sites == null)
            {
                sites = new List<Site>();
                sites.Add(site);
            }

            mock.Setup
            (
                m => m.SiteDataAccess.GetByFilter
                (
                    It.IsAny<int>(),
                    It.IsAny<float?>(),
                    It.IsAny<float?>(),
                    It.IsAny<float?>(),
                    It.IsAny<string>(),
                    It.IsAny<DataTypeEnum>(),
                    out sites
                )
            ).Returns
            (
                ""
            );
        }

        public static void MockSiteMenusDataAccess(Mock<IDataAccessFactory> mock, bool hasMenu)
        {
            // Mock the Sites data access
            SiteMenu siteMenu = null;
            if (hasMenu)
            {
                siteMenu = new SiteMenu();
                siteMenu.MenuData = "test menu data";
                siteMenu.MenuType = "XML";
                siteMenu.SiteID = Guid.NewGuid();
                siteMenu.Version = 5;
            }

            mock.Setup
            (
                m => m.SiteMenuDataAccess.GetBySiteId
                (
                    It.IsAny<Guid>(),
                    It.IsAny<DataTypeEnum>(),
                    out siteMenu
                )
            ).Returns
            (
                ""
            );

            mock.Setup
            (
                m => m.SiteMenuDataAccess.Put
                (
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<DataTypeEnum>()
                )
            ).Returns
            (
                ""
            );
        }

        public static void MockSiteDetailsDataAccess(
            Mock<IDataAccessFactory> mock)
        {
            // Mock the SiteDetails data access
            Address address = new Address();
            address.Country = "UK";
            address.County = "Surrey";
            address.Dps = "zzz";
            address.Lat = "123.456";
            address.Locality = "test locality";
            address.Long = "654.321";
            address.Org1 = "test org1";
            address.Org2 = "test org2";
            address.Org3 = "test org3";
            address.Postcode = "test postcode";
            address.Prem1 = "test prem1";
            address.Prem2 = "test prem2";
            address.Prem3 = "test prem3";
            address.Prem4 = "test prem4";
            address.Prem5 = "test prem5";
            address.Prem6 = "test prem6";
            address.RoadName = "test roadname";
            address.RoadNum = "test roadnum";
            address.Town = "test town";

            SiteDetails siteDetails = new SiteDetails();
            siteDetails.Address = address;
            siteDetails.EstDelivTime = 30;
            siteDetails.ExternalId = "15a8ac10-0ac0-444f-9b47-7db2cc342d10";
            siteDetails.IsOpen = true;
            siteDetails.LicenceKey = "12345";
            siteDetails.MenuVersion = 95;
            siteDetails.Name = "Test Site";
            siteDetails.OpeningHours = null;
            siteDetails.Phone = "0123456789";
            siteDetails.TimeZone = "UTC+1";

            mock.Setup
            (
                m => m.SiteDetailsDataAccess.GetBySiteId
                (
                    It.IsAny<Guid>(),
                    It.IsAny<DataTypeEnum>(),
                    out siteDetails
                )
            ).Returns
            (
                ""
            );
        }

        public static void MockOrdersDataAccess(
            Mock<IDataAccessFactory> mock,
            bool isValidOrderId,
            bool isAllowedToAccessSite,
            string externalOrderId,
            string status)
        {
            // Mock the Orders data access
            Order order = null;

            if (isValidOrderId)
            {
                order = new Order();
                order.ID = Guid.NewGuid();
                order.StoreOrderId = externalOrderId;
                order.InternetOrderNumber = 123;
                order.RamesesStatusId = int.Parse(status);
            }

            Order allowedOrder = isAllowedToAccessSite ? order : null;

            //mock.Setup
            //(
            //    m => m.OrderDataAccess.GetByInternetOrderNumberAndSiteId
            //    (
            //        It.IsAny<int>(),
            //        It.IsAny<Guid>(),
            //        out order
            //    )
            //).Returns
            //(
            //    ""
            //);

            //mock.Setup
            //(
            //    m => m.OrderDataAccess.GetByExternalOrderNumber
            //    (
            //        It.IsAny<string>(),
            //        out order
            //    )
            //).Returns
            //(
            //    ""
            //);

            //mock.Setup
            //(
            //    m => m.OrderDataAccess.GetByApplicationIdOrderId
            //    (
            //        It.IsAny<int>(),
            //        It.IsAny<Guid>(),
            //        out allowedOrder
            //    )
            //).Returns
            //(
            //    ""
            //);

            //mock.Setup
            //(
            //    m => m.OrderDataAccess.GetById
            //    (
            //        It.IsAny<Guid>(),
            //        out allowedOrder
            //    )
            //).Returns
            //(
            //    ""
            //);
        }

        //public static void MockOrderStatusDataAccess(
        //    Mock<IDataAccessFactory> mock,
        //    bool isValidRamesesOrderId,
        //    string ramesesStatusId)
        //{
        //    // Mock the order status data access
        //    OrderStatus orderStatus = null;

        //    if (isValidRamesesOrderId)
        //    {
        //        orderStatus = new OrderStatus();
        //        orderStatus.Description = "test description";
        //        orderStatus.RamesesStatusId = int.Parse(ramesesStatusId);
        //        orderStatus.Id = Guid.NewGuid();
        //    }
 
        //    mock.Setup
        //    (
        //        m => m.OrderStatusDataAccess.GetByRamesesStatusId
        //        (
        //            It.IsAny<int>(),
        //            out orderStatus
        //        )
        //    ).Returns
        //    (
        //        ""
        //    );
        //}
    }
}
