using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;

namespace MyAndromeda.Data.DataAccess
{
    public interface ICountryDataAccess : IDependency
    {
        string GetAll(out List<Data.Domain.Country> countries);
    }
}
