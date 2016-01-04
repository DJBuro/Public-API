using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using AndroCloudDataAccess;
using AndroCloudDataAccess.Domain;
using AndroCloudHelper;

namespace AndroCloudPrivateServicesTests
{
    public class TestHelper
    {
        public static Mock<IDataAccessFactory> GetMockDataAccessFactory()
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

        //public static void MockOrdersDataAccess(
        //    Mock<IDataAccessFactory> mock,
        //    bool isValidOrderId,
        //    bool isAllowedToAccessSite,
        //    string externalOrderId,
        //    string status)
        //{
        //    // Mock the Orders data access
        //    Order order = null;

        //    if (isValidOrderId)
        //    {
        //        order = new Order();
        //        order.ID = Guid.NewGuid();
        //        order.StoreOrderId = externalOrderId;
        //        order.InternetOrderNumber = 123;
        //        order.RamesesStatusId = int.Parse(status);
        //    }

        //    Order allowedOrder = isAllowedToAccessSite ? order : null;

        //    mock.Setup
        //    (
        //        m => m.OrderDataAccess.GetByInternetOrderNumberAndSiteId
        //        (
        //            It.IsAny<int>(),
        //            It.IsAny<Guid>(),
        //            out order
        //        )
        //    ).Returns
        //    (
        //        ""
        //    );

        //    mock.Setup
        //    (
        //        m => m.OrderDataAccess.GetByExternalOrderNumber
        //        (
        //            It.IsAny<string>(),
        //            out order
        //        )
        //    ).Returns
        //    (
        //        ""
        //    );

        //    mock.Setup
        //    (
        //        m => m.OrderDataAccess.GetByApplicationIdOrderId
        //        (
        //            It.IsAny<int>(),
        //            It.IsAny<Guid>(),
        //            out allowedOrder
        //        )
        //    ).Returns
        //    (
        //        ""
        //    );

        //    mock.Setup
        //    (
        //        m => m.OrderDataAccess.GetById
        //        (
        //            It.IsAny<Guid>(),
        //            out allowedOrder
        //        )
        //    ).Returns
        //    (
        //        ""
        //    );
        //}

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

        public static void MockHostDataAccess(
            Mock<IDataAccessFactory> mock)
        {
            // Mock the order status data access
            PrivateHost host = null;
            host = new PrivateHost();
            host.Order = 1;
            host.Url = "thisisatesturl";
            host.Id = Guid.NewGuid();

            List<PrivateHost> hosts = new List<PrivateHost>();
            hosts.Add(host);

            mock.Setup
            (
                m => m.HostDataAccess.GetAllGenericPrivateHosts
                (
                    out hosts
                )
            ).Returns
            (
                ""
            );
        }
    }
}
