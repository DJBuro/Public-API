using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace MyAndromeda.SendGridService.Recipients
{

    public class SendGridTools
    {
        private readonly ISendGridEmailSettings sendGridEmailSettings;

        public SendGridTools(ISendGridEmailSettings sendGridEmailSettings)
        {
            this.sendGridEmailSettings = sendGridEmailSettings;
        }


        //public IEnumerable<object> List() 
        //{
        //    string url = "https://api.sendgrid.com/api/newsletter/lists/get.json"
        //}

        /// <summary>
        /// Remove an email from a distribution list
        /// </summary>
        /// <param name="EmaiLAddress">User's Email Address</param>
        /// <param name="DistributionList">Name of Distribution List</param>
        /// <returns></returns>
        public string DeleteEmailFromList(string emaiLAddress, string distributionList)
        {
            string encodedData = HttpContext.Current.Server.UrlEncode(emaiLAddress);
            string url = "http://sendgrid.com/api/newsletter/lists/email/delete.json?list=" + distributionList + 
                "&email=" + encodedData + 
                "&api_user=" + this.sendGridEmailSettings.UserName + 
                "&api_key=" + this.sendGridEmailSettings.Password;

            return PerformHttpGet(url);
        }

        /// <summary>
        /// Add an email from a distribution list
        /// </summary>
        /// <param name="EmaiLAddress">User's Email Address</param>
        /// <param name="DistributionList">Name of Distribution List</param>
        /// <returns>Results log</returns>
        public string AddEmailToList(string emailAddress, string name, string distributionList)
        {
            string encodedData = "{\"email\":\"" + emailAddress + "\",\"name\":\"" + name + "\"}";
            encodedData = HttpContext.Current.Server.UrlEncode(encodedData);
            string url = "http://sendgrid.com/api/newsletter/lists/email/add.json?list=" + distributionList + 
                "&data=" + encodedData + 
                "&api_user=" + this.sendGridEmailSettings.UserName + 
                "&api_key=" + this.sendGridEmailSettings.Password;

            return PerformHttpGet(url);
        }

        /// <summary>
        /// Send an email to a distribution list
        /// </summary>
        /// <param name="FromName">The SendGrid Newsletter "From Name" You Wish to Send From</param>
        /// <param name="NewsletterTitle">Subject of the Message</param>
        /// <param name="NewsletterHTML">HTML Body of the Message</param>
        /// <param name="SendGridDistributionList">Name of Distribution List</param>
        /// <returns>Results log</returns>
        public string SendNewsletterToList(string fromName, string newsletterTitle, string newsletterHtml, string sendGridDistributionList)
        {

            string encodedNewsletterName = HttpContext.Current.Server.UrlEncode(newsletterTitle + " - " + DateTime.Today.ToString("d")); //append date to deal with duplicate subject lines
            string encodedNewsletterSubject = HttpContext.Current.Server.UrlEncode(newsletterTitle);
            string encodedNewletterHtml = HttpContext.Current.Server.UrlEncode(newsletterHtml);

            string resultsHtml = "";

            //create newsletter and publish to send grid
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://sendgrid.com/api/newsletter/add.json");
            ASCIIEncoding encoding = new ASCIIEncoding();

            string postData = "identity=" + fromName;
            postData += "&name=" + encodedNewsletterName;
            postData += "&subject=" + encodedNewsletterSubject;
            postData += "&html=" + encodedNewletterHtml;
            postData += "&api_user=" + this.sendGridEmailSettings.UserName;
            postData += "&api_key=" + this.sendGridEmailSettings.Password;
            postData += "&data=";

            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            resultsHtml += "Creating Newsletter: " + responseString + "<br/>";

            //assign list to newsletter (i.e. this message goes to this list of recipients)
            string url = "http://sendgrid.com/api/newsletter/recipients/add.json?name=" + encodedNewsletterName + 
                "&list=" + sendGridDistributionList + 
                "&api_user=" + this.sendGridEmailSettings.UserName + 
                "&api_key=" + this.sendGridEmailSettings.Password;

            string sendGridResponse = PerformHttpGet(url);
            resultsHtml += "Assigning Newsletter to List: " + sendGridResponse + "<br/>";

            //schedule newsletter message to be send immediately. There are additional parameters that can be added if you want to schedule your newsletter in a future date
            url = "http://sendgrid.com/api/newsletter/schedule/add.json?name=" + encodedNewsletterName + 
                "&api_user=" + this.sendGridEmailSettings.UserName + 
                "&api_key=" + this.sendGridEmailSettings.Password;

            sendGridResponse = PerformHttpGet(url);
            resultsHtml += "Scheduling Newsletter: " + sendGridResponse + "<br/>";

            return resultsHtml;
        }

        /// <summary>
        /// Delete a distribution list
        /// </summary>
        /// <param name="DistributionList">Name of distribution list</param>
        /// <returns></returns>
        public string DeleteDistributionList(string distributionList)
        {
            string resultsHtml = "";
            string url = "http://sendgrid.com/api/newsletter/lists/delete.json?list=" + distributionList + 
                "&api_user=" + this.sendGridEmailSettings.UserName + 
                "&api_key=" + this.sendGridEmailSettings.Password;

            string sendGridResponse = PerformHttpGet(url);
            resultsHtml += "Deleting List: " + sendGridResponse + "<br/>";
            return resultsHtml;
        }

        /// <summary>
        /// Create a distribution list
        /// </summary>
        /// <param name="DistributionList">Name of distribution list</param>
        /// <returns></returns>
        public string CreateDistributionList(string distributionList)
        {
            string resultsHtml = "";
            string url = "http://sendgrid.com/api/newsletter/lists/add.json?list=" + distributionList + 
                "&api_user=" + this.sendGridEmailSettings.UserName + 
                "&api_key=" + this.sendGridEmailSettings.Password;

            string sendGridResponse = PerformHttpGet(url);
            resultsHtml += "Deleting List: " + sendGridResponse + "<br/>";
            return resultsHtml;
        }

        /// <summary>
        /// Used to add multiple email addresses to a list. 
        /// </summary>
        /// <param name="ListName">Name of distribution list</param>
        /// <param name="EmailAddresses">Email addresses to add (string array)</param>
        /// <returns></returns>
        public string AddMultipleEmailstoList(string listName, string[] emailAddresses)
        {
            // This has been tested on an array with 50,000 recipients. It works well.

            string resultsHtml = ""; 
            string encodedData = "";

            for (int x = 0; x < emailAddresses.Length; x++)
            {
                string emailAddress = emailAddresses[x];

                if (IsValidEmail(emailAddress))
                {
                    encodedData += "&data[]=" + HttpContext.Current.Server.UrlEncode(" {\"email\":\"" + emailAddress + "\",\"name\":\"\"}");
                }

                if (x % 1000 == 0 || x == emailAddresses.Length - 1) //break the requests up into blocks of 1,000 email addresses. 
                {

                    try
                    {
                        HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://sendgrid.com/api/newsletter/lists/email/add.json?list=");
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        
                        string postData = "list=" + listName;
                        postData += encodedData;
                        postData += "&api_user=" + this.sendGridEmailSettings.UserName;
                        postData += "&api_key=" + this.sendGridEmailSettings.Password;
                        byte[] data = encoding.GetBytes(postData);
                        httpWReq.Method = "POST";
                        httpWReq.ContentType = "application/x-www-form-urlencoded";
                        httpWReq.ContentLength = data.Length;
                        
                        using (Stream stream = httpWReq.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                        string sendGridResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        resultsHtml += "Adding Emails (at " + x.ToString() + "): " + sendGridResponse + "<br/>";
                    }
                    catch (Exception ex)
                    {
                        resultsHtml += "Error Adding Emails (at " + x.ToString() + "): " + ex.ToString() + "<br/>";
                    }
                    encodedData = "";
                }

            }



            return resultsHtml;
        }

        /// <summary>
        /// Perform an HTTP Get Request and return results
        /// </summary>
        private string PerformHttpGet(string url)
        {
            try
            {
                // Open a connection
                HttpWebRequest webRequestObject = (HttpWebRequest)HttpWebRequest.Create(url);

                // You can also specify additional header values like 
                // the user agent or the referer:
                webRequestObject.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_5; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.231 Safari/534.10";
                webRequestObject.Referer = "";

                // Request response:
                WebResponse response = webRequestObject.GetResponse();

                // Open data stream:
                Stream webStream = response.GetResponseStream();

                // Create reader object:
                StreamReader reader = new StreamReader(webStream, System.Text.Encoding.Default);

                // Read the entire stream content:
                string pageContent = reader.ReadToEnd();

                // Cleanup
                reader.Close();
                webStream.Close();
                response.Close();

                return pageContent;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Determine if an email address is valid
        /// </summary>
        private static bool IsValidEmail(string emailaddress)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(strRegex);
            if (regex.IsMatch(emailaddress))
                return (true);
            else
                return (false);
        }

    }
}
