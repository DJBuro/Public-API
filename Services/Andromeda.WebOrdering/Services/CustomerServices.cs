using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Andromeda.WebOrdering.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Andromeda.WebOrdering.Services
{
    public class CustomerServices
    {
        public static bool Get(
            string key, 
            DomainConfiguration domainConfiguration,
            string username,
            string siteId,
            string passwordHeader, 
            out HttpStatusCode httpStatus, 
            out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/customers/" + username + "?applicationid=" + domainConfiguration.ApplicationId + "&siteId=" + siteId;

                    success = HttpHelper.RestCall
                    (
                        "GET", 
                        url, 
                        "Application/JSON", 
                        "Application/JSON",
                        new Dictionary<string, string>() { {"Authorization", passwordHeader} }, 
                        "", 
                        true, 
                        out httpStatus,
                        out json
                    );

                    if (success) break;
                }

                if (!success) httpStatus = HttpStatusCode.InternalServerError;
            }

            return success;
        }

        public static bool Put(
            string key, 
            DomainConfiguration domainConfiguration, 
            string username,
            string siteId,
            string passwordHeader, 
            string customerJson, 
            out HttpStatusCode httpStatus, 
            out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            if (success)
            {
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/customers/" + username + "?applicationid=" + domainConfiguration.ApplicationId + "&siteId=" + siteId;

                    success = HttpHelper.RestCall
                    (
                        "Put", 
                        url, 
                        "Application/JSON", 
                        "Application/JSON",
                        new Dictionary<string, string>() { { "Authorization", passwordHeader } }, 
                        customerJson, 
                        true, 
                        out httpStatus,
                        out json
                    );

                    if (success) break;
                }

                if (!success) httpStatus = HttpStatusCode.InternalServerError;
            }

            return success;
        }

        public static bool Post(string key, DomainConfiguration domainConfiguration, string username, bool passwordReset, string passwordResetToken, string passwordHeader, string newPassword, string customerJson, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.InternalServerError;
            json = "";

            // Has the customer requested a password reset?
            if (passwordReset)
            {
                // Did the customer click on the email link?
                if (passwordResetToken != null && passwordResetToken.Length > 0)
                {
                    if (newPassword == null || newPassword.Length < 8)
                    {
                        Logger.Log.Error("CustomerServices.Post Invalid or missing new password: " + newPassword);
                        httpStatus = HttpStatusCode.BadRequest;
                        return false;
                    }
                    else
                    {
                        // Complete password reset
                        success = CustomerServices.CompleteResetPassword(key, domainConfiguration, username, passwordResetToken, newPassword, out httpStatus, out json);
                        if (!success) httpStatus = HttpStatusCode.InternalServerError;
                    }
                }
                else
                {
                    // Request password reset
                    success = CustomerServices.RequestResetPassword(key, domainConfiguration, username, out httpStatus, out json);
                    if (!success) httpStatus = HttpStatusCode.InternalServerError;
                }
            }
            else
            {            
                // Update customer details

                // Call the signpost server to get a list of ACS servers
                IEnumerable<string> serverUrls = null;
                success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/customers/" + username + "?applicationid=" + domainConfiguration.ApplicationId + "&newPassword=" + newPassword;

                    success = HttpHelper.RestCall
                    (
                        "Post",
                        url,
                        "Application/JSON",
                        "Application/JSON",
                        new Dictionary<string, string>() { { "Authorization", passwordHeader } },
                        customerJson,
                        true,
                        out httpStatus,
                        out json
                    );

                    if (success) break;
                }

                if (!success) httpStatus = HttpStatusCode.InternalServerError;
            }

            return success;
        }

        public static bool RequestResetPassword(string key, DomainConfiguration domainConfiguration, string username, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.OK;
            json = "";

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            if (success)
            {
                // Request a password reset
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/customers/" + HttpUtility.UrlEncode(username) + "/passwordresetrequest?applicationid=" + domainConfiguration.ApplicationId;
                    success = HttpHelper.RestCall(
                        "PUT",
                        url,
                        "Application/JSON",
                        "Application/JSON",
                        null,
                        "",
                        true,
                        out httpStatus,
                        out json);

                    if (success) break;
                }

                if (!success) httpStatus = HttpStatusCode.InternalServerError;
            }

            if (success)
            {
                if (httpStatus == HttpStatusCode.OK)
                {
                    ACSError acsError = null;
                    success = Helper.CheckForError(ref json, out acsError);

                    if (success)
                    {
                        if (acsError != null)
                        {
                            success = false;
                            if (!success) httpStatus = HttpStatusCode.InternalServerError;
                        }
                    }
                }
                else
                {
                    success = false;
                }
            }

            string token = "";
            if (success)
            {
                // Parse the response json
                JObject jObject = JObject.Parse(json);

                JToken errorJToken = jObject["errorCode"];
                if (errorJToken != null)
                {
                    // There was a problem
                    Logger.Log.Error("CustomerServices RequestResetPassword returned an error.  Json: " + json);

                    success = false;
                    if (!success) httpStatus = HttpStatusCode.InternalServerError;
                }
                else
                {
                    // Is the site still online?
                    token = (string)jObject["Token"];

                    success = true;
                }
            }

            if (success)
            {
                // Queue email to customer with password reset link
                success = CustomerServices.SendResetPasswordEmail(domainConfiguration, username, token);
            }

            return success;
        }

        private static bool SendResetPasswordEmail(DomainConfiguration domainConfiguration, string username, string passwordResetToken)
        {
            bool success = true;

            // Queue email to customer with password reset link
            Email passwordResetEmailTemplate = null;

            if (domainConfiguration.TemplateEmails != null &&
                domainConfiguration.TemplateEmails.Count > 0 &&
                domainConfiguration.TemplateEmails.TryGetValue("PasswordResetEmail", out passwordResetEmailTemplate))
            {
                string body = passwordResetEmailTemplate.Body
                    .Replace("{domain}", domainConfiguration.HostHeader)
                    .Replace("{passwordResetToken}", passwordResetToken)
                    .Replace("{username}", HttpUtility.UrlEncode(username));

                // Queue the email to be sent in the background
                BackgroundServices.QueueEmail
                (
                    new Email()
                    {
                        To = username,
                        From = passwordResetEmailTemplate.From,
                        Body = body,
                        Subject = passwordResetEmailTemplate.Subject,
                        HostHeader = domainConfiguration.HostHeader,
                        Server = passwordResetEmailTemplate.Server,
                        ServerType = passwordResetEmailTemplate.ServerType,
                        Username = passwordResetEmailTemplate.Username,
                        Password = passwordResetEmailTemplate.Password,
                        AttachmentFilename = null,
                        Attachment = null
                    }
                );

                Logger.Log.Info("CustomerServices.RequestResetPassword Email queued");
            }
            else
            {
                Logger.Log.Info("CustomerServices.RequestResetPassword Unable to send email - missing PasswordResetEmail email template");
            }

            return success;
        }

        public static bool CompleteResetPassword(string key, DomainConfiguration domainConfiguration, string username, string passwordResetToken, string newPassword, out HttpStatusCode httpStatus, out string json)
        {
            bool success = false;
            httpStatus = HttpStatusCode.OK;
            json = "";

            // Call the signpost server to get a list of ACS servers
            IEnumerable<string> serverUrls = null;
            success = HostServices.GetServerUrl(domainConfiguration, out httpStatus, out serverUrls);

            if (success)
            {
                // Update password in ACS
                // Try each ACS server
                foreach (string serverUrl in serverUrls)
                {
                    string url = serverUrl + "/customers/" + HttpUtility.UrlEncode(username) + "/passwordresetrequest?applicationid=" + domainConfiguration.ApplicationId;
                    string data = "{\"Token\":\"" + passwordResetToken + "\"}";
                    success = HttpHelper.RestCall(
                        "POST",
                        url,
                        "Application/JSON",
                        "Application/JSON",
                        new Dictionary<string, string>() { { "Authorization", newPassword } },
                        data,
                        true,
                        out httpStatus,
                        out json);

                    if (success) break;
                }

                if (!success) httpStatus = HttpStatusCode.InternalServerError;
            }

            if (success)
            {
                if (httpStatus == HttpStatusCode.OK)
                {
                    ACSError acsError = null;
                    if (Helper.CheckForError(ref json, out acsError))
                    {
                        if (success)
                        {
                            if (acsError != null)
                            {
                                success = false;
                                if (!success) httpStatus = HttpStatusCode.InternalServerError;
                            }
                        }
                    }
                }
            }

            return success;
        }
    }
}