using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Models
{
    public class Configuration
    {
        const string SignPostKey = "SignpostUrls";
        const string OrderEndpointKey = "OrderEndpoint";

        private static string singPostUrl = null;
        public static string SignPostUrl 
        {
            get 
            {
                return singPostUrl ?? (singPostUrl = ConfigurationManager.AppSettings[SignPostKey]);
            }
        }

        private static string orderEndpoint = null;
        public static string OrderEndpoint 
        {
            get 
            {
                return orderEndpoint ?? (orderEndpoint = ConfigurationManager.AppSettings[OrderEndpointKey]);
            }
        }

    }

    public class JustEatConfiguration 
    {
        const string JustEatApplicationIdKey = "JustEatApplicationId";
        const string AuthorizationTokenKey = "JustEatAuthorizationToken";

        private static string applicationId = null;
        public static string ApplicationId 
        {
            get 
            {
                return applicationId ?? (applicationId = ConfigurationManager.AppSettings[JustEatApplicationIdKey]).Trim();
            } 
        }

        private static string authorizationTokenValue = null;
        public static string AuthorizationTokenValue
        {
            get 
            {
                return authorizationTokenValue ?? (authorizationTokenValue = ConfigurationManager.AppSettings[AuthorizationTokenKey]).Trim();
            } 
        }
    }
}