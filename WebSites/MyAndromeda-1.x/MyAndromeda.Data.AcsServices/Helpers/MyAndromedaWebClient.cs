using System;
using System.Net;
using MyAndromeda.Core;

namespace MyAndromeda.Data.AcsServices.Helpers
{
    public class MyAndromedaWebClient : WebClient, IDependency
    {
        private readonly IDelay timeout;

        public MyAndromedaWebClient(IDelay timeout) : base()
        {
            this.timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);
              
            webRequest.Timeout = timeout.DelayInMilliseconds;
              
            return webRequest;
        }
    }
}