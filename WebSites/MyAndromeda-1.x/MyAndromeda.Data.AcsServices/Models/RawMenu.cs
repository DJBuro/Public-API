using System;
using System.Collections.Generic;

namespace MyAndromeda.Data.AcsServices.Models
{
    public class RawMenu
    {
        public string ExportedOn { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<RawMenuItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the toppings.
        /// </summary>
        /// <value>The toppings.</value>
        public List<RawTopping> Toppings { get; set; }

        /// <summary>
        /// Gets or sets the display.
        /// </summary>
        /// <value>The display.</value>
        public List<RawCategory> Display { get; set; }

        /// <summary>
        /// Gets or sets the category1.
        /// </summary>
        /// <value>The category1.</value>
        public List<RawCategory> Category1 { get; set; }

        /// <summary>
        /// Gets or sets the category2.
        /// </summary>
        /// <value>The category2.</value>
        public List<RawCategory> Category2 { get; set; }

        /// <summary>
        /// Gets or sets the item names.
        /// </summary>
        /// <value>The item names.</value>
        public List<string> ItemNames { get; set; }

        /// <summary>
        /// Gets or sets the Deal items.
        /// </summary>
        public List<Deal> Deals { get; set; }
    }
}