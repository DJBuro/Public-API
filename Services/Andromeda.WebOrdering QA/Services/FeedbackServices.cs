using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Andromeda.WebOrdering.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Configuration;

namespace Andromeda.WebOrdering.Services
{
    public class FeedbackServices
    {
        public static bool Put
        (
            string siteId,
            string key, 
            DomainConfiguration domainConfiguration, 
            string feedbackJson, 
            out HttpStatusCode httpStatus, 
            out string json
        )
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
                    string url = "";

                    if (siteId.Length > 0)
                    {
                        url = serverUrl + "/sites/" + siteId + "/feedback?applicationid=" + domainConfiguration.ApplicationId;
                    }
                    else
                    {
                        url = serverUrl + "/feedback?applicationid=" + domainConfiguration.ApplicationId;
                    }

                    success = HttpHelper.RestCall
                    (
                        "Put", 
                        url, 
                        "Application/JSON", 
                        "Application/JSON",
                        null,
                        feedbackJson, 
                        true, 
                        out httpStatus,
                        out json
                    );

                    if (success) break;
                }

                if (!success) httpStatus = HttpStatusCode.InternalServerError;

                FeedbackServices.SendEmail(domainConfiguration, feedbackJson);
            }

            return success;
        }

        private static void SendEmail(DomainConfiguration domainConfiguration, string feedbackJson)
        {
            string username = ConfigurationManager.AppSettings["feedbackEmailUsername"];
            if (username == null || username.Length == 0) return;

            string password = ConfigurationManager.AppSettings["feedbackEmailPassword"];
            if (password == null || password.Length == 0) return;

            string server = ConfigurationManager.AppSettings["feedbackEmailServer"];
            if (server == null || server.Length == 0) return;

            string emailTo = ConfigurationManager.AppSettings["feedbackEmailTo"];
            if (emailTo == null || emailTo.Length == 0) return;

            string subject = ConfigurationManager.AppSettings["feedbackEmailSubject"];
            if (subject == null || subject.Length == 0) return;

            string body = ConfigurationManager.AppSettings["feedbackEmailBody"];
            if (body == null || body.Length == 0) return;

            // Deserialize the feedback
            Feedback feedback = JsonConvert.DeserializeObject<Feedback>(feedbackJson);

            // This is the template email - it doesn't contain the actual final email text
            Email templateEmail = new Email()
            {
                From = "Onlineordering@androtech.com",
                Password = password,
                Server = server,
                ServerType = "EXCHANGE",
                To = emailTo,
                Username = username
            };

            // Build the email body
            subject = FeedbackServices.InjectValues(domainConfiguration, subject, feedback);
            body = FeedbackServices.InjectValues(domainConfiguration, body, feedback);

            // Send the alert 
            Alerts.SendAlert(
                templateEmail,
                domainConfiguration.HostHeader,
                subject,
                body,
                null,
                null);
        }

        private static string InjectValues(DomainConfiguration domainConfiguration, string input, Feedback feedback)
        {
            string output = input.Replace("{WEBSITE}", domainConfiguration.HostHeader);
            output = output.Replace("{STORENAME}", feedback.StoreName);
            output = output.Replace("{CUSTOMERNAME}", feedback.Name);
            output = output.Replace("{EMAIL}", feedback.Email);
            output = output.Replace("{FEEDBACKCATEGORY}", feedback.FeedbackCategoryName);
            output = output.Replace("{FEEDBACK}", feedback.FeedbackText);

            return output;
        }
    }
}