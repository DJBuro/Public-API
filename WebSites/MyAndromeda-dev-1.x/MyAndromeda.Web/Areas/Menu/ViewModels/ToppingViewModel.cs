using MyAndromeda.Data.AcsServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Menu.ViewModels
{
    public class ToppingViewModelGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Group Group { get; set; }
        public IList<ToppingVarient> ToppingVarients { get; set; }

        public decimal? DineInPrice { get; set; }
        public decimal? CollectionPrice { get; set; }
        public decimal? DeliveryPrice { get; set; }
    }

    public class ToppingVarient 
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        
        public decimal? DineInPrice { get; set; }
        public decimal? CollectionPrice { get; set; }
        public decimal? DeliveryPrice { get; set; }
    }

    public static class ToppingViewModelExtensions 
    {
        public static IEnumerable<MyAndromedaTopping> ToModel(this ToppingViewModelGroup toppingGroup)
        {
            var toppings = toppingGroup.ToppingVarients
                .Select(e => e.ToModel(toppingGroup.Name, toppingGroup.Group))
                .ToArray();

            return toppings;
        }

        public static MyAndromedaTopping ToModel(this ToppingVarient varient, string name, Group group) 
        {
            return new MyAndromedaTopping()
            {
                Id = varient.Id,
                Name = name,
                SubCategory = varient.Category,
                Group = group,
                CollectionPrice = varient.CollectionPrice,
                DeliveryPrice = varient.DeliveryPrice,
                DineInPrice = varient.DineInPrice
            };
        }

        public static ToppingViewModelGroup ToViewModel(this IEnumerable<MyAndromedaTopping> toppings, string name)
        {
            var model = new ToppingViewModelGroup() 
            {
                //need a unique id from the kendo ui perspective ... you will do
                Id = string.Join("-", toppings.Select(e=> e.Id.ToString())),
                Name = name,
                Group = toppings.First().Group //<-- this cannot be null given the place it came from 
            };

            model.ToppingVarients = toppings
                .OrderBy(e => e.SubCategory.Name)
                .Select(e => ToVarientViewModel(e)).ToList();

            model.CollectionPrice = model.ToppingVarients.Average(e => e.CollectionPrice);
            model.DeliveryPrice = model.ToppingVarients.Average(e => e.DeliveryPrice);
            model.DineInPrice = model.ToppingVarients.Average(e => e.DineInPrice);

            return model;
        }

        private static ToppingVarient ToVarientViewModel(this MyAndromedaTopping topping) 
        {
            return new ToppingVarient() 
            {
                Category = topping.SubCategory,
                Id = topping.Id,
                DineInPrice = topping.DineInPrice,
                CollectionPrice = topping.CollectionPrice,
                DeliveryPrice = topping.DineInPrice
            };
        }
    }
}