using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace DemoTrackingService
{
    internal class HttpHelper
    {
        public static string HttpGet(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            webRequest.PreAuthenticate = true;
            webRequest.Accept = "application/xml";
            webRequest.Timeout = 2000;
            webRequest.ReadWriteTimeout = 2000;
            webRequest.AllowAutoRedirect = false;

            StreamReader readStream = null;
            WebResponse webResponse = null;

            string responseText = "";

            try
            {
                // Submit the request to the server
                webResponse = webRequest.GetResponse();

                // Get the http response
                Stream receiveStream = webResponse.GetResponseStream();

                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                // Pipe the stream to a higher level stream reader with the required encoding format. 
                readStream = new StreamReader(receiveStream, encode);

                responseText = readStream.ReadToEnd();
            }
            catch (Exception exception)
            {
                //               Log.LogEvent(Global.LogName, "LivePepper HTTP REST GET failed", EventTypeEnum.Error, exception);
            }
            finally
            {
                // Is the http response stream open?
                if (readStream != null)
                {
                    // Yes.  Try and close it, ignoring any errors
                    try
                    {
                        readStream.Close();
                    }
                    catch { }
                }

                // Is the web response open?
                if (webResponse != null)
                {
                    // Yes.  Try and close it, ignoring any errors
                    try
                    {
                        webResponse.Close();
                    }
                    catch { }
                }
            }

            return responseText;
        }
    }
}