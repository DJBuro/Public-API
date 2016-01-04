using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using MyAndromedaDataAccess.Domain.Menus.Items;

namespace MyAndromeda.Data.AcsServices.Models
{
    public class MyAndromedaMenuItemSavingModel 
    {
        public MyAndromedaMenuItem[] Models { get; set; }
    }

    [DebuggerDisplay("Menu Item: {Id} - {Name} - {DisplayCategoryName}")]
    public class MyAndromedaMenuItem
    {
        public MyAndromedaMenuItem()
        {
            this.Toppings = new MyAndromedaToppings();
            this.Prices = new MyAndromedaMenuItemPrices();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the web.
        /// </summary>
        /// <value>The name of the web.</value>
        [MaxLength(255)]
        public string WebName { get; set; }

        /// <summary>
        /// Gets or sets the web description.
        /// </summary>
        /// <value>The web description.</value>
        [MaxLength(255)]
        public string WebDescription { get; set; }

        /// <summary>
        /// Gets or sets the web order.
        /// </summary>
        /// <value>The web order.</value>
        public int WebSequence { get; set; }

        /// <summary>
        /// Gets or sets the display name of the category.
        /// </summary>
        /// <value>The display name of the category.</value>
        public string DisplayCategoryName { get; set; }

        /// <summary>
        /// Gets or sets the display category id.
        /// </summary>
        /// <value>The display category id.</value>
        public int DisplayCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category id1.
        /// </summary>
        /// <value>The category id1.</value>
        public int CategoryId1 { get; set; }

        /// <summary>
        /// Gets or sets the category id2.
        /// </summary>
        /// <value>The category id2.</value>
        public int CategoryId2 { get; set; }

        /// <summary>
        /// Gets or sets the thumbs.
        /// </summary>
        /// <value>The thumbs.</value>
        public IList<ThumbnailImage> Thumbs { get; set; }

        /// <summary>
        /// Gets or sets the prices.
        /// </summary>
        /// <value>The prices.</value>
        public MyAndromedaMenuItemPrices Prices { get; set; }

        /// <summary>
        /// Gets or sets the toppings.
        /// </summary>
        /// <value>The toppings.</value>
        public MyAndromedaToppings Toppings { get; set; }
    }

    public class MyAndromedaMenuItemPrices
    {
        public decimal Instore { get; set; }
        public decimal Delivery { get; set; }
        public decimal Collection { get; set; }
    }

    public class MyAndromedaToppings
    {
        public MyAndromedaToppings()
        {
            this.DefaultToppings = new List<MyAndromedaTopping>();
            this.OptionalToppings = new List<MyAndromedaTopping>();
        }

        public List<MyAndromedaTopping> DefaultToppings { get; set; }
        public List<MyAndromedaTopping> OptionalToppings { get; set; }
    }
}