using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Sites
{
    public interface IStoreDataService: IDependency, IDataProvider<Store> 
    {
        IDbSet<Store> Table { get; }
        void Update(Store store);
    }
}