using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Core;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Framework.Authorization
{

    public class ChainAndSiteAuthorization
    {
        public bool NotAccessingChain { get; set; }

        public bool NotAccessingSite { get; set; }
        
        /// <summary>
        /// Gets or sets: the is user allowed at chain level.
        /// </summary>
        /// <value>The is user allowed at chain level.</value>
        public bool IsUserAllowedAtChainLevel { get; set; }

        /// <summary>
        /// Gets or sets: the is user allowed to site within chain.
        /// </summary>
        /// <value>The is user allowed to site within chain.</value>
        public bool IsUserAllowedToSiteWithinChain { get; set; }
    }

}