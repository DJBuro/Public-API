namespace MyAndromeda.Data.AcsServices.Models
{
    public class RawTopping 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? DelPrice { get; set; }
        public decimal? ColPrice { get; set; }
    }
    
    public class RawMenuItem
    {
        /// <summary>
        /// The 'MenuId' will be fixed eventually  
        /// </summary>
        //[JsonProperty(propertyName: "Id")]
        public int? Id { get; set; }

        public int? MenuId { get; set; }

        //public int MenuId { get; set; }
        /// <summary>
        /// Gets or sets the display category id.
        /// </summary>
        /// <value>The display.</value>
        public int Display { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the first category id.
        /// </summary>
        /// <value>The category1.</value>
        public int? Category1 { get; set; }

        public int? Cat1 { get; set; }

        /// <summary>
        /// Gets or sets the second category id.
        /// </summary>
        /// <value>The category2.</value>
        public int? Category2 { get; set; }

        public int? Cat2 { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public int? ItemName { get; set; }

        public int? Name { get; set; }

        public int[] DefTopIds { get; set; }
        public int[] OptTopIds { get; set; }
    }
}