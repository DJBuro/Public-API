using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using DataWarehouseDataAccess;
using DataWarehouseDataAccess.Domain;
using AndroCloudHelper;

namespace AndroCloudServicesTests
{
    public class DataWarehouseHelper
    {
        public static Mock<IDataAccessFactory> GetMockDataAccessFactory()
        {
            // Mock up the data access
            Mock<IDataAccessFactory> mock = new Mock<IDataAccessFactory>();

            return mock;
        }

        public static void MockCustomerDataAccess
        (
            Mock<IDataAccessFactory> mock,
            bool usernameAlreadyUsed
        )
        {
            string errorMessage = "";

            if (usernameAlreadyUsed) errorMessage = "Username already used: xxxx";

            mock.Setup
            (
                m => m.CustomerDataAccess.AddCustomer
                (
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Customer>()
                )
            ).Returns
            (
                errorMessage
            );
        }
    }
}
