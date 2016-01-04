using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAndromeda.Core;
using Domain = MyAndromedaDataAccess.Domain;

namespace MyAndromeda.Data.DataAccess
{
    public interface ISiteDataAccess : IDependency
    {
        Domain.Site GetById(int siteId);

        IEnumerable<int> GetAcsApplicationIds(int siteId);
        IEnumerable<string> GetExternalApplicationIds(int siteId);
        //string GetByMyAndromedaUserId(int myAndromedaUserId, out List<MyAndromedaDataAccess.Domain.Site> sites);

        IEnumerable<string> GetExternalAcsApplicationIds(int siteId);
    }
}
