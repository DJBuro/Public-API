using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Andromeda.WebOrdering.Model;

namespace Andromeda.WebOrdering.Services
{
    public class Alerts
    {
        internal static void SendAlert(Email emailTemplate, string hostHeader, string subject, string body, string attachmentFilename, string attachment)
        {
            Global.Log.Info("Alerts.SendAlert");

            // Do we need to send an alert email?
            if (emailTemplate.To != null && emailTemplate.To.Length > 0)
            {
                // Queue the email to be sent in the background
                BackgroundServices.QueueEmail
                (
                    new Email()
                    {
                        To = emailTemplate.To,
                        From = emailTemplate.From,
                        Body = body,
                        Subject = subject,
                        HostHeader = hostHeader,
                        Server = emailTemplate.Server,
                        ServerType = emailTemplate.ServerType,
                        Username = emailTemplate.Username,
                        Password = emailTemplate.Password,
                        AttachmentFilename = attachmentFilename,
                        Attachment = attachment
                    }
                );

                Global.Log.Info("Alerts.SendAlert Email queued");
            }
            else
            {
                Global.Log.Info("Alerts.SendAlert Unable to send email - missing alertemail setting");
            }
        }

        internal static void SendFeedback(Email emailTemplate, string hostHeader, string subject, string body, string attachment)
        {
            Global.Log.Info("Alerts.SendFeedback");

            // Do we need to send a feedback email?
            if (emailTemplate.To != null && emailTemplate.To.Length > 0)
            {
                // Queue the email to be sent in the background
                BackgroundServices.QueueEmail
                (
                    new Email()
                    {
                        To = emailTemplate.To,
                        From = emailTemplate.From,
                        Body = body,
                        Subject = subject,
                        HostHeader = hostHeader,
                        Server = emailTemplate.Server,
                        ServerType = emailTemplate.ServerType,
                        Username = emailTemplate.Username,
                        Password = emailTemplate.Password,
                        Attachment = attachment
                    }
                );

                Global.Log.Info("Alerts.SendFeedback Email queued");
            }
            else
            {
                Global.Log.Info("Alerts.SendFeedback Unable to send email - missing feedbackemail setting");
            }
        }
    }
}