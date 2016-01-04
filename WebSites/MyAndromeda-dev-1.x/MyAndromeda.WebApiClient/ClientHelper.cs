using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.WebApiClient
{
    public static class ClientHelper
    {
        public static HttpClient GetANewJsonClient(Uri fullApiAddress) 
        {
            var client = new HttpClient();

            client.BaseAddress = fullApiAddress;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            return client;
        }
    }
}
