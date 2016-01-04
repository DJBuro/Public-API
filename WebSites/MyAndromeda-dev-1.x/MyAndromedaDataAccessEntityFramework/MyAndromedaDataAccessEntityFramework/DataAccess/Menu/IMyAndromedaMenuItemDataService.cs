using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Menu
{
    public interface IMyAndromedaMenuItemDataService : IDependency 
    {
        IQueryable<MenuItem> Query(int andromedaSiteId);
    }
}