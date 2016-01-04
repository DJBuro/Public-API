using System.Collections.Generic;

namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class MealPart
    {
        public int MealPartId { get; set; }

        public int GroupId { get; set; }

        public string Name { get; set; }

        public string Synonym { get; set; }

        public List<Accessory> OptionalAccessories { get; set; }

        public List<Accessory> RequiredAccessories { get; set; }
    }
}