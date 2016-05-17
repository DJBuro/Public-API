using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyAndromeda.Services.Bringg.Models;

namespace MyAndromeda.Services.Bringg.Services
{
    public class BringgSettingsService : MyAndromeda.Services.Bringg.Services.IBringgSettingsService
    {
        private MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext dbContext;

        public BringgSettingsService(MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext dbContext)
        { 
            this.dbContext = dbContext;
        }

        public BringgAuth Get(int androAdminStoreId) 
        {
            MyAndromeda.Data.Model.AndroAdmin.StoreGPSSetting settings;

            settings = dbContext.StoreGPSSettings.FirstOrDefault(e => e.StoreId == androAdminStoreId);

            var result = JsonConvert.DeserializeObject<Models.BringgAuth>(settings.PartnerConfig);

            return result;
        }
    }
}
