using MyAndromeda.Core;
using MyAndromeda.Data.AcsServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessMenuItemWebOrderingService : IDependency
    {
        int? AndromedaSiteId { get; }
        void SetupWithAndromedaSiteId(int andromedaSiteId);
        void CreateDefaultOrder(IEnumerable<MyAndromedaMenuItem> menuItems);
    }

    public class WebOrderingService : IAccessMenuItemWebOrderingService
    {
        private readonly IAccessMenuItemDataService accessMenuItemDataService;

        public WebOrderingService(IAccessMenuItemDataService accessMenuItemDataService)
        {
            this.accessMenuItemDataService = accessMenuItemDataService;
        }

        public int? AndromedaSiteId
        {
            get;
            private set;
        }

        public void SetupWithAndromedaSiteId(int andromedaSiteId)
        {
            this.accessMenuItemDataService.SetupWithAndromedaSiteId(andromedaSiteId);
            this.AndromedaSiteId = andromedaSiteId;
        }

        public void CreateDefaultOrder(IEnumerable<MyAndromedaMenuItem> menuItems)
        {
            var displayCategoryItems = menuItems
                                                .GroupBy(e => e.DisplayCategoryId)
                                                .ToDictionary(e => e.Key, e => e.ToList());

            foreach (var category in displayCategoryItems)
            {
                var max = category.Value.Max(e => e.WebSequence);


                foreach (var item in category.Value)
                {
                    if (item.WebSequence == 0)
                    {
                        item.WebSequence = Int32.MaxValue;
                    }
                }

                max = 100;

                //order by ascending. 
                var ordered = category.Value.OrderBy(e => e.WebSequence).ToArray();

                foreach (var item in ordered)
                {
                    item.WebSequence = max;
                    max = max + 100;
                }
            }
        }
    }
}
