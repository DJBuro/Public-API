using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin
{
    public static class Permissions
    {
        [Obsolete("Previous hub selection work")]
        public const string ReadHubs = "ViewHubs";
        [Obsolete("Previous hub selection work")]
        public const string EditHubs = "EditHubs";

        public const string ResetHubHardwareKey = "ResetHubHardwareKey";

        public static class EditServicesHostList 
        {
            //Host types
            public const string ReadHostTypeList = "ReadHostTypeList";
            public const string EditHostTypeList = "EditHostTypeList";
            
            //Host list
            public const string ReadHostList = "ReadHostList";
            public const string EditHostList = "EditHostList";

            //Host - Store relation 
            public const string ReadStoreHostList = "ReadStoreHostList";
            public const string EditStoreHostList = "EditStoreHostList";

            //host - Application relation
            public const string ReadApplicationHostList = "ReadApplicationHostList";
            public const string EditApplicationHostList = "EditApplicationHostList";
        }
    }
}