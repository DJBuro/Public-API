using AndroCloudDataAccess.Domain;
using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Services.Loyalty.Models;
using System.Data.Entity;
using Newtonsoft.Json;

namespace MyAndromeda.Services.Loyalty.Points
{
    public interface IAndromedaLoyaltyPointsHelper : IDependency
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <returns></returns>
        Task<AndromedaLoyaltyConfiguration> GetSettingsAsync(int andromedaSiteId);

        /// <summary>
        /// Gets the points summary.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="points">The points.</param>
        /// <returns></returns>
        LoyaltyPointSummary GetPointsSummary(AndromedaLoyaltyConfiguration settings, decimal? points);
    }

    public class PointsHelper : IAndromedaLoyaltyPointsHelper 
    {
        private readonly DbSet<MyAndromeda.Data.Model.AndroAdmin.StoreLoyalty> StoreLoyalty;

        public PointsHelper(MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdmindbContext) 
        {
            this.StoreLoyalty = androAdmindbContext.StoreLoyalties;
        }

        public async Task<AndromedaLoyaltyConfiguration> GetSettingsAsync(int andromedaSiteId)
        {
            var configuration = await this.StoreLoyalty.Where(e => e.Store.AndromedaSiteId == andromedaSiteId).FirstOrDefaultAsync();

            if (configuration == null) { return null; }

            var config = JsonConvert.DeserializeObject<AndromedaLoyaltyConfiguration>(configuration.Configuration);
            
            return config;
        }

        public LoyaltyPointSummary GetPointsSummary(AndromedaLoyaltyConfiguration settings, decimal? points)
        {
            var conversionRate = settings.PointValue;
            var value = points.GetValueOrDefault() * (1/conversionRate.GetValueOrDefault(1));

            return new LoyaltyPointSummary() 
            {
                Points = points.GetValueOrDefault(),
                Value = value
            };
        }
    }
}
