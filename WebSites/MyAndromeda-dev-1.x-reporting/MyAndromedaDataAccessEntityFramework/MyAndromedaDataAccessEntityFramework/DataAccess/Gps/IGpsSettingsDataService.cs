using MyAndromeda.Core;
using System;
using System.Data.Entity;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess.Gps
{
    public interface IBringgSettingsDataService : IDependency
    {
        IDbSet<Model.AndroAdmin.StoreGPSSetting> Settings { get; }
    }

    public class GpsSettingsDataService : IBringgSettingsDataService 
    {
        public GpsSettingsDataService(Model.AndroAdmin.AndroAdminDbContext dbContext) 
        {
            this.Settings = dbContext.StoreGPSSettings;
        }

        public IDbSet<StoreGPSSetting> Settings
        {
            get;
            private set;
        }
    }
}
