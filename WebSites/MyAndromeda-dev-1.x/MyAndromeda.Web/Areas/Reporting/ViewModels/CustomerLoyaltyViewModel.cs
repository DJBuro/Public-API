using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class CustomerLoyaltyViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
        public decimal? Points { get; set; }

        /// <summary>
        /// Gets or sets the pending points.
        /// </summary>
        /// <value>The pending points.</value>
        public decimal PendingPoints { get; set; }

        /// <summary>
        /// Gets or sets the earned points.
        /// </summary>
        /// <value>The earned points.</value>
        public decimal EarnedPoints { get; set; }

        /// <summary>
        /// Gets or sets the spent points.
        /// </summary>
        /// <value>The spent points.</value>
        public decimal SpentPoints { get; set; }

        /// <summary>
        /// Gets or sets the earned points value.
        /// </summary>
        /// <value>The earned points value.</value>
        public decimal EarnedPointsValue { get; set; }

        /// <summary>
        /// Gets or sets the spent points value.
        /// </summary>
        /// <value>The spent points value.</value>
        public int SpentPointsValue { get; set; }
    }
}