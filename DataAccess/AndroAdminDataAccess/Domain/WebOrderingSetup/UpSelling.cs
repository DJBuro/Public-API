using System.Collections.Generic;
using MyAndromeda.Data.AcsServices.Models;

namespace AndroAdminDataAccess.Domain.WebOrderingSetup
{
    public class UpSellingSettings
    {
        public bool Enabled { get; set; }

        public List<Category> DisplayCategories { get; set; }
    }
}