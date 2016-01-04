using System.Collections.Generic;

namespace AndroCloudWebApiExternal.Models.Andromeda
{
    public class OrderLine
    {
        public OrderLine() 
        {
            this.productId = 0;
        }

        public int productId { get; set; }

        public int quantity { get; set; }

        public int price { get; set; }

        public string name { get; set; }

        public int orderLineIndex { get; set; }

        public int lineType { get; set; }

        public string instructions { get; set; }

        public bool inDealFlag { get; set; }

        public List<AddTopping> addToppings { get; set; }

        public List<AddTopping> removeToppings { get; set; }
    }
}