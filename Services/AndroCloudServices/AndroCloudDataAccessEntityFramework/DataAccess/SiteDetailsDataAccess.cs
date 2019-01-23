using System;
using System.Data;
using System.Linq;
using AndroCloudDataAccess.DataAccess;
using System.Collections.Generic;
using AndroCloudDataAccessEntityFramework.Model;
using AndroCloudDataAccess.Domain;
using AndroCloudHelper;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Newtonsoft.Json;

namespace AndroCloudDataAccessEntityFramework.DataAccess
{
    public class SiteDetailsDataAccess : ISiteDetailsDataAccess
    {
        public string ConnectionStringOverride { get; set; }

        public string GetBySiteId(Guid siteId, DataTypeEnum dataType, out AndroCloudDataAccess.Domain.SiteDetails siteDetails)
        {
            siteDetails = null;

            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var sitesQuery = from site in acsEntities.Sites.Include(e => e.SiteLoyalties)
                                 join siteStatus in acsEntities.SiteStatuses
                                   on site.SiteStatusID equals siteStatus.ID
                                 join spp in acsEntities.StorePaymentProviders
                                   on site.StorePaymentProviderID equals spp.ID
                                 into spp2
                                 from spp3 in spp2.DefaultIfEmpty()
                                 where
                                    site.ID == siteId
                                    && siteStatus.Status == "Live"
                                 select new
                                 {
                                     site.ID,
                                     site.ExternalId,
                                     site.ExternalSiteName,
                                     site.StoreConnected,
                                     site.EstimatedDeliveryTime,
                                     site.TimeZone,
                                     site.SiteMenus,
                                     site.Address,
                                     site.OpeningHours,
                                     site.Telephone,
                                     site.SiteLoyalties,
                                     ProviderName = (spp3 == null ? "" : spp3.ProviderName),
                                     ClientId = (spp3 == null ? "" : spp3.ClientId),
                                     ClientPassword = (spp3 == null ? "" : spp3.ClientPassword)
                                 };
                 var siteEntity = sitesQuery.FirstOrDefault();

                // Create a serializable SiteDetails object
                siteDetails = new AndroCloudDataAccess.Domain.SiteDetails();
                siteDetails.Id = siteEntity.ID;
                siteDetails.ExternalId = siteEntity.ExternalId;
                siteDetails.Name = siteEntity.ExternalSiteName;
                siteDetails.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                siteDetails.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                siteDetails.TimeZone = siteEntity.TimeZone;
                siteDetails.PaymentProvider = siteEntity.ProviderName;
                siteDetails.PaymentClientId = siteEntity.ClientId;
                siteDetails.PaymentClientPassword = siteEntity.ClientPassword;
                siteDetails.Phone = siteEntity.Telephone;

                // Get the menu version for the requested data type (JSON or XML)
                foreach (Model.SiteMenu siteMenu in siteEntity.SiteMenus)
                {
                    if (siteMenu.MenuType == dataType.ToString() && siteMenu.Version.HasValue)
                    {
                        siteDetails.MenuVersion = siteMenu.Version.Value;
                        break;
                    }
                }

                // Address
                if (siteEntity.Address != null)
                {
                    Model.Address dbAddress = siteEntity.Address;

                    siteDetails.Address = new AndroCloudDataAccess.Domain.Address();
                    siteDetails.Address.Country = dbAddress.Country.CountryName;
                    siteDetails.Address.County = dbAddress.County;
                    siteDetails.Address.Dps = dbAddress.DPS;
                    siteDetails.Address.Lat = dbAddress.Lat;
                    siteDetails.Address.Locality = dbAddress.Locality;
                    siteDetails.Address.Long = dbAddress.Long;
                    siteDetails.Address.Org1 = dbAddress.Org1;
                    siteDetails.Address.Org2 = dbAddress.Org2;
                    siteDetails.Address.Org3 = dbAddress.Org3;
                    siteDetails.Address.Postcode = dbAddress.PostCode;
                    siteDetails.Address.Prem1 = dbAddress.Prem1;
                    siteDetails.Address.Prem2 = dbAddress.Prem2;
                    siteDetails.Address.Prem3 = dbAddress.Prem3;
                    siteDetails.Address.Prem4 = dbAddress.Prem4;
                    siteDetails.Address.Prem5 = dbAddress.Prem5;
                    siteDetails.Address.Prem6 = dbAddress.Prem6;
                    siteDetails.Address.RoadName = dbAddress.RoadName;
                    siteDetails.Address.RoadNum = dbAddress.RoadNum;
                    siteDetails.Address.Town = dbAddress.Town;
                }

                // Opening hours
                siteDetails.OpeningHours = new List<TimeSpanBlock>();
                if (siteEntity.OpeningHours != null)
                {
                    foreach (OpeningHour openingHour in siteEntity.OpeningHours)
                    {
                        TimeSpanBlock timeSpanBlock = new TimeSpanBlock();
                        timeSpanBlock.ID = openingHour.ID;
                        timeSpanBlock.Day = openingHour.Day.Description;
                        timeSpanBlock.StartTime = openingHour.TimeStart.Hours.ToString("00") + ":" + openingHour.TimeStart.Minutes.ToString("00");
                        timeSpanBlock.EndTime = openingHour.TimeEnd.Hours.ToString("00") + ":" + openingHour.TimeEnd.Minutes.ToString("00");
                        timeSpanBlock.OpenAllDay = openingHour.OpenAllDay;

                        siteDetails.OpeningHours.Add(timeSpanBlock);
                    }
                }
                siteDetails.SiteLoyalties = new List<AndroCloudDataAccess.Domain.SiteLoyalty>();
                if (siteEntity.SiteLoyalties != null)
                {
                    foreach (var config in siteEntity.SiteLoyalties)
                    {
                        AndroCloudDataAccess.Domain.SiteLoyalty siteConfig = new AndroCloudDataAccess.Domain.SiteLoyalty();
                        siteConfig.Id = config.Id;
                        //siteConfig.SiteId = config.SiteId;
                        siteConfig.RawConfiguration = config.Configuration;
                        siteConfig.ProviderName = config.ProviderName;
                        siteConfig.RawConfiguration = config.Configuration;
                        var t =  JsonConvert.DeserializeObject(string.IsNullOrWhiteSpace(config.Configuration) ? "{}" : config.Configuration);
                        

                        if(config.ProviderName.Equals("Andromeda", StringComparison.InvariantCultureIgnoreCase))
                        {
                            siteConfig.Configuration = JsonConvert.DeserializeObject<AndromedaLoyaltyConfiguration>(string.IsNullOrWhiteSpace(config.Configuration) ? "{}" : config.Configuration);
                        }
                        else 
                        {
                            siteConfig.Configuration = new AndromedaLoyaltyConfiguration(){ };
                        }
                        
                        if (siteConfig.Configuration != null && siteConfig.Configuration.Enabled.GetValueOrDefault())
                        {
                           siteDetails.SiteLoyalties.Add(siteConfig);
                        }
                    }
                }
            }

            return "";
        }

        public string GetBySiteId3(Guid siteId, DataTypeEnum dataType, out AndroCloudDataAccess.Domain.SiteDetails3 siteDetails)
        {
            siteDetails = null;

            using (ACSEntities acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var sitesQuery = from site in acsEntities.Sites.Include(e => e.SiteLoyalties)
                                 join siteStatus in acsEntities.SiteStatuses
                                   on site.SiteStatusID equals siteStatus.ID
                                 join spp in acsEntities.StorePaymentProviders
                                   on site.StorePaymentProviderID equals spp.ID
                                 into spp2
                                 from spp3 in spp2.DefaultIfEmpty()
                                 where
                                    site.ID == siteId
                                    && siteStatus.Status == "Live"
                                 select new
                                 {
                                     site.ID,
                                     site.ExternalId,
                                     site.ExternalSiteName,
                                     site.StoreConnected,
                                     site.EstimatedDeliveryTime,
                                     site.TimeZone,
                                     site.SiteMenus,
                                     site.Address,
                                     site.OpeningHours,
                                     site.SiteOccasionTimes,
                                     site.Telephone,
                                     site.SiteLoyalties,
                                     ProviderName = (spp3 == null ? "" : spp3.ProviderName),
                                     ClientId = (spp3 == null ? "" : spp3.ClientId),
                                     ClientPassword = (spp3 == null ? "" : spp3.ClientPassword)
                                 };
                var siteEntity = sitesQuery.FirstOrDefault();

                // Create a serializable SiteDetails object
                siteDetails = new AndroCloudDataAccess.Domain.SiteDetails3();
                siteDetails.Id = siteEntity.ID;
                siteDetails.ExternalId = siteEntity.ExternalId;
                siteDetails.Name = siteEntity.ExternalSiteName;
                siteDetails.IsOpen = siteEntity.StoreConnected.GetValueOrDefault(false);
                siteDetails.EstDelivTime = siteEntity.EstimatedDeliveryTime.GetValueOrDefault(0);
                siteDetails.TimeZone = siteEntity.TimeZone;
                siteDetails.PaymentProvider = siteEntity.ProviderName;
                siteDetails.PaymentClientId = siteEntity.ClientId;
                siteDetails.PaymentClientPassword = siteEntity.ClientPassword;
                siteDetails.Phone = siteEntity.Telephone;

                // Get the menu version for the requested data type (JSON or XML)
                foreach (Model.SiteMenu siteMenu in siteEntity.SiteMenus)
                {
                    if (siteMenu.MenuType == dataType.ToString() && siteMenu.Version.HasValue)
                    {
                        siteDetails.MenuVersion = siteMenu.Version.Value;
                        break;
                    }
                }

                // Address
                if (siteEntity.Address != null)
                {
                    Model.Address dbAddress = siteEntity.Address;

                    siteDetails.Address = new AndroCloudDataAccess.Domain.Address();
                    siteDetails.Address.Country = dbAddress.Country.CountryName;
                    siteDetails.Address.County = dbAddress.County;
                    siteDetails.Address.Dps = dbAddress.DPS;
                    siteDetails.Address.Lat = dbAddress.Lat;
                    siteDetails.Address.Locality = dbAddress.Locality;
                    siteDetails.Address.Long = dbAddress.Long;
                    siteDetails.Address.Org1 = dbAddress.Org1;
                    siteDetails.Address.Org2 = dbAddress.Org2;
                    siteDetails.Address.Org3 = dbAddress.Org3;
                    siteDetails.Address.Postcode = dbAddress.PostCode;
                    siteDetails.Address.Prem1 = dbAddress.Prem1;
                    siteDetails.Address.Prem2 = dbAddress.Prem2;
                    siteDetails.Address.Prem3 = dbAddress.Prem3;
                    siteDetails.Address.Prem4 = dbAddress.Prem4;
                    siteDetails.Address.Prem5 = dbAddress.Prem5;
                    siteDetails.Address.Prem6 = dbAddress.Prem6;
                    siteDetails.Address.RoadName = dbAddress.RoadName;
                    siteDetails.Address.RoadNum = dbAddress.RoadNum;
                    siteDetails.Address.Town = dbAddress.Town;
                }

                // Extract the service times
                siteDetails.ServiceTimes = this.ExtractOccasionTimes(siteEntity.SiteOccasionTimes);

                // Opening hours
                siteDetails.OpeningHours = new List<TimeSpanBlock>();
                if (siteEntity.OpeningHours != null)
                {
                    foreach (OpeningHour openingHour in siteEntity.OpeningHours)
                    {
                        TimeSpanBlock timeSpanBlock = new TimeSpanBlock();
                        timeSpanBlock.ID = openingHour.ID;
                        timeSpanBlock.Day = openingHour.Day.Description;
                        timeSpanBlock.StartTime = openingHour.TimeStart.Hours.ToString("00") + ":" + openingHour.TimeStart.Minutes.ToString("00");
                        timeSpanBlock.EndTime = openingHour.TimeEnd.Hours.ToString("00") + ":" + openingHour.TimeEnd.Minutes.ToString("00");
                        timeSpanBlock.OpenAllDay = openingHour.OpenAllDay;

                        siteDetails.OpeningHours.Add(timeSpanBlock);
                    }
                }

                // Loyalty
                siteDetails.SiteLoyalties = new List<AndroCloudDataAccess.Domain.SiteLoyalty>();
                if (siteEntity.SiteLoyalties != null)
                {
                    foreach (var config in siteEntity.SiteLoyalties)
                    {
                        AndroCloudDataAccess.Domain.SiteLoyalty siteConfig = new AndroCloudDataAccess.Domain.SiteLoyalty();
                        siteConfig.Id = config.Id;
                        //siteConfig.SiteId = config.SiteId;
                        siteConfig.RawConfiguration = config.Configuration;
                        siteConfig.ProviderName = config.ProviderName;
                        siteConfig.RawConfiguration = config.Configuration;
                        var t = JsonConvert.DeserializeObject(string.IsNullOrWhiteSpace(config.Configuration) ? "{}" : config.Configuration);


                        if (config.ProviderName.Equals("Andromeda", StringComparison.InvariantCultureIgnoreCase))
                        {
                            siteConfig.Configuration = JsonConvert.DeserializeObject<AndromedaLoyaltyConfiguration>(string.IsNullOrWhiteSpace(config.Configuration) ? "{}" : config.Configuration);
                        }
                        else
                        {
                            siteConfig.Configuration = new AndromedaLoyaltyConfiguration() { };
                        }

                        if (siteConfig.Configuration != null && siteConfig.Configuration.Enabled.GetValueOrDefault())
                        {
                            siteDetails.SiteLoyalties.Add(siteConfig);
                        }
                    }
                }
            }

            return "";
        }

        public List<ServiceTimes> ExtractOccasionTimes(ICollection<SiteOccasionTime> siteOccasionTimes)
        {
            List<ServiceTimes> serviceTimes = new List<ServiceTimes>();

            // Delivery service times
            ServiceTimes deliveryServiceTimes = new ServiceTimes() { Occasion="delivery" };
            serviceTimes.Add(deliveryServiceTimes);

            // Collection service times
            ServiceTimes collectionServiceTimes = new ServiceTimes() { Occasion = "collection" };
            serviceTimes.Add(collectionServiceTimes);

            // Dine in service times
            ServiceTimes dineInServiceTimes = new ServiceTimes() { Occasion = "dinein" };
            serviceTimes.Add(dineInServiceTimes);

            // Distribute the service times between occasions
            foreach (SiteOccasionTime siteOccasionTime in siteOccasionTimes)
            {
                // What occasions are these occasion times for?
                List<ServiceTimes> serviceTimeOccasions = new List<ServiceTimes>();
                string[] serviceTimeOccasionNames = siteOccasionTime.Occasions.Split(',');
                foreach (string occasionName in serviceTimeOccasionNames)
                {
                    if (occasionName.ToUpper() == "DELIVERY")
                    {
                        serviceTimeOccasions.Add(deliveryServiceTimes);
                    }
                    else if (occasionName.ToUpper() == "COLLECTION")
                    {
                        serviceTimeOccasions.Add(collectionServiceTimes);
                    }
                    else if (occasionName.ToUpper() == "DINE IN")
                    {
                        serviceTimeOccasions.Add(dineInServiceTimes);
                    }
                }

                // Extract the occasion times
                if (!String.IsNullOrEmpty(siteOccasionTime.RecurrenceRule))
                {
                    this.AddOccasionTimesToOccasions(siteOccasionTime, serviceTimeOccasions);
                }
            }

            return serviceTimes;
        }

        private void AddOccasionTimesToOccasions(SiteOccasionTime siteOccasionTime, List<ServiceTimes> serviceTimeOccasions)
        {
            // The rule in the database is a string like this "FREQ=WEEKLY;BYDAY=TU,FR"
            // Take apart the string and represent it an object structure
            string[] recurrenceRuleBlocks = siteOccasionTime.RecurrenceRule.Split(';');
            string frequency = "";
            string[] days = new string[0];

            // Figure out which days this relates to
            this.GetFrequencyDays(recurrenceRuleBlocks, out frequency, out days);

            if (frequency.ToUpper() == "WEEKLY" || frequency.ToUpper() == "DAILY")
            {
                // Happens every week or every day
                TimeSpanBlock3 time = null;

                // Are there opening times?
                if (!siteOccasionTime.IsAllDay)
                {
                    // The MyAndromeda UI does not account for trading days so we may need to fiddle the times around
                    this.CheckTradingDay(siteOccasionTime);

                    // It's not open all day
                    TimeSpan duration = siteOccasionTime.EndUtc - siteOccasionTime.StartUtc;

                    time = new TimeSpanBlock3()
                    {
                        StartTime = siteOccasionTime.StartUtc.Hour.ToString() + ":" + siteOccasionTime.StartUtc.Minute.ToString("00"),
                        DurationMinutes = (int)Math.Floor(duration.TotalMinutes)
                    };
                }

                // Add the service time to each occasion
                foreach (ServiceTimes serviceTimes in serviceTimeOccasions)
                {
                    // Add the service time to the relevant day
                    if (frequency.ToUpper() == "WEEKLY")
                    {
                        // Add the times to the specified days
                        foreach (string day in days)
                        {
                            this.SetDay(day, siteOccasionTime, serviceTimes, time);
                        }
                    }
                    else if (frequency.ToUpper() == "DAILY")
                    {
                        // Add the times to all days
                        this.SetDay(siteOccasionTime, serviceTimes.Monday, time);
                        this.SetDay(siteOccasionTime, serviceTimes.Tuesday, time);
                        this.SetDay(siteOccasionTime, serviceTimes.Wednesday, time);
                        this.SetDay(siteOccasionTime, serviceTimes.Thursday, time);
                        this.SetDay(siteOccasionTime, serviceTimes.Friday, time);
                        this.SetDay(siteOccasionTime, serviceTimes.Saturday, time);
                        this.SetDay(siteOccasionTime, serviceTimes.Sunday, time);
                    }
                }
            }
        }

        private void CheckTradingDay(SiteOccasionTime siteOccasionTime)
        {
            // Is this time for todays trading day
            if (siteOccasionTime.StartUtc.Hour < 6)
            {
                // This time slot actually starts in yesterdays trading day
                if (siteOccasionTime.EndUtc.Hour > 6)
                {
                    // The time slot straddles yesterday and today so we need to split the timeslot
                    siteOccasionTime.StartUtc = new DateTime(siteOccasionTime.StartUtc.Year, siteOccasionTime.StartUtc.Month, siteOccasionTime.StartUtc.Day, 6, 0, 0);
                }
                else
                {
                    // Move the entire time slot to yesterday?
                    // Too Complicated - maybe if it's raised as a bug :)
                }
            }
        }

        private void GetFrequencyDays(string[] recurrenceRuleBlocks, out string frequency, out string[] days)
        {
            frequency = "";
            days = null;

            // Figure out which days this relates to
            foreach (string recurrenceRuleBlock in recurrenceRuleBlocks)
            {
                string[] recurrenceRuleBlockParts = recurrenceRuleBlock.Split('=');
                if (recurrenceRuleBlockParts.Length == 2)
                {
                    if (recurrenceRuleBlockParts[0].ToUpper() == "FREQ")
                    {
                        frequency = recurrenceRuleBlockParts[1];
                    }
                    else if (recurrenceRuleBlockParts[0].ToUpper() == "BYDAY")
                    {
                        days = recurrenceRuleBlockParts[1].Split(',');
                    }
                }
            }
        }

        private void SetDay(string day, SiteOccasionTime siteOccasionTime, ServiceTimes serviceTimes, TimeSpanBlock3 time)
        {
            if (day.ToUpper() == "MO")
            {
                this.SetDay(siteOccasionTime, serviceTimes.Monday, time);
            }
            else if (day.ToUpper() == "TU")
            {
                this.SetDay(siteOccasionTime, serviceTimes.Tuesday, time);
            }
            else if (day.ToUpper() == "WE")
            {
                this.SetDay(siteOccasionTime, serviceTimes.Wednesday, time);
            }
            else if (day.ToUpper() == "TH")
            {
                this.SetDay(siteOccasionTime, serviceTimes.Thursday, time);
            }
            else if (day.ToUpper() == "FR")
            {
                this.SetDay(siteOccasionTime, serviceTimes.Friday, time);
            }
            else if (day.ToUpper() == "SA")
            {
                this.SetDay(siteOccasionTime, serviceTimes.Saturday, time);
            }
            else if (day.ToUpper() == "SU")
            {
                this.SetDay(siteOccasionTime, serviceTimes.Sunday, time);
            }
        }

        private void SetDay(SiteOccasionTime siteOccasionTime, DayServiceTimes dayServiceTimes, TimeSpanBlock3 time)
        {
            // The store is definitely NOT closed all day
            dayServiceTimes.IsClosedAllDay = false;

            if (siteOccasionTime.IsAllDay)
            {
                // The store is open all day
                dayServiceTimes.IsOpenAllDay = true;
            }
            else
            {
                // The store is open at a specfiic time during the day
                dayServiceTimes.Times.Add(time);
            }
        }
    }
}
