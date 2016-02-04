using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public interface IMyAndromedaMediaProfilesDataService : IDependency 
    {
        IQueryable<SiteMenuMediaProfileSize> Query();
    }
}