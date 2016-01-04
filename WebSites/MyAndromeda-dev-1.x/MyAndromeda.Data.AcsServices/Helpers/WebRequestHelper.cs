using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Core;

namespace MyAndromeda.Data.AcsServices.Helpers
{
    public static class WebRequestHelper
    {
        public static Task<string> GrabARequestAsync(string url, IDelay timeout, ResponseType requestType = ResponseType.Json)
        {
            var client = new MyAndromedaWebClient(timeout);

            client.Headers.Add("User-Agent", "Nobody");

            client.Encoding = Encoding.UTF8;

            if (requestType == ResponseType.Xml)
            {
                client.Headers.Add(HttpRequestHeader.Accept, "application/xml");
            }
            else
            {
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
            }

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            return client.DownloadStringTaskAsync(url);
        }

        public static Task<byte[]> Post(string url, IDelay timeout, ResponseType requestType = ResponseType.Json) 
        {
            var client = new MyAndromedaWebClient(timeout);

            client.Headers.Add("User-Agent", "Nobody");

            client.Encoding = Encoding.UTF8;

            if (requestType == ResponseType.Xml)
            {
                client.Headers.Add(HttpRequestHeader.Accept, "application/xml");
            }
            else
            {
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
            }

            var post = System.Text.Encoding.ASCII.GetBytes("");
            return client.UploadDataTaskAsync(url, "POST", post);

        }
    }
}
