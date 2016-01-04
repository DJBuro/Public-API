using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAndromeda.Core;
using System.Data.Entity;
using MyAndromeda.Data.Domain;

namespace MyAndromedaDataAccess.DataAccess
{
    public interface IAddressDataAccess : IDependency
    {
        string UpsertBySiteId(int siteId, Address address);
    }
}
