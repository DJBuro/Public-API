using System;
using System.Linq;

namespace MyAndromeda.Menus
{
    public static class Extensions
    {
        public static void Update(this MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.MenuItem entity, MyAndromeda.Data.AcsServices.Models.MyAndromedaMenuItem menuItem) 
        {
            entity.Enabled = menuItem.Enabled;
            entity.Name = menuItem.Name;
            entity.WebName = menuItem.WebName;
            entity.WebDescription = menuItem.WebDescription;
        }
    }
}