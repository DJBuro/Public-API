using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Net.Mail;
using System.Net;
using Microsoft.Exchange.WebServices.Data;
using System.Text;
using Andromeda.WebOrdering.Services;

namespace Andromeda.WebOrdering
{
    internal class BackgroundServices
    {
        private static int TimerFrequency = 0;
        private static Timer Timer = null;

        private static readonly ConcurrentQueue<Email> EmailQueue = new ConcurrentQueue<Email>();

        // Queue of websites that need to have static files generated
        public static readonly ConcurrentDictionary<string, WebSiteServicesDataWrapper> GenerateStaticContentQueue = 
            new ConcurrentDictionary<string, WebSiteServicesDataWrapper>();

        internal static void QueueEmail(Email email)
        {
            BackgroundServices.EmailQueue.Enqueue(email);
        }

        internal static void QueueStaticContent(WebSiteServicesDataWrapper webSiteServicesDataWrapper)
        {
            BackgroundServices.GenerateStaticContentQueue.TryAdd(webSiteServicesDataWrapper.domainName, webSiteServicesDataWrapper);
        }

        internal static void StartAsync()
        {
            try
            {
                Logger.Log.Debug("BackgroundServices StartAsync");

                // Get the timer frequency from the settings
                string serviceTimerFrequency = "";
                try
                {
                    serviceTimerFrequency = ConfigurationManager.AppSettings["BackgroundServicesInterval"];
                }
                catch (Exception exception)
                {
                    Logger.Log.Error("Missing setting: BackgroundServicesInterval", exception);
                    return;
                }

                int timerFrequency = 0;

                // Did we get the timer frequency from the settings?
                if (serviceTimerFrequency == null || serviceTimerFrequency.Length == 0)
                {
                    // No.
                    Logger.Log.Error("Missing setting: BackgroundServicesInterval");
                    return;
                }
                // Is it a valid int?
                else if (!int.TryParse(serviceTimerFrequency, out timerFrequency))
                {
                    // No.
                    Logger.Log.Error("Invalid setting: BackgroundServicesInterval=" + serviceTimerFrequency);
                    return;
                }
                else
                {
                    Logger.Log.Info("BackgroundServices StartAsync timerFrequency=" + timerFrequency);

                    // We've Got a valid timer frequency
                    // Store the timer frequency for later as we will stop and start the timer
                    BackgroundServices.TimerFrequency = timerFrequency;

                    // Set which method gets called when the timer fires
                    TimerCallback timerDelegate = new TimerCallback(OnTimerFired);

                    // Start the timer
                    BackgroundServices.Timer = new Timer(timerDelegate, null, BackgroundServices.TimerFrequency, 0);
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Error("Unhandled exception in BackgroundServicesInterval", exception);
            }
        }

        private static void OnTimerFired(Object stateInfo)
        {
            Console.WriteLine("timer fired");

            // Note that this method is called by a random thread from the thread pool.
            // Also, the timer only fires once so we must restart it once we've finished processing.
            // This is by design to ensure that the timer doesn't fire again before we've
            // finished processing.

            try
            {
                ProcessEmails();
                ProcessStaticContent();
            }
            catch (Exception exception)
            {
                Logger.Log.Error("OnTimerFired: Unhandled exception", exception);
            }
            finally
            {
                // Wait for a bit and see if there are any new emails to send
                BackgroundServices.StartTimer();
            }
        }

        private static void StartTimer()
        {
            if (BackgroundServices.Timer != null)
            {
                lock (BackgroundServices.Timer)
                {
                    BackgroundServices.Timer.Change(BackgroundServices.TimerFrequency, 0);
                }
            }
        }

        private static void StopTimer(bool dispose)
        {
            if (BackgroundServices.Timer != null)
            {
                lock (BackgroundServices.Timer)
                {
                    // Stop the timer
                    BackgroundServices.Timer.Change(Timeout.Infinite, Timeout.Infinite);

                    if (dispose)
                    {
                        // Release timer resources
                        BackgroundServices.Timer.Dispose();
                        BackgroundServices.Timer = null;
                    }
                }
            }
        }

        private static void ProcessEmails()
        {
            // Send emails
            Email email = null;
            if (BackgroundServices.EmailQueue.TryDequeue(out email))
            {
                Logger.Log.Info("OnTimerFired: Processing email");

                //  Get the email addresses to send the alert email to
                string[] alertEmails = email.To.Split(',');

                if (email.ServerType == "EXCHANGE")
                {
                    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                    service.Url = new Uri(email.Server);
                    service.Credentials = new NetworkCredential(email.Username, email.Password);

                    EmailMessage mailMessage = new EmailMessage(service)
                    {
                        Subject = email.Subject,
                        Body = new MessageBody(BodyType.HTML, email.Body),
                        From = new EmailAddress(email.From),
                        Importance = Importance.High
                    };

                    // Do we need to add an attachment?
                    if (email.AttachmentFilename != null && email.Attachment != null)
                    {
                        mailMessage.Attachments.AddFileAttachment(email.AttachmentFilename, Encoding.UTF8.GetBytes(email.Attachment));
                    }

                    foreach (string alertEmail in alertEmails)
                    {
                        mailMessage.ToRecipients.Add(alertEmail, alertEmail);
                    }

                    try
                    {
                        mailMessage.SendAndSaveCopy();

                        Logger.Log.Debug("OnTimerFired: successfully sent email to " + email.To);
                    }
                    catch (Exception exception)
                    {
                        Logger.Log.Error("OnTimerFired: Unhandled exception sending email", exception);
                    }
                }
                else
                {
                    SmtpClient client = new SmtpClient(email.Server);
                    client.Credentials = new NetworkCredential(email.Username, email.Password);

                    // Send an email to the customer (the store/chain not the end customer) notifying them that a manual refund is needed
                    MailMessage mailMessage = new MailMessage()
                    {
                        IsBodyHtml = true,
                        Priority = MailPriority.High,
                        Subject = email.Subject,
                        Body = email.Body,
                        From = new MailAddress(email.From)
                    };

                    try
                    {
                        client.Send(mailMessage);
                    }
                    catch (Exception exception)
                    {
                        Logger.Log.Error("OnTimerFired: Unhandled exception sending email", exception);
                    }
                }
            }
        }

        private static void ProcessStaticContent()
        {
            WebSiteServicesData websiteSettingsToProcess = null;
            try
            {
                // Process each website in the queue
                foreach (var webSiteServicesDataWrapper in BackgroundServices.GenerateStaticContentQueue.Values)
                {
                    websiteSettingsToProcess = webSiteServicesDataWrapper.webSiteServicesData;

                    if (System.DateTime.Now >= webSiteServicesDataWrapper.deQueueTime)
                    {
                        Logger.Log.Debug("OnTimerFired:Started processing domain (static content): " + webSiteServicesDataWrapper.webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName);
                        
                        WebSiteServicesDataWrapper dummy = null;
                        bool doProcessing = false;

                        // Make sure only we can change the list of files that are currently being published
                        lock (WebSiteServices.publishingCurrently)
                        {
                            if (WebSiteServices.publishingCurrently.ContainsKey(webSiteServicesDataWrapper.domainName))
                            {
                                // The website is already being published - this shouldn't be possible!
                                Logger.Log.Debug("OnTimerFired:Unable to generate static content (locked) : " + webSiteServicesDataWrapper.webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName);
                            }
                            else
                            {
                                // Make sure MyAndromeda can't publish this website while we're generating the static files for it
                                // A website publish would delete all files and folders while we're working on them
                                doProcessing = true;
                                WebSiteServices.publishingCurrently.Add(webSiteServicesDataWrapper.domainName, webSiteServicesDataWrapper);
                            }
                        }

                        if (doProcessing)
                        {
                            // We're generating static files for the website now so remove it from the queue
                            if (BackgroundServices.GenerateStaticContentQueue.TryRemove(webSiteServicesDataWrapper.domainName, out dummy))
                            {
                                Logger.Log.Info("OnTimerFired: Processing static content for domain: " + webSiteServicesDataWrapper.webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName);
                                try
                                {
                                    // Generate static files
                                    StaticPageGenerator.GenerateStoreSnapshot(webSiteServicesDataWrapper.webSiteServicesData, (IStaticPageGenerator)new StaticPageGenerator());

                                    Logger.Log.Debug("OnTimerFired:domain processed Successfully (static content): " + webSiteServicesDataWrapper.webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Error("OnTimerFired: Unhandled exception generating static content for domain: "
                                        + webSiteServicesDataWrapper.webSiteServicesData == null ? string.Empty : webSiteServicesDataWrapper.webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName, ex);
                                }
                                finally
                                {
                                    // We're no longer generating static files
                                    Logger.Log.Debug("OnTimerFired:Removing domain from the dictionary: " + webSiteServicesDataWrapper.webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName);
                                    WebSiteServices.publishingCurrently.Remove(webSiteServicesDataWrapper.webSiteServicesData.WebSiteConfiguration.SiteDetails.DomainName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("OnTimerFired: Unhandled exception generating static content for domain: "
                    + websiteSettingsToProcess == null ? string.Empty : websiteSettingsToProcess.WebSiteConfiguration.SiteDetails.DomainName, ex);
            }
        }
    }
}