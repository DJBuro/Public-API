namespace AndroCloudDataAccessEntityFramework.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using AndroCloudDataAccess.DataAccess;
    using AndroCloudDataAccess.Domain;
    using AndroCloudDataAccessEntityFramework.Exceptions;
    using AndroCloudDataAccessEntityFramework.Model;
    using AndroCloudHelper;
    using AndroCloudServices.Models;
    using AndroCloudWCFHelper;
    using Newtonsoft.Json;
    using ACSApplication = AndroCloudDataAccessEntityFramework.Model.ACSApplication;
    using Site = AndroCloudDataAccess.Domain.Site;


    public class SitesDataAccess : ISiteDataAccess
    {
        public string ConnectionStringOverride { get; set; }

        public string GetByFilter(
            int applicationId, 
            float? filterByMaxDistance, 
            double? filterByLongitude,
            double? filterByLatitude,
            string deliveryZoneFilter,
            DataTypeEnum dataType, 
            out List<AndroCloudDataAccess.Domain.Site> sites)
        {
            sites = new List<AndroCloudDataAccess.Domain.Site>();

            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);
                string dataTypeString = dataType.ToString();

                if (deliveryZoneFilter == null || deliveryZoneFilter.Length == 0)
                {
                    // Don't filter by delivery zone
                    var sitesQuery = from s in acsEntities.Sites.Include(e => e.SiteLoyalties)
                                     join acss in acsEntities.ACSApplicationSites
                                       on s.ID equals acss.SiteId
                                     join a in acsEntities.ACSApplications
                                       on acss.ACSApplicationId equals a.Id
                                     join sm in acsEntities.SiteMenus
                                       on s.ID equals sm.SiteID
                                     join ss in acsEntities.SiteStatuses
                                       on s.SiteStatusID equals ss.ID
                                     where sm.MenuType == dataTypeString
                                       && a.Id == applicationId
                                       && ss.Status == "Live"
                                     select new
                                     {
                                         s.ID,
                                         s.EstimatedDeliveryTime,
                                         s.EstimatedCollectionTime,
                                         s.StoreConnected,
                                         sm.Version,
                                         s.ExternalSiteName,
                                         s.ExternalId,
                                         s.LicenceKey,
                                         s.Address.Lat,
                                         s.Address.Long,
                                         s.AndroID
                                     };

                    var siteEntities = sitesQuery.ToList();

                    foreach (var siteEntity in siteEntities)
                    {
                        bool returnSite = true;

                        // Do we need to filter by distance i.e. only return the closest X stores?
                        if (filterByMaxDistance != null && filterByLongitude != null && filterByLatitude != null)
                        {
                            // Do we have the location of the site?
                            if (siteEntity.Lat == null ||
                                siteEntity.Long == null ||
                                filterByLatitude == null ||
                                filterByLongitude == null ||
                                !double.TryParse(siteEntity.Long, out double storeLongitude) ||
                                !double.TryParse(siteEntity.Lat, out double storeLatitude))
                            {
                                // Don't know where the site is so don't return it
                                returnSite = false;
                            }
                            else
                            {
                                // Calculate the distance between the site and the customer
                                double distance = SpacialHelper.CalcDistanceBetweenTwoPoints(filterByLongitude.Value, filterByLatitude.Value, storeLongitude, storeLatitude);

                                // Is the site within X km of the customer?
                                if (distance > filterByMaxDistance)
                                {
                                    // Out of range - don't return the site
                                    returnSite = false;
                                }
                            }
                        }

                        if (returnSite)
                        {
                            AndroCloudDataAccess.Domain.Site site = new AndroCloudDataAccess.Domain.Site();
                            site.Id = siteEntity.ID;
                            site.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                            site.EstCollTime = siteEntity.EstimatedCollectionTime.GetValueOrDefault(0);
                            site.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                            site.MenuVersion = siteEntity.Version.GetValueOrDefault(0);
                            site.Name = siteEntity.ExternalSiteName;
                            site.ExternalId = siteEntity.ExternalId;
                            site.LicenceKey = siteEntity.LicenceKey;
                            site.AndroId = siteEntity.AndroID;

                            GetLoyaltyConfiguration(site, siteEntity);

                            sites.Add(site);
                        }
                    }
                }
                else
                {
                    // Get rid of spaces
                    deliveryZoneFilter = deliveryZoneFilter.Replace(" ", "").Trim();

                    // Add the space back in, in the correct place
                    string cleanedUpDeliveryZoneFilter = 
                        deliveryZoneFilter.Substring(0, deliveryZoneFilter.Length - 3) + 
                        " " + 
                        deliveryZoneFilter.Substring(deliveryZoneFilter.Length - 3, 3);

                    // Filter by delivery zone
                    var sitesQuery = from s in acsEntities.Sites
                                     join acss in acsEntities.ACSApplicationSites
                                       on s.ID equals acss.SiteId
                                     join a in acsEntities.ACSApplications
                                       on acss.ACSApplicationId equals a.Id
                                     join sm in acsEntities.SiteMenus
                                       on s.ID equals sm.SiteID
                                     join ss in acsEntities.SiteStatuses
                                       on s.SiteStatusID equals ss.ID
                                     join da in acsEntities.DeliveryAreas
                                       on s.ID equals da.SiteId
                                     where sm.MenuType == dataTypeString
                                       && a.Id == applicationId
                                       && ss.Status == "Live"
                                       && cleanedUpDeliveryZoneFilter.StartsWith(da.DeliveryArea1.ToUpper().Trim())
                                     select new
                                     {
                                         s.ID,
                                         s.EstimatedDeliveryTime,
                                         s.EstimatedCollectionTime,
                                         s.StoreConnected,
                                         sm.Version,
                                         s.ExternalSiteName,
                                         s.ExternalId,
                                         s.LicenceKey,
                                         s.Address.Lat,
                                         s.Address.Long,
                                         s.AndroID
                                     };

                    var siteEntities = sitesQuery.ToList();

                    foreach (var siteEntity in siteEntities)
                    {
                        bool returnSite = true;

                        // Do we need to filter by distance i.e. only return the closest X stores?
                        if (filterByMaxDistance != null && filterByLongitude != null && filterByLatitude != null)
                        {
                            // Do we have the location of the site?
                            if (siteEntity.Lat == null ||
                                siteEntity.Long == null ||
                                filterByLatitude == null ||
                                filterByLongitude == null ||
                                !double.TryParse(siteEntity.Long, out double storeLongitude) ||
                                !double.TryParse(siteEntity.Lat, out double storeLatitude))
                            {
                                // Don't know where the site is so don't return it
                                returnSite = false;
                            }
                            else
                            {
                                // Calculate the distance between the site and the customer
                                double distance = SpacialHelper.CalcDistanceBetweenTwoPoints(filterByLongitude.Value, filterByLatitude.Value, storeLongitude, storeLatitude);

                                // Is the site within X km of the customer?
                                if (distance > filterByMaxDistance)
                                {
                                    // Out of range - don't return the site
                                    returnSite = false;
                                }
                            }
                        }

                        if (returnSite)
                        {
                            AndroCloudDataAccess.Domain.Site site = new AndroCloudDataAccess.Domain.Site();
                            site.Id = siteEntity.ID;
                            site.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                            site.EstCollTime = siteEntity.EstimatedCollectionTime.GetValueOrDefault(0);
                            site.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                            site.MenuVersion = siteEntity.Version.GetValueOrDefault(0);
                            site.Name = siteEntity.ExternalSiteName;
                            site.ExternalId = siteEntity.ExternalId;
                            site.LicenceKey = siteEntity.LicenceKey;
                            site.AndroId = siteEntity.AndroID;
                            GetLoyaltyConfiguration(site, siteEntity);

                            sites.Add(site);
                        }
                    }
                }
            }

            return "";
        }

        public string GetById(Guid siteId, out Site site)
        {
            site = null;

            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var sitesQuery = from s in acsEntities.Sites.Include(e => e.SiteLoyalties)
                                 join ss in acsEntities.SiteStatuses
                                   on s.SiteStatusID equals ss.ID
                                 where s.ID == siteId
                                   && ss.Status == "Live"
                                 select s;
                Model.Site siteEntity = sitesQuery.FirstOrDefault();

                if (siteEntity != null)
                {
                    site = new AndroCloudDataAccess.Domain.Site();
                    site.Id = siteEntity.ID;
                    site.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                    site.EstCollTime = siteEntity.EstimatedCollectionTime.GetValueOrDefault(0);
                    site.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                    site.MenuVersion = 0;
                    site.Name = siteEntity.ExternalSiteName;
                    site.ExternalId = siteEntity.ExternalId;
                    site.LicenceKey = siteEntity.LicenceKey;
                    site.AndroId = siteEntity.AndroID;
                    GetLoyaltyConfiguration(site, siteEntity);
                }
            }

            return "";
        }

        public string GetByExternalSiteId(string externalSiteId, out Site site)
        {
            site = null;

            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var sitesQuery = from s in acsEntities.Sites.Include(e => e.SiteLoyalties)
                                 join ss in acsEntities.SiteStatuses
                                   on s.SiteStatusID equals ss.ID
                                 where s.ExternalId == externalSiteId
                                   && ss.Status == "Live"
                                 select s;
                Model.Site siteEntity = sitesQuery.FirstOrDefault();

                if (siteEntity != null)
                {
                    site = new AndroCloudDataAccess.Domain.Site();
                    site.Id = siteEntity.ID;
                    site.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                    site.EstCollTime = siteEntity.EstimatedCollectionTime.GetValueOrDefault(0);
                    site.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                    site.MenuVersion = 0;
                    site.Name = siteEntity.ExternalSiteName;
                    site.ExternalId = siteEntity.ExternalId;
                    site.LicenceKey = siteEntity.LicenceKey;
                    site.AndroId = siteEntity.AndroID;
                    GetLoyaltyConfiguration(site, siteEntity);
                }
            }

            return "";
        }

        public string GetByAndromedaSiteIdAndLive(int andromedaSiteId, out Site site)
        {
            site = null;

            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var sitesQuery = from s in acsEntities.Sites.Include(e => e.SiteLoyalties)
                                 join ss in acsEntities.SiteStatuses
                                   on s.SiteStatusID equals ss.ID
                                 where s.AndroID == andromedaSiteId
                                   && ss.Status == "Live"
                                 select s;
                Model.Site siteEntity = sitesQuery.FirstOrDefault();

                if (siteEntity != null)
                {
                    site = new AndroCloudDataAccess.Domain.Site();
                    site.Id = siteEntity.ID;
                    site.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                    site.EstCollTime = siteEntity.EstimatedCollectionTime.GetValueOrDefault(0);
                    site.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                    site.MenuVersion = 0;
                    site.Name = siteEntity.ExternalSiteName;
                    site.ExternalId = siteEntity.ExternalId;
                    site.LicenceKey = siteEntity.LicenceKey;
                    site.AndroId = siteEntity.AndroID;
                    GetLoyaltyConfiguration(site, siteEntity);
                }
            }

            return "";
        }

        public string GetByAndromedaSiteId(int andromedaSiteId, out Site site)
        {
            site = null;

            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var sitesQuery = from s in acsEntities.Sites.Include(e => e.SiteLoyalties)
                                 join ss in acsEntities.SiteStatuses
                                   on s.SiteStatusID equals ss.ID
                                 where s.AndroID == andromedaSiteId
                                 select s;
                Model.Site siteEntity = sitesQuery.FirstOrDefault();

                if (siteEntity != null)
                {
                    site = new AndroCloudDataAccess.Domain.Site();
                    site.Id = siteEntity.ID;
                    site.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                    site.EstCollTime = siteEntity.EstimatedCollectionTime.GetValueOrDefault(0);
                    site.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                    site.MenuVersion = 0;
                    site.Name = siteEntity.ExternalSiteName;
                    site.ExternalId = siteEntity.ExternalId;
                    site.LicenceKey = siteEntity.LicenceKey;
                    site.AndroId = siteEntity.AndroID;
                    GetLoyaltyConfiguration(site, siteEntity);
                }
            }

            return "";
        }

        public string GetByIdAndApplication(int applicationId, Guid siteId, out Site site)
        {
            site = null;

            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var sitesQuery = from s in acsEntities.Sites.Include(e => e.SiteLoyalties)
                                 join acss in acsEntities.ACSApplicationSites
                                   on s.ID equals acss.SiteId
                                 join a in acsEntities.ACSApplications
                                   on acss.ACSApplicationId equals a.Id
                                 join sm in acsEntities.SiteMenus
                                   on s.ID equals sm.SiteID
                                 join ss in acsEntities.SiteStatuses
                                   on s.SiteStatusID equals ss.ID
                                 where a.Id == applicationId
                                   && s.ID == siteId
                                   && ss.Status == "Live"
                                 select new 
                                 { 
                                     s.ID, 
                                     s.EstimatedDeliveryTime,
                                     s.EstimatedCollectionTime, 
                                     s.StoreConnected, 
                                     sm.Version, 
                                     s.ExternalSiteName, 
                                     s.ExternalId, 
                                     s.LicenceKey, 
                                     s.AndroID, 
                                     s.SiteLoyalties 
                                 };

                var siteEntity = sitesQuery.FirstOrDefault();

                if (siteEntity != null)
                {
                    site = new AndroCloudDataAccess.Domain.Site();
                    site.Id = siteEntity.ID;
                    site.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                    site.EstCollTime = siteEntity.EstimatedCollectionTime.GetValueOrDefault(0);
                    site.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                    site.MenuVersion = siteEntity.Version.GetValueOrDefault(0);
                    site.Name = siteEntity.ExternalSiteName;
                    site.ExternalId = siteEntity.ExternalId;
                    site.LicenceKey = siteEntity.LicenceKey;
                    site.AndroId = siteEntity.AndroID;
                    GetLoyaltyConfiguration(site, siteEntity);
                }
            }

            return "";
        }

        public string Update(int andromedaSiteId, int etd)
        {
            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var acsQuery = from o in acsEntities.Sites
                               where o.AndroID == andromedaSiteId
                               select o;

                var acsQueryEntity = acsQuery.FirstOrDefault();

                // Update the site record
                if (acsQueryEntity != null)
                {
                    acsQueryEntity.EstimatedDeliveryTime = etd;
                    acsEntities.SaveChanges();
                }
            }

            return "";
        }

        public IEnumerable<AcsSiteDetails> GetSitesWithDetailsByApplicationId(string applicationId)
        {
            using (var acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, ConnectionStringOverride);

                ACSApplication acsApplication = acsEntities.ACSApplications
                    .Where(x => x.ExternalApplicationId == applicationId)
                    .Include(x => x.ACSApplicationSites.Select(a => a.Site.OpeningHours))
                    .Include(x => x.ACSApplicationSites.Select(a => a.Site.Address))
                    .Include(x => x.ACSApplicationSites.Select(a => a.Site.DeliveryAreas))
                    .FirstOrDefault();

                if (acsApplication == null)
                {
                    throw new NotExistingEntityException("Entity with this applicationId doesn't exist");
                }

                var sites = new List<AcsSiteDetails>();

                foreach (ACSApplicationSite acsApplicationSite in acsApplication.ACSApplicationSites)
                {
                    var address = new AcsSiteAddress
                    {
                        Dps = acsApplicationSite.Site.Address.DPS,
                        Latitude = acsApplicationSite.Site.Address.Lat,
                        Locality = acsApplicationSite.Site.Address.Locality,
                        Longitude = acsApplicationSite.Site.Address.Long,
                        Org1 = acsApplicationSite.Site.Address.Org1,
                        Org2 = acsApplicationSite.Site.Address.Org2,
                        Org3 = acsApplicationSite.Site.Address.Org3,
                        Prem1 = acsApplicationSite.Site.Address.Prem1,
                        Prem2 = acsApplicationSite.Site.Address.Prem2,
                        Prem3 = acsApplicationSite.Site.Address.Prem3,
                        Prem4 = acsApplicationSite.Site.Address.Prem4,
                        Prem5 = acsApplicationSite.Site.Address.Prem5,
                        Prem6 = acsApplicationSite.Site.Address.Prem6
                    };

                    List<AcsOpeningHour> openingHours = acsApplicationSite.Site.OpeningHours
                        .Select(
                            siteOpeningHour => new AcsOpeningHour
                            {
                                Day = siteOpeningHour.Day.Description,
                                StartTime = siteOpeningHour.TimeStart,
                                EndTime = siteOpeningHour.TimeEnd,
                                OpenAllDay = siteOpeningHour.OpenAllDay
                            })
                        .ToList();

                    var site = new AcsSiteDetails
                    {
                        Name = acsApplicationSite.Site.ExternalSiteName,
                        Address = address,
                        OpeningHours = openingHours,
                        SiteId = acsApplicationSite.Site.ExternalId,
                        DeliveryZones = acsApplicationSite.Site.DeliveryAreas.Select(da => da.DeliveryArea1).ToList(),
                        Phone = acsApplicationSite.Site.Telephone,
                        TimeZone = acsApplicationSite.Site.TimeZone,
                        EstimatedCollectionDuration = acsApplicationSite.Site.EstimatedCollectionTime,
                        EstimatedDeliveryDuration = acsApplicationSite.Site.EstimatedDeliveryTime
                    };

                    sites.Add(site);
                }

                return sites;
            }
        }

        private void GetLoyaltyConfiguration(Site site, dynamic siteEntity)
        {
            site.SiteLoyalties = new List<AndroCloudDataAccess.Domain.SiteLoyalty>();
            try
            {
                if (siteEntity.GetType().GetProperty("SiteLoyalties") != null && siteEntity.SiteLoyalties != null)
                {
                    foreach (var config in siteEntity.SiteLoyalties)
                    {
                        AndroCloudDataAccess.Domain.SiteLoyalty siteConfig = new AndroCloudDataAccess.Domain.SiteLoyalty();
                        siteConfig.Id = config.Id;
                        siteConfig.RawConfiguration = config.Configuration;
                        siteConfig.ProviderName = config.ProviderName;
                        
                        if (!string.IsNullOrEmpty(config.Configuration))
                        {
                            siteConfig.Configuration = JsonConvert.DeserializeObject<AndromedaLoyaltyConfiguration>(config.Configuration);
                        }
                        if (siteConfig.Configuration != null && siteConfig.Configuration.Enabled.GetValueOrDefault())
                        {
                            site.SiteLoyalties.Add(siteConfig);
                        }
                    }
                }
            }
            catch (Exception)
            { 
                // log - 
            }
        }
    }
}
