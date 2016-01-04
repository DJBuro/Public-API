using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AndroAdmin.Controllers;
using AndroAdmin.Model;
using AndroUsersDataAccess.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AndroAdminTests
{
    [TestClass]
    public class SecurityGroupTests
    {
        [TestInitialize]
        public void Startup()
        {
            // Delete any test databases left over from previous test runs
            DatabaseHelper.DeleteTestDatabases();
        }

        /// <summary>
        /// Start with a new empty database and run a series of tests against it
        /// </summary>
        [TestMethod]
        public void SecurityGroupsTestMethod()
        {
            // Create a test database
            string androUsersConnectionString = DatabaseHelper.CreateTestAndroUsersDatabase(
                DatabaseHelper.CreateAndroUsersDatabaseScriptFilename,
                DatabaseHelper.CreateAndroUsersDataScriptFilename);

            // Ensure that no security groups are returned for an empty database
            this.NoSecurityGroups(androUsersConnectionString);

            // Add a security group with a blank name - should fail
            this.AddSecurityGroupBlankName(androUsersConnectionString);

            // Add security group 1 - should succeed
            this.AddFirstSecurityGroup(androUsersConnectionString);

            // Add a new security group with the same name as security group 1 - should fail
            this.AddSecurityGroupNameAlreadyUsed(androUsersConnectionString);

            // Get the details for a security group that doesn't exist
            this.GetInvalidSecurityGroup(androUsersConnectionString);

            // Get the details for security group 1
            this.GetFirstSecurityGroup(androUsersConnectionString);

            // Modify security group 1 to make the name blank - should fail
            this.ModifyFirstSecurityGroupBlankName(androUsersConnectionString);

            // Add security group 2 - should succeed
            this.AddSecondSecurityGroup(androUsersConnectionString);

            // Modify security group 2 changing the name to match security group 1 - should fail
            this.ModifyFirstSecurityGroupNameAlreadyUsed(androUsersConnectionString);

            // Modify security group 1 to a different name - should succeed
            this.ModifySecurityGroup(androUsersConnectionString);

            // Add a permission to security group 1
            this.AddFirstPermissionToSecurityGroup(androUsersConnectionString);

            // Add a second permission to security group 1
            this.AddSecondPermissionToSecurityGroup(androUsersConnectionString);
        }

        /// <summary>
        /// Ensure that no security groups are returned for an empty database
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void NoSecurityGroups(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Get the security groups
            object result = (ViewResult)securityGroupController.Index();

            // Check the result
            Assert.IsTrue(result is ViewResult);
            ViewResult viewResult = (ViewResult)result;

            // Shouldn't be any security groups yet
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());

            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;
            Assert.AreEqual(0, actualSecurityGroups.Count);
        }

        /// <summary>
        /// Add a security group with a blank name - should fail
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void AddSecurityGroupBlankName(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            SecurityGroupModel newSecurityGroupModel = new SecurityGroupModel()
            {
                SecurityGroup = new SecurityGroup()
                {
                    Name = "",
                    Description = "",
                    Permissions = null
                }
            };

            // Add a security group
            object result = securityGroupController.Add(newSecurityGroupModel);

            Assert.AreEqual(1, securityGroupController.ModelState.Count);
            Assert.AreEqual(1, securityGroupController.ModelState["SecurityGroup.Name"].Errors.Count);
            Assert.AreEqual("Name must be between one and 255 characters", securityGroupController.ModelState["SecurityGroup.Name"].Errors[0].ErrorMessage);
        }

        /// <summary>
        /// Add security group 1 - should succeed
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void AddFirstSecurityGroup(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            SecurityGroupModel newSecurityGroupModel = new SecurityGroupModel()
            {
                SecurityGroup = new SecurityGroup()
                {
                    Name = "test security group",
                    Description = "test security group description",
                    Permissions = null
                }
            };

            // Add a security group
            object result = securityGroupController.Add(newSecurityGroupModel);

            // Check the result
            Assert.IsTrue(result is RedirectToRouteResult);
            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult)result;

            // We should be redirected back to the index page
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("SecurityGroup", redirectToRouteResult.RouteValues["controller"]);

            // To check that the security group was added by getting a list of security groups
            ViewResult viewResult = (ViewResult)securityGroupController.Index();

            // Check the correct security groups were returned
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // Expected results
            newSecurityGroupModel.SecurityGroup.Id = 1; // It's the first security group so id will be 1
            List<SecurityGroupModel> expectedResults = new List<SecurityGroupModel>()
            {
                newSecurityGroupModel
            };

            string resultMessage = ValidationHelper.CheckSecurityGroups(expectedResults, actualSecurityGroups);
            if (resultMessage.Length > 0) Assert.Fail(resultMessage);
        }

        /// <summary>
        /// Add a new security group with the same name as security group 1 - should fail
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void AddSecurityGroupNameAlreadyUsed(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            SecurityGroupModel newSecurityGroupModel = new SecurityGroupModel()
            {
                SecurityGroup = new SecurityGroup()
                {
                    Name = "test security group",
                    Description = "test security group description name already used",
                    Permissions = null
                }
            };

            // Add a security group
            object result = securityGroupController.Add(newSecurityGroupModel);

            Assert.AreEqual(1, securityGroupController.ModelState.Count);
            Assert.AreEqual(1, securityGroupController.ModelState["SecurityGroup.Name"].Errors.Count);
            Assert.AreEqual("Name already used", securityGroupController.ModelState["SecurityGroup.Name"].Errors[0].ErrorMessage);
        }

        /// <summary>
        /// Get the details for a security group that doesn't exist
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void GetInvalidSecurityGroup(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Get a security group that doesn't exist
            object result = securityGroupController.Details(-1, false);

            // Check the result
            Assert.IsTrue(result is RedirectToRouteResult);
            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult)result;

            // We should be redirected back to the index page
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("Error", redirectToRouteResult.RouteValues["controller"]);
        }

        /// <summary>
        /// Get the details for security group 1
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void GetFirstSecurityGroup(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Get all the security groups
            ViewResult viewResult = (ViewResult)securityGroupController.Index();

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // Get the security group details for the first (and only) item
            viewResult = (ViewResult)securityGroupController.Details(actualSecurityGroups[0].SecurityGroup.Id, false);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is SecurityGroupModel, "Expected SecurityGroupModel got " + viewResult.Model.GetType().ToString());
            SecurityGroupModel actualSecurityGroup = (SecurityGroupModel)viewResult.Model;

            // Check that the security group details are correct
            Assert.AreEqual("test security group", actualSecurityGroup.SecurityGroup.Name);
            Assert.IsNull(actualSecurityGroup.Permissions);
        }

        /// <summary>
        /// Modify security group 1 to make the name blank - should fail
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void ModifyFirstSecurityGroupBlankName(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Get all the security groups
            ViewResult viewResult = (ViewResult)securityGroupController.Index();

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // Blank out the name
            SecurityGroupModel securityGroupModel = actualSecurityGroups[0];
            securityGroupModel.SecurityGroup.Name = "";
            //SecurityGroup updatedSecurityGroup = new SecurityGroup()
            //{
            //    Id = securityGroupModel.SecurityGroup.Id,
            //    Name = "",
            //    Description = "test security group description update name blank",
            //    Permissions = null
            //};

            // Update the security group
            object result = securityGroupController.Details(securityGroupModel);

            // Check that the change was rejected
            Assert.AreEqual(1, securityGroupController.ModelState.Count);
            Assert.AreEqual(1, securityGroupController.ModelState["SecurityGroup.Name"].Errors.Count);
            Assert.AreEqual("Name must be between one and 255 characters", securityGroupController.ModelState["SecurityGroup.Name"].Errors[0].ErrorMessage);
        }

        /// <summary>
        /// Add security group 2 - should succeed
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void AddSecondSecurityGroup(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            SecurityGroupModel newSecurityGroupModel = new SecurityGroupModel()
            {
                SecurityGroup = new SecurityGroup()
                {
                    Name = "test security group 2",
                    Description = "test security group 2 description",
                    Permissions = null
                }
            };

            // Add a security group
            object result = securityGroupController.Add(newSecurityGroupModel);

            // Check the result
            Assert.IsTrue(result is RedirectToRouteResult);
            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult)result;

            // We should be redirected back to the index page
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("SecurityGroup", redirectToRouteResult.RouteValues["controller"]);

            // To check that the security group was added by getting a list of security groups
            ViewResult viewResult = (ViewResult)securityGroupController.Index();

            // Check the correct security groups were returned
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // Expected results
            newSecurityGroupModel.SecurityGroup.Id = 2; // It's the first security group so id will be 2
            List<SecurityGroupModel> expectedResults = new List<SecurityGroupModel>()
            {
                // The first security group
                new SecurityGroupModel()
                {
                    SecurityGroup = new SecurityGroup()
                    {
                        Id = 1,
                        Name = "test security group",
                        Description = "test security group description",
                        Permissions = null
                    }
                },
                // The second security group
                newSecurityGroupModel
            };

            string resultMessage = ValidationHelper.CheckSecurityGroups(expectedResults, actualSecurityGroups);
            if (resultMessage.Length > 0) Assert.Fail(resultMessage);
        }

        /// <summary>
        /// Modify security group 1 changing the name to match security group 2 - should fail
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void ModifyFirstSecurityGroupNameAlreadyUsed(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Get all the security groups
            ViewResult viewResult = (ViewResult)securityGroupController.Index();

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // Change the name to match security group 2
            SecurityGroupModel securityGroupModel = actualSecurityGroups[0];
            securityGroupModel.SecurityGroup.Name = "test security group 2";

            // Update the security group
            object result = securityGroupController.Details(securityGroupModel);

            // Check that the change was rejected
            Assert.AreEqual(1, securityGroupController.ModelState.Count);
            Assert.AreEqual(1, securityGroupController.ModelState["SecurityGroup.Name"].Errors.Count);
            Assert.AreEqual("Name already used", securityGroupController.ModelState["SecurityGroup.Name"].Errors[0].ErrorMessage);
        }

        /// <summary>
        /// Modify security group 1 to a different name - should succeed
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void ModifySecurityGroup(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Get all the security groups
            ViewResult viewResult = (ViewResult)securityGroupController.Index();

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // Change the name
            SecurityGroupModel securityGroupModel = actualSecurityGroups[0];
            securityGroupModel.SecurityGroup.Name = "A different security group name";

            // Update the security group
            object result = securityGroupController.Details(securityGroupModel);

            // Check the result
            Assert.IsTrue(result is RedirectToRouteResult);
            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult)result;

            // We should be redirected back to the same page
            Assert.AreEqual(0, securityGroupController.ModelState.Count);
            Assert.AreEqual("Details", redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("SecurityGroup", redirectToRouteResult.RouteValues["controller"]);

            // To check that the security group was modified by getting a list of security groups
            viewResult = (ViewResult)securityGroupController.Index();

            // Check the correct security groups were returned
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // Expected results
            List<SecurityGroupModel> expectedResults = new List<SecurityGroupModel>()
            {
                new SecurityGroupModel() { SecurityGroup = new SecurityGroup() { Id = 1, Name = "A different security group name", Description = "test security group description" } },
                new SecurityGroupModel() { SecurityGroup = new SecurityGroup() { Id = 2, Name = "test security group 2", Description = "test security group 2 description" } }
            };

            string resultMessage = ValidationHelper.CheckSecurityGroups(expectedResults, actualSecurityGroups);
            if (resultMessage.Length > 0) Assert.Fail(resultMessage);
        }

        /// <summary>
        /// Add a permission to security group 1
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void AddFirstPermissionToSecurityGroup(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Get all the security groups
            ViewResult viewResult = (ViewResult)securityGroupController.Index();

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // The security group we will be working with
            SecurityGroup securityGroup = actualSecurityGroups[0].SecurityGroup;

            // Get all the permissions
            viewResult = (ViewResult)securityGroupController.Permissions(securityGroup.Id);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is SecurityGroupModel, "Expected GroupModel got " + viewResult.Model.GetType().ToString());
            SecurityGroupModel actualSecurityGroupModel = (SecurityGroupModel)viewResult.Model;

            // Check that the list of all permissions is correct
            ValidationHelper.CheckAllPermissions(actualSecurityGroupModel.Permissions);

            // Add a single permission to the security group
            actualSecurityGroupModel.Permissions[0].Selected = true;

            // Submit the selected permissions
            object result = securityGroupController.Permissions(actualSecurityGroupModel);

            // Check the result
            Assert.IsTrue(result is RedirectToRouteResult);
            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult)result;

            // We should be redirected back to the details page
            Assert.AreEqual(0, securityGroupController.ModelState.Count);
            Assert.AreEqual("Details", redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("SecurityGroup", redirectToRouteResult.RouteValues["controller"]);
            Assert.AreEqual(securityGroup.Id, redirectToRouteResult.RouteValues["Id"]);
            Assert.AreEqual(false, redirectToRouteResult.RouteValues["edit"]);

            // Get the security group details so we can check that the permission has actually been set
            viewResult = (ViewResult)securityGroupController.Details(securityGroup.Id, false);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is SecurityGroupModel, "Expected SecurityGroupModel got " + viewResult.Model.GetType().ToString());
            SecurityGroupModel actualSecurityGroup = (SecurityGroupModel)viewResult.Model;

            // Check that the security group details are correct
            Assert.AreEqual(securityGroup.Name, actualSecurityGroup.SecurityGroup.Name);
            Assert.IsNotNull(actualSecurityGroup.SecurityGroup.Permissions);
            Assert.AreEqual(1, actualSecurityGroup.SecurityGroup.Permissions.Count);
            Assert.AreEqual(2, actualSecurityGroup.SecurityGroup.Permissions[0].Id); // For some reason the permission ids start at 2 :(
            Assert.AreEqual("ViewStores", actualSecurityGroup.SecurityGroup.Permissions[0].Name);
        }

        /// <summary>
        /// Add a secondpermission to security group 1
        /// </summary>
        /// <param name="androUsersConnectionString"></param>
        private void AddSecondPermissionToSecurityGroup(string androUsersConnectionString)
        {
            SecurityGroupController securityGroupController = new SecurityGroupController();
            securityGroupController.AndroUsersConnectionStringOverride = androUsersConnectionString;

            // Get all the security groups
            ViewResult viewResult = (ViewResult)securityGroupController.Index();

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<SecurityGroupModel>, "Expected List<SecurityGroupModel> got " + viewResult.Model.GetType().ToString());
            List<SecurityGroupModel> actualSecurityGroups = (List<SecurityGroupModel>)viewResult.Model;

            // The security group we will be working with
            SecurityGroup securityGroup = actualSecurityGroups[0].SecurityGroup;

            // Get all the permissions
            viewResult = (ViewResult)securityGroupController.Permissions(securityGroup.Id);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is SecurityGroupModel, "Expected GroupModel got " + viewResult.Model.GetType().ToString());
            SecurityGroupModel actualSecurityGroupModel = (SecurityGroupModel)viewResult.Model;

            // Check that the list of all permissions is correct
            ValidationHelper.CheckAllPermissions(actualSecurityGroupModel.Permissions);

            // Add a second permission to the security group
            actualSecurityGroupModel.Permissions[1].Selected = true;

            // Submit the selected permissions
            object result = securityGroupController.Permissions(actualSecurityGroupModel);

            // Check the result
            Assert.IsTrue(result is RedirectToRouteResult);
            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult)result;

            // We should be redirected back to the details view
            Assert.AreEqual(0, securityGroupController.ModelState.Count);
            Assert.AreEqual("Details", redirectToRouteResult.RouteValues["action"]);
            Assert.AreEqual("SecurityGroup", redirectToRouteResult.RouteValues["controller"]);
            Assert.AreEqual(securityGroup.Id, redirectToRouteResult.RouteValues["Id"]);
            Assert.AreEqual(false, redirectToRouteResult.RouteValues["edit"]);

            // Get the security group details so we can check that the permission has actually been set
            viewResult = (ViewResult)securityGroupController.Details(securityGroup.Id, false);

            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is SecurityGroupModel, "Expected SecurityGroupModel got " + viewResult.Model.GetType().ToString());
            SecurityGroupModel actualSecurityGroup = (SecurityGroupModel)viewResult.Model;

            // Check that the security group details are correct
            Assert.AreEqual(securityGroup.Name, actualSecurityGroup.SecurityGroup.Name);
            Assert.IsNotNull(actualSecurityGroup.SecurityGroup.Permissions);
            Assert.AreEqual(2, actualSecurityGroup.SecurityGroup.Permissions.Count);
            Assert.AreEqual(2, actualSecurityGroup.SecurityGroup.Permissions[0].Id); // For some reason the permission ids start at 2 :(
            Assert.AreEqual(3, actualSecurityGroup.SecurityGroup.Permissions[1].Id); // For some reason the permission ids start at 2 :(
            Assert.AreEqual("ViewStores", actualSecurityGroup.SecurityGroup.Permissions[0].Name);
        }
    }
}
