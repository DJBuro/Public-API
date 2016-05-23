using System.Collections.Generic;
using System.Diagnostics;

namespace MyAndromeda.Data.AcsServices.Models
{
    [DebuggerDisplay("Topping: {Name}")]
    public class MyAndromedaTopping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category SubCategory { get; set; }

        public Group Group { get; set; }

        public decimal? DeliveryPrice { get; set; }

        public decimal? CollectionPrice { get; set; }

        public decimal? DineInPrice { get; set; }
    }

    public class GroupedToppingsByGroup
    {
        Group Group { get; set; }
        IEnumerable<MyAndromedaTopping> Toppings { get; set; }
    }
}