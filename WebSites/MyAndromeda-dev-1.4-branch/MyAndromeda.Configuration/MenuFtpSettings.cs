using System;
using System.Linq;
using System.Web.Configuration;

namespace MyAndromeda.Configuration
{
    public static class MenuFtpSettings
    {
        //<add key="MyAndromeda.Menu.Ftp.Url" value="54.245.96.4" />
        //<add key="MyAndromeda.Menu.Ftp.UserName" value="Matt" />
        //<add key="MyAndromeda.Menu.Ftp.Password" value="Loki4446" />
        //<add key="MyAndromeda.Menu.Ftp.Root" value="Menus" />
        //<add key="MyAndromeda.Menu.Ftp.TransferMode" value="Passive" <!-- or Active --> />
        public static string UserName 
        {
            get 
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.Ftp.UserName"];
            }
        }

        public static string Password 
        {
            get 
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.Ftp.Password"];
            }
        }

        public static string TransferMode 
        {
            get 
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.Ftp.TransferMode"];
            }
        }

        public static string RootFolder
        {
            get 
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.Ftp.Root"];    
            }
        }

        public static string Host 
        {
            get 
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.Ftp.Url"];
            }
        }

        public static string LocalFolder
        {
            get 
            {
                return WebConfigurationManager.AppSettings["MyAndromeda.Menu.LocalFolder"];
            }
        }


    }
}