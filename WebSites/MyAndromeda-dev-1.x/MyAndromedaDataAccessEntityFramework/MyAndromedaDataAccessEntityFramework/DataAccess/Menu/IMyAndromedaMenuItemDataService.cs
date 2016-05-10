using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public interface IMyAndromedaMenuItemDataService : IDependency 
    {
        IQueryable<MenuItem> Query(int andromedaSiteId);
    }
}