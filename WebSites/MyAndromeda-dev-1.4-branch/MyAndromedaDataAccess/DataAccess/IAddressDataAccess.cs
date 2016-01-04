using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAndromedaDataAccess.Domain;
using MyAndromeda.Core;

namespace MyAndromedaDataAccess.DataAccess
{
    public interface IAddressDataAccess : IDependency
    {
        string UpsertBySiteId(int siteId, Address address);
    }
}
