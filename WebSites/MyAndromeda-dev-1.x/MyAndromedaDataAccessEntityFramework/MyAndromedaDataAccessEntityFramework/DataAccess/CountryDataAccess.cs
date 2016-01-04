using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DataAccess;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromedaDataAccessEntityFramework.DataAccess
{
    public class CountryDataAccess : ICountryDataAccess
    {
        public string GetAll(out List<MyAndromeda.Data.Domain.Country> models)
        {
            models = new List<MyAndromeda.Data.Domain.Country>();

            using (var entitiesContext = new AndroAdminDbContext())
            {
                var query = from s in entitiesContext.Countries
                            select s;

                foreach (Country country in query)
                {
                    var model = new MyAndromeda.Data.Domain.Country()
                    {
                        Id = country.Id,
                        CountryName = country.CountryName,
                        ISO3166_1_alpha_2 = country.ISO3166_1_alpha_2,
                        ISO3166_1_numeric = country.ISO3166_1_numeric
                    };

                    models.Add(model);
                }
            }

            return "";
        }
    }
}