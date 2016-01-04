namespace AndroCloudWebApiExternal.Models.Andromeda
{
    public class AddTopping
    {
        public AddTopping() 
        {
            this.quantity = 1;
        }

        public int productId { get; set; }

        public string name { get; set; }

        public int quantity { get; set; }

        public int price { get; set; }

        public int itemPrice { get; set; }

        public string instructions { get; set; }
    }
}