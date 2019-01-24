namespace AndroAdminDataAccess.Domain.WebOrderingSetup
{
    public class GeneralSettings
    {
        public decimal MinimumDeliveryAmount { get; set; }

        public decimal DeliveryCharge { get; set; }

        public decimal? OptionalFreeDeliveryThreshold { get; set; }

        public decimal? CardCharge { get; set; } 

        public bool ApplyDeliveryCharges { get; set; }
        
        public bool IsList { get; set; }

        public bool IsEnterPostCode { get; set; }

        public bool EnableStoreLocatorPage { get; set; }

        public bool EnableHomePage { get; set; }

        public bool? EnableDelivery { get; set; }

        public bool? EnableCollection { get; set; }
        
        public bool? EnableDineIn { get; set; }
        //these are in CustomerAccountSettings object 
        //public bool EnableAndromedaLogin { get; set; }
        //public bool EnableFacebookLogin { get; set; }
        //public string FacebookApplicationId { get; set; }

        public decimal? DineInServiceCharge { get; set; }
        public decimal? LegalDineInServiceChargeLimit { get; set; }
        public bool? CompulsoryDineInServiceCharge { get; set; }

    }
}