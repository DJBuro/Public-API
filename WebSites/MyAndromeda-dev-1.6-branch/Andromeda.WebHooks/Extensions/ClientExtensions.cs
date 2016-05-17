using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.WebHooks.Extensions
{
    public static class ClientExtensions
    {
        public static HttpClient AddJson(this HttpClient client) 
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            return client;
        }

        public static HttpClient AddAuth(this HttpClient client) 
        {
            client.DefaultRequestHeaders.Add("MyAuthorization", "Hi");

            return client;
        }
    }
}
