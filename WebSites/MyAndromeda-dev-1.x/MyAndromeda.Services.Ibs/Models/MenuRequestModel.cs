using System.Collections.Generic;

namespace MyAndromeda.Services.Ibs.Models
{
    public class MenuRequestModel 
    {
    
    }

    public class MenuResult 
    {
        public int WebOrderingPriceLevel { get; set; }

        public List<MenuSection> Items { get; set; }
        public List<MenuGroup> MajorGroups { get; set; }
        public List<PrimaryGroup> PrimaryGroups { get; set; }
        public List<Modifier> Modifiers { get; set; }
        public List<SalesGroup> SalesGroups { get; set; }
    }

    public class MenuSection 
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public List<MenuSubCatItem> SubCategroyItems { get; set; }
    }

    public class MenuItem 
    {
        public long PluNumber { get; set; }

        public long PluType { get; set; }

        public string Description { get; set; }

        public string KpDescription { get; set; }

        public string Details { get; set; }

        public List<PriceModel> Prices { get; set; }

        public bool InUse { get; set; }
    }

    public class MenuSubCatItem 
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public List<MenuItem> Items { get; set; }
    }

    public class PriceModel
    {
        public decimal Price { get; set; }
        public int ModNo { get; set; }
    }

    public class MenuGroup
    {
        public long Id { get; set; }
        public long PrimaryGroupId { get; set; }
        
        public string Description { get; set; }
    }

    public class Modifier
    {
        public string Prefix { get; set; }
        public double Factor { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class PrimaryGroup
    { 
        public long Id { get; set; }
        public string Description { get; set; }
    }

    public class SalesGroup 
    {
        public long Id { get; set; }
        public long MajorGroupId { get; set; }
        public string Description { get; set; }
    }
}