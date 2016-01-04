using System;
using System.Linq;
using System.Web.Configuration;

namespace MyAndromeda.Configuration
{
    public static class AzureBlobStorage
    {
        public static string ContainerName
        {
            get
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.AzureMenuCloudContainer"];
            }
        }

        public static string Password
        {
            get 
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.AzureAccountPassword"];   
            } 
        }

        public static string AccountName 
        {
            get 
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.AzureAccountName"];   
            }
        }
    }
}