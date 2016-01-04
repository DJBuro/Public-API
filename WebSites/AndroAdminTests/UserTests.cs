using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AndroAdmin.Controllers;
using AndroAdmin.Model;
using AndroUsersDataAccess.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AndroAdminTests
{
    [TestClass]
    public class UserTests
    {
        [TestInitialize]
        public void Startup()
        {
            DatabaseHelper.DeleteTestDatabases();
        }

        [TestMethod]
        public void UsersTestMethod()
        {
            // Create a test database
            string androUsersConnectionString = DatabaseHelper.CreateTestAndroUsersDatabase(
                DatabaseHelper.CreateAndroUsersDatabaseScriptFilename, 
                DatabaseHelper.CreateAndroUsersDataScriptFilename);

            AdminController adminController = new AdminController();
            adminController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Test the empty database
            this.NoUsers(adminController);

            // Test adding a user
            this.AddFirstUser(adminController);
        }

        private void NoUsers(AdminController adminController)
        {
            // Get the users
            object result = (ViewResult)adminController.Index();

            // Check the result
            Assert.IsTrue(result is ViewResult);
            ViewResult viewResult = (ViewResult)result;

            // Shouldn't be any users yet
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<UserListModel>, "Expected List<UserListModel> got " + viewResult.Model.GetType().ToString());

            List<UserListModel> actualAndroUsers = (List<UserListModel>)viewResult.Model;
            Assert.AreEqual(0, actualAndroUsers.Count);
        }

        private void AddFirstUser(AdminController adminController)
        {
            AndroUser newAndroUser = new AndroUser()
            {
                Active = true,
                Created = DateTime.Now,
                EmailAddress = "a@b.com",
                FirstName = "Firstname",
                Password = "testPass",
                SurName = "SurName"
            };

            // Add a user
            object result = adminController.Add(newAndroUser);

            // Check the result
            Assert.IsTrue(result is RedirectToRouteResult);
            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult)result;

            // We should be redirected back to the index page
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("Admin", redirectToRouteResult.RouteValues["controller"]);

            // To check that the user was added by getting a list of users
            ViewResult viewResult = (ViewResult)adminController.Index();

            // Check the correct users were returned
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<UserListModel>, "Expected List<UserListModel> got " + viewResult.Model.GetType().ToString());
            List<UserListModel> actualAndroUsers = (List<UserListModel>)viewResult.Model;

            // Expected results
            List<UserListModel> expectedResults = new List<UserListModel>()
            {
                new UserListModel() {AndroUser = newAndroUser, SecurityGroups = ""}
            };

            string resultMessage = ValidationHelper.CheckUsers(expectedResults, actualAndroUsers);
            if (resultMessage.Length > 0) Assert.Fail(resultMessage);
        }
    }
}
