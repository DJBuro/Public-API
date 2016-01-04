using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System.Xml.Serialization;
using System.IO;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using DataWarehouseDataAccess;
using AndroCloudHelper;
using DataWarehouseDataAccess.Domain;

namespace AndroCloudServices.Services
{
    public class CustomerService
    {
        public static Response Get(
            string externalApplicationId,
            string username,
            string password,
            string externalSiteId,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
            out string sourceId)
        {
            // The source is the externalApplicationId
            sourceId = externalApplicationId;

            // Was a application id provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // Application id was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an username provided?
            if (username == null || username.Length == 0)
            {
                // username was not provided
                return new Response(Errors.MissingUsername, dataType);
            }

            // Check if this is a valid application id
            ACSApplication acsApplication = null;
            androCloudDataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId,out acsApplication);

            if (acsApplication == null)
            {
                // Invalid application id
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            string errorMessage = "";
            Response response = null;
            if (password != null && password.Length > 0)
            {
                // Get the full customer details from the database
                Customer customer = null;
                errorMessage = dataWarehouseDataAccessFactory.CustomerDataAccess.GetByUsernamePassword(username, password, acsApplication.Id, out customer);

                if (errorMessage.Length == 0)
                {
                    // Add in the customer loyalty
                    IEnumerable<SiteLoyalty> siteLoyalty;
                    errorMessage = androCloudDataAccessFactory.SiteLoyaltyDataAccess.GetAllByExternalApplicationId(externalApplicationId, externalSiteId, out siteLoyalty);

                    if (siteLoyalty.Any())
                    {
                        foreach (var loyalty in siteLoyalty)
                        {
                            dataWarehouseDataAccessFactory.CustomerDataAccess.AddLoyaltyProvider(customer, loyalty);
                        }
                    }
                }

                if (errorMessage == "Incorrect password")
                {
                    response = new Response(Errors.IncorrectPassword, dataType);
                }
                else if (errorMessage.StartsWith("Unknown username:"))
                {
                    response = new Response(Errors.UnknownUsername, dataType);
                }
                else
                {
                    response = new Response(SerializeHelper.Serialize<Customer>(customer, dataType));
                }
            }
            else
            {
                // Just check if the customer exists
                bool exists = false;
                errorMessage = dataWarehouseDataAccessFactory.CustomerDataAccess.Exists(username, acsApplication.Id, out exists);

                if (exists)
                {
                    response = new Response(SerializeHelper.Serialize<Customer>(new Customer(), dataType));
                }
                else
                {
                    response = new Response(Errors.UnknownUsername, dataType);
                }
            }
            
            // Success
            return response;
        }

        public static Response Put(
            string externalApplicationId, 
            string username, 
            string password, 
            string customerData, 
            string externalSiteId,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory, 
            out string sourceId)
        {
            sourceId = externalApplicationId;

            // Was a externalPartnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an username provided?
            if (username == null || username.Length == 0)
            {
                // username was not provided
                return new Response(Errors.MissingUsername, dataType);
            }

            // Was an password provided?
            if (password == null || password.Length == 0)
            {
                // password was not provided
                return new Response(Errors.MissingPassword, dataType);
            }

            // Deserialize the XML/JSON into an object
            Customer customer = null;
            string result = SerializeHelper.Deserialize<Customer>(customerData, dataType, out customer);

            if (result != null && result.Length > 0)
            {
                return new Response(Errors.InvalidCustomer, dataType);
            }

            // Make sure there is a customer loyalties collection
            customer.CustomerLoyalties = customer.CustomerLoyalties == null ? new List<CustomerLoyalty>() : customer.CustomerLoyalties;

            // Check if this is a valid application id
            ACSApplication acsApplication = null;
            androCloudDataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                // Invalid application id
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            // Add the new customer
            string errorMessage = dataWarehouseDataAccessFactory.CustomerDataAccess.AddCustomer(username, password, acsApplication.Id, customer);

            IEnumerable<SiteLoyalty> siteLoyalty = null;
            if (errorMessage.Length == 0)
            {
                // Add in the customer loyalty
                errorMessage = androCloudDataAccessFactory.SiteLoyaltyDataAccess.GetAllByExternalApplicationId(externalApplicationId, externalSiteId, out siteLoyalty);
            }

            if (errorMessage.Length == 0)
            {
                if (siteLoyalty.Any())
                {
                    foreach (var loyalty in siteLoyalty)
                    {
                        dataWarehouseDataAccessFactory.CustomerDataAccess.AddLoyaltyProvider(customer, loyalty);
                    }
                }
            }

            Response response = null;

            // Any errors?
            if (errorMessage.StartsWith("Username already used:") || errorMessage.StartsWith("User-name already"))
            {
                response = new Response(Errors.UsernameAlreadyUsed, dataType);
            }
            else if (errorMessage.StartsWith("Unknown country:")) response = new Response(Errors.UnknownCountry, dataType);
            else if (errorMessage.StartsWith("Unknown marketing level:")) response = new Response(Errors.UnknownMarketingLevel, dataType);
            else if (errorMessage.StartsWith("Unknown contact type:")) response = new Response(Errors.UnknownContactType, dataType);
            else if (errorMessage.Length > 0) response = new Response(Errors.InternalError, dataType);
            else response = new Response(SerializeHelper.Serialize<Customer>(customer, dataType));

            return response;
        }

        public static Response Post(
            string externalApplicationId, 
            string username, 
            string password, 
            string newPassword,
            string customerData, 
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory, 
            out string sourceId)
        {
            sourceId = externalApplicationId;

            // Was a externalPartnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an username provided?
            if (username == null || username.Length == 0)
            {
                // username was not provided
                return new Response(Errors.MissingUsername, dataType);
            }

            // Was an password provided?
            if (password == null || password.Length == 0)
            {
                // password was not provided
                return new Response(Errors.MissingPassword, dataType);
            }

            // Deserialize the XML/JSON into an object
            Customer customer = null;
            if (customerData != null && customerData.Length > 0)
            {
                string result = SerializeHelper.Deserialize<Customer>(customerData, dataType, out customer);

                if (result != null && result.Length > 0)
                {
                    return new Response(Errors.InvalidCustomer, dataType);
                }
            }

            // Check if this is a valid application id
            ACSApplication acsApplication = null;
            androCloudDataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                // Invalid application id
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            // Update the customer
            string errorMessage = dataWarehouseDataAccessFactory.CustomerDataAccess.UpdateCustomer(username, password, newPassword, acsApplication.Id, customer);

            Response response = null;

            // Any errors?
            if (errorMessage.StartsWith("Username already used:")) response = new Response(Errors.UsernameAlreadyUsed, dataType);
            else if (errorMessage == "Incorrect password") response = new Response(Errors.IncorrectPassword, dataType);
            else if (errorMessage.StartsWith("Unknown username:")) response = new Response(Errors.UnknownUsername, dataType);
            else if (errorMessage.StartsWith("Unknown country:")) response = new Response(Errors.UnknownCountry, dataType);
            else if (errorMessage.StartsWith("Unknown marketing level:")) response = new Response(Errors.UnknownMarketingLevel, dataType);
            else if (errorMessage.StartsWith("Unknown contact type:")) response = new Response(Errors.UnknownContactType, dataType);
            else response = new Response();

            return response;
        }

        public static Response PostLoyalty(
            string externalApplicationId,
            string username,
            string customerData,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
            out string sourceId)
        {
            sourceId = externalApplicationId;

            // Was a externalPartnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an username provided?
            if (username == null || username.Length == 0)
            {
                // username was not provided
                return new Response(Errors.MissingUsername, dataType);
            }

            // Deserialize the XML/JSON into an object
            CustomerLoyalty customer = null;
            if (customerData != null && customerData.Length > 0)
            {
                string result = SerializeHelper.Deserialize<CustomerLoyalty>(customerData, dataType, out customer);

                if (result != null && result.Length > 0)
                {
                    return new Response(Errors.Loyalty_InvalidData, dataType);
                }
            }

            if (customer == null)
            {
                return new Response(Errors.Loyalty_InvalidData, dataType);
            }
            // Check if this is a valid application id
            ACSApplication acsApplication = null;
            androCloudDataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                // Invalid application id
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            // Update the customer
            string errorMessage = dataWarehouseDataAccessFactory.CustomerDataAccess.UpdateCustomerLoyalty(username, acsApplication.Id, customer);

            Response response = null;

            // Any errors?
            if (errorMessage.StartsWith("Unknown username:")) response = new Response(Errors.UnknownUsername, dataType);
            else if (errorMessage.StartsWith("Customer Loyalty NULL:")) response = new Response(Errors.Loyalty_InvalidData, dataType);
            else if (errorMessage.StartsWith("CustomerId mismatch:")) response = new Response(Errors.Loyalty_MismatchCustomerId, dataType);
            else response = new Response();

            return response;
        }


        public static Response PostApplyCustomerLoyalty(
            string externalApplicationId,
            string username,
            string externalOrderRef, 
            string action,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory,
            out string sourceId)
        {
            sourceId = externalApplicationId;

            // Was a externalPartnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an username provided?
            if (username == null || username.Length == 0)
            {
                // username was not provided
                return new Response(Errors.MissingUsername, dataType);
            }

            if (string.IsNullOrWhiteSpace(action)) 
            {
                // need an action "Commit", "Reject" 
                return new Response(Errors.Loyalty_MissingAction, dataType);
            }

            // Check if this is a valid application id
            ACSApplication acsApplication = null;
            androCloudDataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                // Invalid application id
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            string errorMessage = string.Empty;

            if (string.Compare("Accept", action, true) >= 0 || string.Compare("Commit", action, true) >= 0)
            {
                // Update the customer
                errorMessage = dataWarehouseDataAccessFactory.CustomerDataAccess
                    .UpdateCustomerLoyaltyPoints(username, acsApplication.Id, externalOrderRef, commitPointsToCustomer: true);
            }
            else if(string.Compare("Reject", action, true) >= 0)
            {
                errorMessage = dataWarehouseDataAccessFactory.CustomerDataAccess
                    .UpdateCustomerLoyaltyPoints(username, acsApplication.Id, externalOrderRef, commitPointsToCustomer: false);
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
            
            Response response = null;

            // Any errors?
            if (errorMessage.StartsWith("Unknown username:")) response = new Response(Errors.UnknownUsername, dataType);
            else if (errorMessage.StartsWith("Customer Loyalty NULL:")) response = new Response(Errors.Loyalty_InvalidData, dataType);
            else if (errorMessage.StartsWith("CustomerId mismatch:")) response = new Response(Errors.Loyalty_MismatchCustomerId, dataType);
            else response = new Response();

            return response;
        }
    }
}
