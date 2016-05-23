using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Data.DataAccess
{
    public interface ICountryDataAccess : IDependency
    {
        string GetAll(out List<CountryDomainModel> countries);
    }
}
