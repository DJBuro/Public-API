using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using System.Monads;
using MyAndromeda.Data.AcsServices.Helpers;

namespace MyAndromeda.Data.AcsServices.Services
{
    public class MenuWebServiceDataAccess : IMenuWebServiceDataAccess
    {
        private readonly IDelay delay;

        public MenuWebServiceDataAccess(IDelay delay) 
        {
            this.delay = delay;
        }

        public async Task<string> FetchFromServiceAsync(IEnumerable<string> serverEndpoints)
        {
            serverEndpoints.CheckNull("serverAddreses");
            serverEndpoints.Check((a) => a.Count() > 0, _ => { 
                throw new ArgumentException("No server addressees provided", "serverAddreses"); 
            });

            var queryTask = await this.QueryServicesInOrder(serverEndpoints);
            
            return queryTask;
        }

        private async Task<string> QueryServicesInOrder(IEnumerable<string> urls)
        {
            foreach (var endpoint in urls) 
            { 
                var requestTask = WebRequestHelper.GrabARequestAsync(endpoint, this.delay, MyAndromeda.Data.AcsServices.Models.ResponseType.Json);
                var result = await requestTask;

                if (requestTask.Exception != null) { continue; }

                return result;
            }

            return string.Empty;
        }
    }
}


