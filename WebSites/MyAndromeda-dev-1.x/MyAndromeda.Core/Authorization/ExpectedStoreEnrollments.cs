using System;
using MyAndromeda.Core.Site;
using System.Collections.Generic;

namespace MyAndromeda.Core.Authorization
{
    public static class ExpectedStoreEnrollments 
    {
        /// <summary>
        /// Every store is awarded this enrollment regardless of selection.
        /// </summary>
        public const string DefaultStoreEnrollment = "Default Store";

        /// <summary>
        /// Full enrollment that contains access to all 
        /// </summary>
        public const string FullEnrollment = "Full";

        /// <summary>
        /// This enrollment contains every feature that a Rameses store can make use of. 
        /// </summary>
        public const string RamesesStoreEnrollment = "Rameses store";

        /// <summary>
        /// This enrollment contains every feature that a GPRS store can make use of. 
        /// </summary>
        public const string GprsStore = "GPRS store";
    }

    
}