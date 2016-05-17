using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyAndromeda.Data.AcsServices.Models
{
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
}
