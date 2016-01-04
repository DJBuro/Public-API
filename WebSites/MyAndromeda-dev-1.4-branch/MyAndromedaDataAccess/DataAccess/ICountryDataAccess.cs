using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroCloudHelper;
using MyAndromedaDataAccess.Domain;
using MyAndromeda.Core;

namespace MyAndromedaDataAccess.DataAccess
{
    public interface ICountryDataAccess : IDependency
    {
        string GetAll(out List<MyAndromedaDataAccess.Domain.Country> countries);
    }
}
