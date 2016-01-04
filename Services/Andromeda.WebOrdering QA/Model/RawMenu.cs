using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Andromeda.WebOrdering.Model
{
    /// <summary>
    /// Parent Class - Menu
    /// </summary>
    public class MenuRoot
    {
        public Menu Menu { get; set; }
    }
    
    public class Menu
    {
        public string SiteID { get; set; }
        public string MenuType { get; set; }
        public string Version { get; set; }
        public string MenuData { get; set; }
        public string MenuDataThumbnails { get; set; }
    }

    public class StoreMenu
    {
        public RawMenu MenuData { get; set; }
        public MenuThumbnails MenuDataThumbnails { get; set; }
    }

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
        public string ItemName { get; set; }

        public string Desc { get; set; }

        public int? Name { get; set; }

        public int[] DefTopIds { get; set; }
        public int[] OptTopIds { get; set; }

        public decimal? DelPrice { get; set; }
        public decimal? ColPrice { get; set; }
    }

    public class RawCategory
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public int? Parent { get; set; }
    }

    public class Deal
    {
        private int dealId = 0;
        [XmlElement(ElementName = "Id")]
        [JsonProperty(PropertyName = "Id")]
        public int DealId
        {
            get { return dealId; }
            set { dealId = value; }
        }

        private string dealName = "";
        public string DealName
        {
            get { return dealName; }
            set { dealName = value; }
        }

        private bool forDelivery = false;
        public bool ForDelivery
        {
            get { return forDelivery; }
            set { forDelivery = value; }
        }

        private int deliveryAmount = 0;
        public int DeliveryAmount
        {
            get { return deliveryAmount; }
            set { deliveryAmount = value; }
        }

        private bool forCollection = false;
        public bool ForCollection
        {
            get { return forCollection; }
            set { forCollection = value; }
        }

        private int collectionAmount = 0;
        public int CollectionAmount
        {
            get { return collectionAmount; }
            set { collectionAmount = value; }
        }

        private bool forceCheapestFree = false;
        public bool ForceCheapestFree
        {
            get { return forceCheapestFree; }
            set { forceCheapestFree = value; }
        }

        private int minimumOrderValue = 0;
        public int MinimumOrderValue
        {
            get { return minimumOrderValue; }
            set { minimumOrderValue = value; }
        }

        private string description = "";
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private List<DealLine> dealLines = new List<DealLine>();
        public List<DealLine> DealLines
        {
            get { return dealLines; }
            set { dealLines = value; }
        }

        private int displayOrder = 0;
        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public AvailableTimes AvailableTimes { get; set; }

        public string FullOrderDiscountType { get; set; }
        public int FullOrderDiscountDeliveryAmount { get; set; }
        public int FullOrderDiscountCollectionAmount { get; set; }
        public int FullOrderDiscountMagicNumber { get; set; }
    }

    public class DealLine
    {
        public string Type = "";

        [XmlElement(ElementName = "DelAmount")]
        [JsonProperty(PropertyName = "DelAmount")]
        public int DeliveryAmount = 0;

        [XmlElement(ElementName = "ColAmount")]
        [JsonProperty(PropertyName = "ColAmount")]
        public int CollectionAmount = 0;

        [XmlArray("AllowedItems")]
        [XmlArrayItem("Id")]
        public List<int> AllowableItemsIds = new List<int>();
    }

    public class AvailableTimes
    {
        [XmlElement(ElementName = "Mon")]
        [JsonProperty(PropertyName = "Mon")]
        public List<AvailableTime> MondayAvailableTimes { get; set; }

        [XmlElement(ElementName = "Tue")]
        [JsonProperty(PropertyName = "Tue")]
        public List<AvailableTime> TuesdayAvailableTimes { get; set; }

        [XmlElement(ElementName = "Wed")]
        [JsonProperty(PropertyName = "Wed")]
        public List<AvailableTime> WednesdayAvailableTimes { get; set; }

        [XmlElement(ElementName = "Thr")]
        [JsonProperty(PropertyName = "Thr")]
        public List<AvailableTime> ThursdayAvailableTimes { get; set; }

        [XmlElement(ElementName = "Fri")]
        [JsonProperty(PropertyName = "Fri")]
        public List<AvailableTime> FridayAvailableTimes { get; set; }

        [XmlElement(ElementName = "Sat")]
        [JsonProperty(PropertyName = "Sat")]
        public List<AvailableTime> SaturdayAvailableTimes { get; set; }

        [XmlElement(ElementName = "Sun")]
        [JsonProperty(PropertyName = "Sun")]
        public List<AvailableTime> SundayAvailableTimes { get; set; }
    }

    public class AvailableTime
    {
        [XmlIgnore]
        [JsonIgnore]
        public DateTime FromTime { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public DateTime ToTime { get; set; }

        public bool NotAvailableToday { get; set; }

        [XmlElement(ElementName = "From")]
        [JsonProperty(PropertyName = "From")]
        public string FromTimeString { get { return FromTime.ToShortTimeString(); } }

        [XmlElement(ElementName = "To")]
        [JsonProperty(PropertyName = "To")]
        public string ToTimeString { get { return ToTime.ToShortTimeString(); } }

        public AvailableTime() { }

        public AvailableTime(DateTime fromTime, DateTime toTime)
        {
            this.FromTime = fromTime;
            this.ToTime = toTime;
        }
    }

    public class MenuThumbnails
    {
        public Server Server { get; set; }
        public List<MenuThumbnailItem> MenuItemThumbnails { get; set; }
    }

    public class MenuThumbnailItem
    {
        public string Src { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public List<int> ItemIds { get; set; }
    }

    public class Server
    {
        public string Endpoint { get; set; }
    }
}