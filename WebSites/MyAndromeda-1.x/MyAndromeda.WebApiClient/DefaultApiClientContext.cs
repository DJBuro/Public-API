using System;

namespace MyAndromeda.WebApiClient
{
    public class DefaultApiClientContext : IWebApiClientContext
    {
        private const string BaseAddressKey = "MyAndromeda.WebApiClient.BaseAddress";
        private const string SyncingMenuEndpointKey = "MyAndromeda.WebApiClient.SyncingMenuEndpoint";

        public DefaultApiClientContext() 
        {
        }

        private string baseAddress; 

        public string BaseAddress
        {
            get 
            {
                var value = this.baseAddress ?? (this.baseAddress = System.Configuration.ConfigurationManager.AppSettings[BaseAddressKey]);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException(BaseAddressKey, new Exception("Is missing from the config file!"));
                }

                return value;
            }
            set 
            {
                this.baseAddress = value;
            }
        }

        private string syncingMenuEndpoint;

        public string SyncingMenuEndpoint
        {
            get
            {
                var value = this.syncingMenuEndpoint ?? (this.syncingMenuEndpoint = System.Configuration.ConfigurationManager.AppSettings[SyncingMenuEndpointKey]);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException(SyncingMenuEndpointKey, new Exception("Is missing from the config file!"));
                }

                return value;
            }
            set
            {
                this.syncingMenuEndpoint = value;
            }
        }


    }
}