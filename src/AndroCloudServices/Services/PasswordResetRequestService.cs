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
    public class PasswordResetRequestService
    {
        public static Response Put(
            string externalApplicationId, 
            string username, 
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

            // Check if this is a valid application id
            ACSApplication acsApplication = null;
            androCloudDataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                // Invalid application id
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            // Add the password reset request
            string token = "";
            bool isFacebookAccount = false;
            string errorMessage = dataWarehouseDataAccessFactory.PasswordResetRequestDataAccess.RequestPasswordReset(username, acsApplication.Id, out token, out isFacebookAccount);

            if (isFacebookAccount)
            {
                ErrorHelper.Log.Debug("Attempt to reset a facebook account for user " + username);

                return new Response(Errors.PasswordResetNotAllowedForFacebookAccount, dataType);
            }
            else if (errorMessage != null && errorMessage.Length > 0)
            {
                ErrorHelper.Log.Error(errorMessage);
                Error error = Errors.BadData;
                error.ErrorMessage = error.ErrorMessage.Replace("{errorMessage}", errorMessage);
                return new Response(error, dataType); ;
            }

            return new Response(SerializeHelper.Serialize<PasswordResetRequest>(new PasswordResetRequest() { Token = token }, dataType));
        }

        public static Response Post(
            string externalApplicationId, 
            string username,
            string passwordResetRequestText,  
            string newPassword,
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

            // Was a password provided?
            if (newPassword == null || newPassword.Length == 0)
            {
                // password was not provided
                return new Response(Errors.MissingPassword, dataType);
            }

            // Check if this is a valid application id
            ACSApplication acsApplication = null;
            androCloudDataAccessFactory.AcsApplicationDataAccess.GetByExternalId(externalApplicationId, out acsApplication);

            if (acsApplication == null)
            {
                // Invalid application id
                return new Response(Errors.UnknownApplicationId, dataType);
            }

            PasswordResetRequest passwordResetRequest = null;
            string errorMessage = SerializeHelper.Deserialize<PasswordResetRequest>(passwordResetRequestText, dataType, out passwordResetRequest);

            if (errorMessage != null && errorMessage.Length > 0)
            {
                ErrorHelper.Log.Error(errorMessage);
                Error error = Errors.BadData;
                error.ErrorMessage = error.ErrorMessage.Replace("{errorMessage}", errorMessage);
                return new Response(error, dataType); ;
            }

            // Reset password
            Guid? customerId;
            errorMessage = dataWarehouseDataAccessFactory.PasswordResetRequestDataAccess.PasswordReset(username, passwordResetRequest.Token, newPassword, out customerId);

            if (errorMessage != null && errorMessage.Length > 0)
            {
                ErrorHelper.Log.Error(errorMessage);
                Error error = null;
                if (errorMessage == "Password reset request expired")
                {
                    error = Errors.PasswordResetRequestExpired;
                }
                else
                {
                    error = Errors.BadData;
                    error.ErrorMessage = error.ErrorMessage.Replace("{errorMessage}", errorMessage);
                }
                return new Response(error, dataType);
            }

            ErrorHelper.Log.Info("Password reset for customerId: " + customerId);

            return new Response();
        }
    }
}
