using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroAdmin.Model;
using AndroUsersDataAccess.Domain;

namespace AndroAdminTests
{
    public class ValidationHelper
    {
        public static string CheckUsers(List<UserListModel> expectedUsers, List<UserListModel> actualUsers)
        {
            if (expectedUsers.Count != actualUsers.Count)
            {
                return "Expected " + expectedUsers.Count.ToString() + " users but got " + actualUsers.Count.ToString();
            }

            foreach (UserListModel expectedAndroUser in expectedUsers)
            {
                foreach (UserListModel actualAndroUser in actualUsers)
                {
                    if (actualAndroUser.AndroUser.Id == expectedAndroUser.AndroUser.Id)
                    {
                        if (actualAndroUser.SecurityGroups != expectedAndroUser.SecurityGroups)
                            return "SecurityGroups properties do not match for id " + expectedAndroUser.AndroUser.Id.ToString() + ".  Expected " + expectedAndroUser.SecurityGroups + " got " + actualAndroUser.SecurityGroups;

                        if (actualAndroUser.AndroUser.Active != expectedAndroUser.AndroUser.Active)
                            return "Active properties do not match for id " + expectedAndroUser.AndroUser.Id.ToString() + ".  Expected " + expectedAndroUser.AndroUser.Active.ToString() + " got " + actualAndroUser.AndroUser.Active.ToString();
                        
                        if (actualAndroUser.AndroUser.Created != expectedAndroUser.AndroUser.Created)
                            return "Created properties do not match for id " + expectedAndroUser.AndroUser.Id.ToString() + ".  Expected " + expectedAndroUser.AndroUser.Created.ToString() + " got " + actualAndroUser.AndroUser.Created.ToString();
                        
                        if (actualAndroUser.AndroUser.EmailAddress != expectedAndroUser.AndroUser.EmailAddress)
                            return "EmailAddress properties do not match for id " + expectedAndroUser.AndroUser.Id.ToString() + ".  Expected " + expectedAndroUser.AndroUser.EmailAddress.ToString() + " got " + actualAndroUser.AndroUser.EmailAddress.ToString();
                        
                        if (actualAndroUser.AndroUser.FirstName != expectedAndroUser.AndroUser.FirstName)
                            return "FirstName properties do not match for id " + expectedAndroUser.AndroUser.Id.ToString() + ".  Expected " + expectedAndroUser.AndroUser.FirstName + " got " + actualAndroUser.AndroUser.FirstName;
                        
                        if (actualAndroUser.AndroUser.Password != expectedAndroUser.AndroUser.Password)
                            return "Password properties do not match for id " + expectedAndroUser.AndroUser.Id.ToString() + ".  Expected " + expectedAndroUser.AndroUser.Password + " got " + actualAndroUser.AndroUser.Password;
                    }
                }
            }

            return "";
        }

        public static string CheckSecurityGroups(List<SecurityGroupModel> expectedSecurityGroups, List<SecurityGroupModel> actualSecurityGroups)
        {
            // Did we get the expected number of security groups?
            if (expectedSecurityGroups.Count != actualSecurityGroups.Count)
            {
                return "Expected " + expectedSecurityGroups.Count.ToString() + " security groups but got " + actualSecurityGroups.Count.ToString();
            }

            // Check that each expected security group exists and is correct
            foreach (SecurityGroupModel expectedSecurityGroup in expectedSecurityGroups)
            {
                bool securityGroupFound = false;
                foreach (SecurityGroupModel actualSecurityGroup in actualSecurityGroups)
                {
                    if (actualSecurityGroup.SecurityGroup.Id == expectedSecurityGroup.SecurityGroup.Id)
                    {
                        // Check that the name is correct
                        if (actualSecurityGroup.SecurityGroup.Name != expectedSecurityGroup.SecurityGroup.Name)
                            return "Name properties do not match for id " + expectedSecurityGroup.SecurityGroup.Id.ToString() + ".  Expected " + expectedSecurityGroup.SecurityGroup.Name + " got " + actualSecurityGroup.SecurityGroup.Name;
                        
                        // Check that the description is correct
                        if (actualSecurityGroup.SecurityGroup.Description != expectedSecurityGroup.SecurityGroup.Description)
                            return "Description properties do not match for id " + expectedSecurityGroup.SecurityGroup.Id.ToString() + ".  Expected " + expectedSecurityGroup.SecurityGroup.Description + " got " + actualSecurityGroup.SecurityGroup.Description;

                        // Check that the permissions are correct
                        if (actualSecurityGroup.Permissions == null && expectedSecurityGroup.Permissions != null)
                            return "Unexpected security groups for id " + expectedSecurityGroup.SecurityGroup.Id.ToString() + ".  Expected null object got " + actualSecurityGroup.Permissions.Count;

                        if (actualSecurityGroup.Permissions != null && expectedSecurityGroup.Permissions == null)
                            return "Unexpected security groups for id " + expectedSecurityGroup.SecurityGroup.Id.ToString() + ".  Expected " + expectedSecurityGroup.Permissions.Count + " got null object";

                        if (expectedSecurityGroup.Permissions != null)
                        {
                            if (expectedSecurityGroup.Permissions.Count != expectedSecurityGroup.Permissions.Count)
                            {
                                return "Unexpected security groups for id " + expectedSecurityGroup.SecurityGroup.Id.ToString() + ".  Expected " + expectedSecurityGroup.Permissions.Count + " got " + actualSecurityGroup.Permissions.Count;
                            }

                            // Check for each expected permission
                            foreach (PermissionModel expectedPermissionModel in expectedSecurityGroup.Permissions)
                            {
                                bool foundPermission = false;
                                foreach (PermissionModel actualPermissionModel in actualSecurityGroup.Permissions)
                                {
                                    if (expectedPermissionModel.Permission.Id == actualPermissionModel.Permission.Id)
                                    {
                                        foundPermission = true;
                                        break;
                                    }
                                }

                                if (!foundPermission)
                                {
                                    return "Expected security group " + expectedSecurityGroup.SecurityGroup.Id.ToString() + " missing expected permission " + expectedPermissionModel.Permission.Id;
                                }
                            }
                        }

                        securityGroupFound = true;
                    }
                }

                // Did we find the expected security group
                if (!securityGroupFound)
                {
                    return "Expected security group " + expectedSecurityGroup.SecurityGroup.Id.ToString() + " missing";
                }
            }

            return "";
        }

        private static List<Permission> ExpectedPermssions = new List<Permission>()
        {
            new Permission() { Name="ViewStores" },
            new Permission() { Name="AddStore" },
            new Permission() { Name="EditStore" },
            new Permission() { Name="ViewPaymentProviders" },
            new Permission() { Name="AddPaymentProvider" },
            new Permission() { Name="ViewAMSStores" },
            new Permission() { Name="EditAMSStore" },
            new Permission() { Name="ViewAMSServers" },
            new Permission() { Name="AddAMSServer" },
            new Permission() { Name="EditAMSServer" },
            new Permission() { Name="ViewFTPSites" },
            new Permission() { Name="AddFTPSite" },
            new Permission() { Name="EditFTPSite" },
            new Permission() { Name="ViewACSPartners" },
            new Permission() { Name="AddACSPartner" },
            new Permission() { Name="EditACSPartner" },
            new Permission() { Name="ViewCloudServers" },
            new Permission() { Name="ViewAndroAdminLinks" },
            new Permission() { Name="ViewUsers" },
            new Permission() { Name="AddUser" },
            new Permission() { Name="EditUser" },
            new Permission() { Name="ViewSecurityGroups" },
            new Permission() { Name="AddSecurityGroup" },
            new Permission() { Name="EditSecurityGroup" }
        };

        public static string CheckAllPermissions(List<PermissionModel> permissions)
        {
            // Do we have the correct number of permissions?
            if (permissions.Count != ValidationHelper.ExpectedPermssions.Count)
            {
                return "Expected " + ValidationHelper.ExpectedPermssions.Count.ToString() + " permissions but got " + permissions.Count.ToString();
            }

            // Check that we've got each expected permission 
            foreach (Permission expectedPermission in ValidationHelper.ExpectedPermssions)
            {
                bool found = false;
                foreach (PermissionModel actualPermissionModel in permissions)
                {
                    if (expectedPermission.Name == actualPermissionModel.Permission.Name)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return "Expected permission " + expectedPermission.Id + " not found";
                }
            }

            return "";
        }
    }
}
