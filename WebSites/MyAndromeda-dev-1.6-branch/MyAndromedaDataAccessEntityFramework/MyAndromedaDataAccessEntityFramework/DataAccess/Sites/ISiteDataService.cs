using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess.Sites
{
    public interface ISiteDataService : IDependency
    {
        IEnumerable<SiteDomainModel> List(Expression<Func<Store, bool>> query);

        IEnumerable<TResult> ListAndTransform<TResult>(Expression<Func<Store, bool>> query, Expression<Func<Store, TResult>> transform);

        IEnumerable<int> GetAcsApplicationIds(int siteId);

        IEnumerable<string> GetExternalApplicationIds(int siteId);

        IEnumerable<string> GetExternalAcsApplicationIds(int siteId);
    }
}