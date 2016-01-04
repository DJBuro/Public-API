using System;
using System.Configuration;
using System.Linq;
using MyAndromeda.Core;

namespace MyAndromeda.Services.WebHooks
{
    public class WebhookEndpointManger : ISingletonDependency
    {
        private const string EdtEndpoint = "WebHooks.EdtEndpoint";
        private const string StoreStatusEndpoint = "WebHooks.StoreStatusEndpoint";
        private const string MenuVersionEndpoint = "WebHooks.MenuVersionEndpoint";
        private const string MenuItemsChangedEndpoint = "WebHooks.MenuItemsChangedEndpoint";

        private const string OrderStatusEndpoint = "WebHooks.OrderStatusEndpoint";

        private const string BringgEndpointKey = "Webhooks.BringEndpoint";
        private const string BringgEtaEndpointKey = "Webhooks.BringEtaEndpoint";

        private string edt;
        public string Edt
        {
            get
            {
                string value = this.edt ?? (this.edt = ConfigurationManager.AppSettings[EdtEndpoint]);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException(EdtEndpoint, new Exception("Is missing from the config file!"));
                }

                return value;
            }
            set
            {
                this.edt = value;
            }
        }

        private string orderStatus;
        public string OrderStatus
        {
            get
            {
                string value = this.orderStatus ?? (this.orderStatus = ConfigurationManager.AppSettings[OrderStatusEndpoint]);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException(OrderStatusEndpoint, new Exception(OrderStatusEndpoint + "Is missing from the config file!"));
                }

                return value;
            }
            set
            {
                this.orderStatus = value;
            }
        }

        private string storeStatus;
        public string StoreStatus
        {
            get 
            {
                string value = this.storeStatus ?? (this.storeStatus = ConfigurationManager.AppSettings[StoreStatusEndpoint]);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException(StoreStatusEndpoint, new Exception(StoreStatusEndpoint + " Is missing from the config file!"));
                }

                return value;
            }
            set
            {
                this.storeStatus = value;
            }
        }

        private string menuVersion;
        public string MenuVersion
        {
            get 
            {
                string value = this.menuVersion ?? (this.menuVersion = ConfigurationManager.AppSettings[MenuVersionEndpoint]);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException(MenuVersionEndpoint, new Exception(MenuVersionEndpoint+ " Is missing from the config file!"));
                }

                return value;
            }
            set
            {
                this.menuVersion = value;
            }
        }

        private string menuItemsChanged;
        public string MenuItemsChanged
        {
            get
            {
                string value = this.menuItemsChanged ?? (this.menuItemsChanged = ConfigurationManager.AppSettings[MenuItemsChangedEndpoint]);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException(MenuItemsChangedEndpoint, new Exception(MenuItemsChangedEndpoint + " Is missing from the config file!"));
                }

                return value;
            }
            set
            {
                this.menuVersion = value;
            }
        }

        private string bringgWebhookEndpoint;
        public string BringgEndpoint
        {
            get
            {
                var value = this.bringgWebhookEndpoint ?? (this.bringgWebhookEndpoint = System.Configuration.ConfigurationManager.AppSettings[BringgEndpointKey]);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException(BringgEndpointKey, new Exception("Is missing from the config file!"));
                }

                return value;
            }
        }

        //private string orderStatusChangeEndpoint;
        //public string WebhookOrderStatusChangeEndpoint
        //{
        //    get
        //    {
        //        var value = this.bringgWebhookEndpoint ?? (this.bringgWebhookEndpoint = System.Configuration.ConfigurationManager.AppSettings[WebHookOrderStatusChangeKey]);

        //        if (string.IsNullOrWhiteSpace(value))
        //        {
        //            throw new NullReferenceException(WebHookOrderStatusChangeKey, new Exception("Is missing from the config file!"));
        //        }

        //        return value;
        //    }
        //}
    }
}