using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using MyAndromeda.Data.MenuDatabase.Models.Database;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessMenuItemDataService: IAccessMenuDatabaseQuery<Menu.n_MenuRow>
    {
        int? AndromedaSiteId { get; }
        void SetupWithAndromedaSiteId(int andromedaSiteId);
    }

}