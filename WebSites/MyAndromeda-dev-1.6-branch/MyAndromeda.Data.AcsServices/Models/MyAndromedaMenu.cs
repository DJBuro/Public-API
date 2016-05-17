using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromedaDataAccess.Domain.Menus.Items;

namespace MyAndromeda.Data.AcsServices.Models
{
    public class MyAndromedaMenu
    {
        private readonly RawMenu rawMenu;

        private readonly Dictionary<int, RawCategory> allCategories;
        private readonly Dictionary<int, RawCategory> displayCategories;
        private readonly Dictionary<int, MyAndromedaTopping> toppingsLookup;

        public MyAndromedaMenu(RawMenu rawMenu, IDictionary<int, IList<ThumbnailImageDomainModel>> menuItemThumbnails)
        {
            this.rawMenu = rawMenu;
            this.allCategories = rawMenu
                .Display //list of categories
                .Union(rawMenu.Category1) //list of categories
                .Union(rawMenu.Category2) //list of categories
                .GroupBy(e=> e.Id, e=> e)
                .ToDictionary(e => e.Key, e=> e.First());

            this.displayCategories = rawMenu.Display.ToDictionary(e => e.Id, e => e);

            //categories
            this.DisplayCategories = displayCategories.Values.Select(e => e.Tranform(null)).ToList();

            //both parents are of the display category
            this.Categories1 = rawMenu.Category1
                .GroupBy(e=> e.Id, e=> e)
                .Select(e=> e.First())
                .Select(e => e.Tranform(rawMenu.Display)).ToList();

            this.Categories2 = rawMenu.Category2
                .GroupBy(e=> e.Id, e=> e)
                .Select(e=> e.First())
                .Select(e => e.Tranform(rawMenu.Display)).ToList();

            this.toppingsLookup = rawMenu.Toppings
                .GroupBy(e => e.Id, e => e)
                .ToDictionary(e => e.Key, e => e.First().Transform());

            this.Toppings = this.toppingsLookup.Values.ToList();

            this.MenuItems = rawMenu.Items
                //.GroupBy(e=> e.Id, e => e)
                //.Select(e=> e.First())
                .Select(e => e.Transform(rawMenu.ItemNames, 
                    displayCategories, 
                    allCategories, 
                    toppingsLookup,
                    menuItemThumbnails))
                .ToList();

            this.DealItems = rawMenu.Deals;
        }

        public bool DetailEditable { get; set; }

        public DateTime ExportedOn { get; set; }

        public IList<MyAndromedaMenuItem> MenuItems { get; set; }
        public IList<Deal> DealItems { get; set; }

        public IList<Category> DisplayCategories { get; set; }
        public IList<Category> Categories1 { get; set; }
        public IList<Category> Categories2 { get; set; }

        public List<MyAndromedaTopping> Toppings { get; set; }
    }
}
