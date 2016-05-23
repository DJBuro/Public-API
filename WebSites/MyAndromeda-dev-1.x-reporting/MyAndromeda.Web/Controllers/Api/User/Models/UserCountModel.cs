using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Web.Controllers.Api.User.Models
{
    public class UserCountModel
    {
        /// <summary>
        /// Gets or sets the chain users count.
        /// </summary>
        /// <value>The chain users count.</value>
        public int ChainUsersCount { get; internal set; }
        /// <summary>
        /// Gets or sets the count - sum of chain and store.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
        /// <summary>
        /// Gets or sets the store users count.
        /// </summary>
        /// <value>The store users count.</value>
        public int StoreUsersCount { get; internal set; }
    }
}
