
using System.Linq;
using MyAndromeda.Data.Domain;
using MyAndromeda.Data.Model.AndroAdmin;
using System.Collections.Generic;

namespace MyAndromeda.Data.DataAccess
{
    public class CountryDataAccess : ICountryDataAccess
    {
        public string GetAll(out List<CountryDomainModel> models)
        {
            models = new List<CountryDomainModel>();

            using (var entitiesContext = new AndroAdminDbContext())
            {
                var query = from s in entitiesContext.Countries
                            select s;

                foreach (Country country in query)
                {
                    var model = new CountryDomainModel()
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