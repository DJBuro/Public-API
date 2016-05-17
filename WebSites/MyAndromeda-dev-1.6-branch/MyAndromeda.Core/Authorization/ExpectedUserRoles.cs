using System;
using System.Linq;

namespace MyAndromeda.Core.Authorization
{
    /// <summary>
    /// Predefined static roles for the application 
    /// </summary>
    public static class ExpectedUserRoles
    {
        /// <summary>
        /// Can do everything - providing the store enrollment allows it. 
        /// </summary>
        public const string SuperAdministrator = "Super Administrator";
        /// <summary>
        /// Can do most things 
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// Provides access to logging and debug information and notification. 
        /// </summary>
        public const string DebugUser = "Debug";

        /// <summary>
        /// Well we dont build anything that isnt speced anymore ... so this is the see everything that shouldn't be seen permission. 
        /// </summary>
        public const string Experimental = "Experimental";
        /// <summary>
        /// Access to each of the stores in a chain 
        /// </summary>
        public const string ChainAdministrator = "Chain administrator";
        /// <summary>
        /// Alls access to all store admin permissions and gives user permissions at a chain level. 
        /// </summary>
        public const string StoreAdministrator = "Store Administrator";
        /// <summary>
        /// Allows access to every reporting permission.
        /// </summary>
        public const string ReportingUser = "Reporting";
        /// <summary>
        /// Default role. 
        /// </summary>
        public const string User = "Store User";
    }
}
