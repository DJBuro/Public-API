using System.Collections.Generic;

namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class BasketItem
    {
        public int ProductId { get; set; }

        public int ProductTypeId { get; set; }

        public string MenuCardNumber { get; set; }

        public string Name { get; set; }

        public string Synonym { get; set; }

        public double UnitPrice { get; set; }

        public double CombinedPrice { get; set; }

        public List<MealPart> MealParts { get; set; }

        public List<Accessory> OptionalAccessories { get; set; }

        public List<Accessory> RequiredAccessories { get; set; }

        public List<object> Discounts { get; set; }

        public List<object> MultiBuyDiscounts { get; set; }
    }
}