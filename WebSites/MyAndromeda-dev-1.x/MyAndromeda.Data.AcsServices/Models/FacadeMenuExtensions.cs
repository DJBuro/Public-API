using System.Collections.Generic;
using System.Linq;
using MyAndromedaDataAccess.Domain.Menus.Items;

namespace MyAndromeda.Data.AcsServices.Models
{
    public static class MenuFacadeExtensions
    {
        public static Category Tranform(this RawCategory category, IList<RawCategory> parents)
        {
            var parentIndex = category.ParentId.HasValue ? category.ParentId.Value : category.Parent.GetValueOrDefault(); 
            var item = new Category()
            {
                Id = category.Id,
                Name = category.Name,
                //ParentId = 
            };

            if (parents == null || parentIndex < 0) 
            {
                return item;    
            }

            item.ParentId = parents[parentIndex].Id;

            return item;
        }

        public static MyAndromedaTopping Transform(this RawTopping topping) 
        {
            return new MyAndromedaTopping() 
            {
                CollectionPrice = topping.ColPrice,
                DeliveryPrice = topping.DelPrice,
                DineInPrice = null,
                Id = topping.Id,
                Name = topping.Name
            };
        }

        public static MyAndromedaMenuItem Transform(
            this RawMenuItem item,
            IList<string> names,
            IDictionary<int, RawCategory> displayCategories,
            IDictionary<int, RawCategory> categories,
            IDictionary<int, MyAndromedaTopping> toppings,
            IDictionary<int, IList<ThumbnailImage>> menuItemThumbnails)
        {
            var memuItemIndex = item.Name.HasValue ? item.Name.Value : item.ItemName.GetValueOrDefault();

            var oItem = new MyAndromedaMenuItem()
            {
                Id = item.Id.HasValue ? item.Id.Value : item.MenuId.HasValue ? item.MenuId.Value : 0,
                Name = names[memuItemIndex],
                DisplayCategoryId = item.Display,
                DisplayCategoryName = displayCategories[item.Display].Name,
                CategoryId1 = item.Category1.HasValue ? item.Category1.Value : item.Cat1.GetValueOrDefault(),
                CategoryId2 = item.Category2.HasValue ? item.Category2.Value : item.Cat2.GetValueOrDefault(),
                WebSequence = item.DisplayOrder.GetValueOrDefault()
            };

            if (menuItemThumbnails.ContainsKey(oItem.Id))
            {
                oItem.Thumbs = menuItemThumbnails[oItem.Id];
            }

            oItem.Toppings.FillToppings(item, toppings);

            return oItem;
        }

        public static void FillToppings(this MyAndromedaToppings toppings, RawMenuItem menuItem, IDictionary<int, MyAndromedaTopping> lookup)
        {
            foreach (var id in menuItem.DefTopIds)
            {
                toppings.DefaultToppings.Add(lookup[id]);
            }

            foreach (var id in menuItem.OptTopIds)
            {
                toppings.OptionalToppings.Add(lookup[id]);
            }
        }

        public static void AddThumbnails(this MyAndromedaMenuItem item, ILookup<int, ThumbnailImage> lookup)
        {
            if (lookup[item.Id] != null)
            {
                item.Thumbs = lookup[item.Id].ToList();
            }
            item.Thumbs = null;
        }
    }
}