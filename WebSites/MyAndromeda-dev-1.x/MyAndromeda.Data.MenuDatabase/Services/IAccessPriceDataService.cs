using System;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Models.Database;
using MyAndromeda.Data.MenuDatabase.Models;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessPriceDataService : 
        IAccessMenuDatabaseQuery<Menu.n_PricesRow>,
        IAccessMenuDatabaseQuery<Menu.t_MenuRow>
    {
        int? AndromedaSiteId { get; }
        void SetupWithAndromedaSiteId(int andromedaSiteId);

        void UpdatePrice(int menuItemId, int? inStore, int? delivery, int? collection);

        PriceCollection GetPrices(int menuItemId);
    }
}
