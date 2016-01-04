using System;
using System.Linq;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Data.AcsServices.Models;

namespace MyAndromeda.Menus
{
    public static class Extensions
    {
        public static void Update(this MenuItem entity, MyAndromedaMenuItem menuItem) 
        {
            entity.Enabled = menuItem.Enabled;
            entity.Name = menuItem.Name;
            entity.WebName = menuItem.WebName;
            entity.WebDescription = menuItem.WebDescription;
        }
    }
}