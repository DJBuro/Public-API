using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AndroAdminDataAccess.DataAccess;
using AndroAdminDataAccess.Domain;
using AndroAdmin.Controllers;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AndroAdminTests
{
    [TestClass]
    public class AMSServerControllerTests
    {
        [TestMethod]
        public void IndexTest()
        {
            // Create a mock AMS server DAO
            Mock<IAMSServerDAO> mock = new Mock<IAMSServerDAO>();

            // Mock the GetAll method
            mock.Setup(m => m.GetAll()).Returns
                (
                    new List<AMSServer>() 
                    {
                        new AMSServer() { Id=0, Name="AMS Server 1", Description="An AMS Server" },
                        new AMSServer() { Id=1, Name="AMS Server 2", Description="Another AMS Server" }
                    }
                );

            // Create a controller giving it our mock DAO so we can feed in test data
            AMSServerController amsServerController = new AMSServerController();
            amsServerController.AMSServerDAO = mock.Object;

            // Call the controller
            IEnumerable<AMSServer> amsServers = (IEnumerable<AMSServer>)amsServerController.Index();

            // Tests
            AMSServer[] amsServersArray = amsServers.ToArray<AMSServer>();
            Assert.AreEqual(2, amsServersArray.Length);

            this.CheckAMSServer(amsServersArray[0], 0, 900, "AMS Server 1", "An AMS Server");
            this.CheckAMSServer(amsServersArray[1], 1, 999, "AMS Server 2", "Another AMS Server");
        }

        [TestMethod]
        public void AddTest_BlankName()
        {
            // Create a mock AMS server DAO
            Mock<IAMSServerDAO> mock = new Mock<IAMSServerDAO>();

            // Mock the Add method
            mock.Setup(m => m.Add(null));

            // Create a controller giving it our mock DAO so we can feed in test data
            AMSServerController amsServerController = new AMSServerController();
            amsServerController.AMSServerDAO = mock.Object;

            // Create a test object
            AMSServer amsServer = new AMSServer() { Id=0, Name="", Description="test"};

            // Manually trigger model validation
            TestHelper.ValidateModel(amsServer, amsServerController);

            // Call the controller
            amsServerController.Add(amsServer);

            // Check for the validation error
            string errorMessage = TestHelper.CheckForModelError((Controller)amsServerController, "Name", "Please enter a name");

            if (errorMessage.Length != 0)
            {
                Assert.Fail(errorMessage);
            }
        }

        [TestMethod]
        public void AddTest_NameAlreadyUsed()
        {
            // Create a mock AMS server DAO
            Mock<IAMSServerDAO> mock = new Mock<IAMSServerDAO>();

            // Mock the GetByName method
            mock.Setup(m => m.GetByName("")).Returns
                (
                        new AMSServer() { Id=0, Name="AMS Server 1", Description="An AMS Server" }
                );

            // Create a controller giving it our mock DAO so we can feed in test data
            AMSServerController amsServerController = new AMSServerController();
            amsServerController.AMSServerDAO = mock.Object;

            // Create a test object
            AMSServer amsServer = new AMSServer() { Id = 0, Name = "", Description = "test" };

            // Call the controller
            amsServerController.Add(amsServer);

            // Check for the validation error
            string errorMessage = TestHelper.CheckForModelError((Controller)amsServerController, "Name", "Name already used");

            if (errorMessage.Length != 0)
            {
                Assert.Fail(errorMessage);
            }
        }

        private void CheckAMSServer(AMSServer amsServer, int id, int amsServerId, string name, string description)
        {
            Assert.AreEqual(amsServer.Id, id);
            Assert.AreEqual(amsServer.Name, name);
            Assert.AreEqual(amsServer.Description, description);
        }
    }
}
