using MyAndromeda.Core;
using System;
using System.Linq;

namespace MyAndromeda.Web.Services
{
    public class ServiceDelay : IDelay 
    {
        private readonly int serviceTimeout;
        public ServiceDelay() 
        {
            var appSettings = new System.Configuration.AppSettingsReader();
            var serviceTimeout = appSettings.GetValue("ServiceTimeout", typeof(int));

            this.serviceTimeout = serviceTimeout == null ? 5000 : int.Parse(serviceTimeout.ToString());
        }

        public int DelayInMilliseconds
        {
            get { return this.serviceTimeout;  }
        }

        public int DelayInSeconds
        {
            get { return Convert.ToInt32(Math.Ceiling((double)this.serviceTimeout / 1000)); }
        }
    }
}