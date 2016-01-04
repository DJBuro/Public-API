using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AndroCloudServices.Helper
{
    public class RestCallResult
    {
        public bool Success { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public string ResponseData { get; set; }
    }

    public class HttpHelper
    {
        /// <summary>
        /// Makes an HTTP call
        /// </summary>
        /// <param name="httpMethod">The HTTP method to use (GET, POST etc...)</param>
        /// <param name="url">The url to call</param>
        /// <param name="contentType">The type of content in the request</param>
        /// <param name="accepts">The type of content we want the server to return</param>
        /// <param name="requestData">The data to send to the server</param>
        /// <param name="responseData">The data returned by the server</param>
        /// <returns></returns>
        public async static Task<RestCallResult> RestCall
        (
            string httpMethod,
            string url,
            string contentType,
            string accepts,
            Dictionary<string, string> headers,
            string requestData
        )
        {
            RestCallResult restCallResult = new Helper.RestCallResult()
            {
                Success = false,
                HttpStatus = HttpStatusCode.InternalServerError,
                ResponseData = ""
            };

            // The request and response data will be UTF-8
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(requestData);

            // Accept invalid SSL certs
            ServicePointManager.ServerCertificateValidationCallback = delegate
            {
                return true;
            };

            // The REST service to call
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            webRequest.Method = httpMethod;
            webRequest.PreAuthenticate = true;
            webRequest.ContentType = contentType;
            webRequest.Accept = accepts;
            webRequest.ContentLength = data.Length;
            webRequest.Timeout = 60000;
            webRequest.ReadWriteTimeout = 60000;
            webRequest.AllowAutoRedirect = false;
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    webRequest.Headers.Add(header.Key, header.Value);
                }
            }

            try
            {
                // Do we need to send any data to the server?
                if (data.Length > 0)
                {
                    // Open a connection to the server and stream the data to it
                    using (Stream contentStream = webRequest.GetRequestStream())
                    {
                        contentStream.Write(data, 0, data.Length);
                        contentStream.Close();
                    }
                }

                // Submit the request to the server and get the response
                using (WebResponse webResponse = (HttpWebResponse)await webRequest.GetResponseAsync())
                {
                    restCallResult.HttpStatus = ((HttpWebResponse)webResponse).StatusCode;

                    // Get the http response
                    Stream stream = webResponse.GetResponseStream();

                    // Get the response data
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        restCallResult.ResponseData = reader.ReadToEnd();
                    }
                }

                // Success!
                restCallResult.Success = true;
            }
            catch (WebException webException)
            {
                // Is there a response with the exception?
                if (webException.Response != null)
                {
                    // Did the server return any data with the exception
                    if (webException.Response.ContentLength != 0)
                    {
                        // Get the response
                        using (Stream stream = webException.Response.GetResponseStream())
                        {
                            // Get the response data
                            using (StreamReader reader = new StreamReader(stream, encoding))
                            {
                                restCallResult.ResponseData = reader.ReadToEnd();
                            }
                        }
                    }
                }

                // Logging
                string statusCodeText = "";
                if (webException.Response is HttpWebResponse)
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse)webException.Response;
                    restCallResult.HttpStatus = httpWebResponse.StatusCode;
                    statusCodeText = "status code=" + ((int)restCallResult.HttpStatus).ToString() + " ";
                }

                // Failed
                restCallResult.Success = false;
                restCallResult.ResponseData = "ACS error '" + statusCodeText + "' response=" + restCallResult.ResponseData;
            }

            return restCallResult;
        }
    }
}
