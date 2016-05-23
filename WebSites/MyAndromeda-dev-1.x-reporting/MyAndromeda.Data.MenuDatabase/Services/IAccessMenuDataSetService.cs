using MyAndromeda.Core;
using MyAndromeda.Data.MenuDatabase.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessMenuDataSetService : 
        IDependency,
        IAccessMenuItemDataService,
        IAccessPriceDataService
    {


        new void SaveChanges();

        IEnumerable<Menu.n_PricesRow> ListPrices(Func<Menu.n_PricesRow, bool> query = null);
        IEnumerable<Menu.t_MenuRow> ListStructure(Func<Menu.t_MenuRow, bool> query = null);
    }
}
