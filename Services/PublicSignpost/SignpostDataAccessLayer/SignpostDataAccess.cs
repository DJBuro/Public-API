using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SignpostDataAccessLayer
{
    public class SignpostDataAccess : ISignpostDataAccess
    {
        public string GetServicesByApplicationId(string applicationId, out List<Models.HostV2> hosts)
        {
            hosts = new List<Models.HostV2>();

            using (SignpostEntities signpostEntities = new SignpostEntities())
            {
                //               DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var servicesQuery = from service in signpostEntities.Services
                                   join environment in signpostEntities.Environments
                                        on service.EnvironmentId equals environment.Id
                                   join acsApplication in signpostEntities.ACSApplications
                                        on environment.Id equals acsApplication.EnvironmentId
                                   where acsApplication.ExternalApplicationId == applicationId
                                   select service;

                if (servicesQuery != null)
                {
                    foreach (Service service in servicesQuery)
                    {
                        Models.HostV2 hostV2 = new Models.HostV2()
                        {
                            Id = Guid.Empty,
                            Order = service.Order,
                            Name = service.Name,
                            Url = service.Url,
                            Version = service.Version,
                            Type = service.ServiceType.Description,
                            TypeId = service.ServiceType.Id.ToString()
                        };

                        hosts.Add(hostV2);
                    }
                }
            }

            return "";
        }

        public string GetServicesBySiteId(int andromedaSiteId, out List<Models.HostV2> hosts)
        {
            hosts = new List<Models.HostV2>();

            using (SignpostEntities signpostEntities = new SignpostEntities())
            {
                //               DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var servicesQuery = from server in signpostEntities.Services
                                   join environment in signpostEntities.Environments
                                        on server.EnvironmentId equals environment.Id
                                   join acsApplication in signpostEntities.ACSApplications
                                        on environment.Id equals acsApplication.EnvironmentId
                                   join acsApplicationSite in signpostEntities.ACSApplicationSites
                                        on acsApplication.Id equals acsApplicationSite.ACSApplicationId
                                   where acsApplicationSite.AndromedaSiteId == andromedaSiteId
                                   select server;

                if (servicesQuery != null)
                {
                    // In theory a store can be a part of multiple applications, each in a different or the same environments
                    // Therefore the same server may be returned multiple times so make sure we only return each service once
                    Dictionary<Guid, Service> gotServices = new Dictionary<Guid, Service>();
                    foreach (Service service in servicesQuery)
                    {
                        Service checkService = null;
                        if (!gotServices.TryGetValue(service.Id, out checkService))
                        {
                            Models.HostV2 hostV2 = new Models.HostV2()
                            {
                                Id = Guid.Empty,
                                Order = service.Order,
                                Name = service.Name,
                                Url = service.Url,
                                Version = service.Version,
                                Type = service.ServiceType.Description,
                                TypeId = service.ServiceType.Id.ToString()
                            };

                            hosts.Add(hostV2);
                        }
                    }
                }
            }

            return "";
        }

        public string GetLegacyPrivateHostsBySiteId(int andromedaSiteId, out List<Models.HostV1> hosts)
        {
            hosts = new List<Models.HostV1>();

            using (SignpostEntities signpostEntities = new SignpostEntities())
            {
                //               DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var legacyHostQuery = from legacyHost in signpostEntities.LegacyHosts
                                    join privateService in signpostEntities.Services
                                        on legacyHost.ACSPrivateServiceId equals privateService.Id
                                    join signalRService in signpostEntities.Services
                                        on legacyHost.ACSPrivateServiceId equals signalRService.Id
                                    join privateEnvironment in signpostEntities.Environments
                                        on privateService.EnvironmentId equals privateEnvironment.Id
                                    join acsApplication in signpostEntities.ACSApplications
                                         on privateEnvironment.Id equals acsApplication.EnvironmentId
                                    join acsApplicationSite in signpostEntities.ACSApplicationSites
                                         on acsApplication.Id equals acsApplicationSite.ACSApplicationId
                                    where acsApplicationSite.AndromedaSiteId == andromedaSiteId
                                    select new 
                                    {
                                        Order = legacyHost.Order,
                                        Url = privateService.Url,
                                        SignalRUrl = signalRService.Url
                                    };

                if (legacyHostQuery != null)
                {
                    foreach (var legacyHost in legacyHostQuery)
                    {
                        Models.HostV1 hostV1 = new Models.HostV1()
                        {
                            Order = legacyHost.Order,
                            Url = legacyHost.Url,
                            SignalRUrl = legacyHost.SignalRUrl
                        };

                        hosts.Add(hostV1);
                    }
                }
            }

            return "";
        }

        public string GetLegacyPublicHostsBySiteId(int andromedaSiteId, out List<Models.HostV2> hosts)
        {
            hosts = new List<Models.HostV2>();

            using (SignpostEntities signpostEntities = new SignpostEntities())
            {
                //               DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var servicesQuery = from service in signpostEntities.Services
                                    join environment in signpostEntities.Environments
                                         on service.EnvironmentId equals environment.Id
                                    join acsApplication in signpostEntities.ACSApplications
                                         on environment.Id equals acsApplication.EnvironmentId
                                    join acsApplicationSite in signpostEntities.ACSApplicationSites
                                         on acsApplication.Id equals acsApplicationSite.ACSApplicationId
                                    join legacyHost in signpostEntities.LegacyHosts
                                         on service.Id equals legacyHost.ACSPrivateServiceId

                                    where acsApplicationSite.AndromedaSiteId == andromedaSiteId
                                    select service;

                if (servicesQuery != null)
                {
                    // In theory a store can be a part of multiple applications, each in a different or the same environments
                    // Therefore the same server may be returned multiple times so make sure we only return each service once
                    Dictionary<Guid, Service> gotServices = new Dictionary<Guid, Service>();
                    foreach (Service service in servicesQuery)
                    {
                        Service checkService = null;
                        if (!gotServices.TryGetValue(service.Id, out checkService))
                        {
                            Models.HostV2 hostV2 = new Models.HostV2()
                            {
                                Id = Guid.Empty,
                                Order = service.Order,
                                Name = service.Name,
                                Url = service.Url,
                                Version = service.Version,
                                Type = service.ServiceType.Description,
                                TypeId = service.ServiceType.Id.ToString()
                            };

                            hosts.Add(hostV2);
                        }
                    }
                }
            }

            return "";
        }

        public string GetDataVersion(out int version)
        {
            version = -1;

            using (SignpostEntities signpostEntities = new SignpostEntities())
            {
                //               DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

                var settingsQuery = from settings in signpostEntities.Settings
                                    select settings;

                Setting setting = settingsQuery.FirstOrDefault();

                if (setting != null)
                {
                    version = int.Parse(setting.Value);
                }
            }

            return "";
        }

        public string AddUpdateACSApplications(int fromVersion, int toVersion, List<SignpostDataAccessLayer.Models.ACSApplication> acsApplications)
        {
            using (System.Transactions.TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (SignpostEntities signpostEntities = new SignpostEntities())
                {
                    signpostEntities.Database.Connection.Open();

                    // Update the data version.  We're using this as a guard to prevent multiple simultaneous syncs
                    bool success = this.SetVersion(fromVersion, toVersion, signpostEntities);

                    // Did we successfully update the version
                    if (!success)
                    {
                        // Someone probably sneaked in and applied another sync
                        return "SetVersion failed.  From:" + fromVersion + " to: " + toVersion;
                    }

                    foreach (SignpostDataAccessLayer.Models.ACSApplication acsApplication in acsApplications)
                    {
                        // Delete the acs application sites
                        this.DeleteACSApplicationSites(acsApplication, signpostEntities);

                        // Delete the acs application
                        this.DeleteACSApplication(acsApplication, signpostEntities);

                        // Insert the acs application
                        this.InsertACSApplication(acsApplication, signpostEntities);

                        // Insert the acs application sites
                        this.InsertACSApplicationSites(acsApplication, signpostEntities);
                    }
                }
            }

            return "";
        }

        private bool SetVersion(int fromVersion, int toVersion, SignpostEntities signpostEntities)
        {
            // We need to do something a little unusual here.  This signpost server has a specific version of the signpost database (data not schema).
            // When there are changes to the master signpost database, the sync works out what data has changed between this signpost database version and the master db version.
            // However,  it's possible for multiple people to change the master db at the same time which could result in multiple simultanous data syncs.
            // To prevent data loss these syncs are done in transactions.  However, we still need to check that the sync is being applied to the correct version of the database.
            // We need to check that the database version matches the database version that the upgrade is for.
            // To do this we need to do something that doesn't appear to be possible in EF - "update where"

            // Note that the current database version is stored in the settings table.  The setting values are strings.

            // Get a SQL connection from EF
            SqlConnection sqlConnection = (SqlConnection)signpostEntities.Database.Connection;

            // We're gonna do this in a SQL command
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;
            command.CommandText = "UPDATE [Settings] SET [Value] = '" + toVersion + "' where [name] = 'dataversion' and [Value] = '" + fromVersion + "'";

            // We need to check that the version was actually updated.  If someone else has sneaked in and done a sync then rowsAffected will be zero
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected != 1)
            {
                return false;
            }

            return true;
        }

        private void DeleteACSApplicationSites(
            SignpostDataAccessLayer.Models.ACSApplication acsApplication, SignpostEntities signpostEntities)
        {
            foreach (var acsApplicationSiteEntity in signpostEntities.ACSApplicationSites.Where(e => e.ACSApplicationId == acsApplication.id))
            {
                signpostEntities.ACSApplicationSites.Remove(acsApplicationSiteEntity);
            }

            signpostEntities.SaveChanges();
        }

        private void DeleteACSApplication(
            SignpostDataAccessLayer.Models.ACSApplication acsApplication, SignpostEntities signpostEntities)
        {
            var acsApplicationQuery = from a in signpostEntities.ACSApplications
                                      where a.Id == acsApplication.id
                                      select a;

            var acsApplicationEntity = acsApplicationQuery.FirstOrDefault();

            if (acsApplicationEntity != null)
            {
                signpostEntities.ACSApplications.Remove(acsApplicationEntity);
            }

            signpostEntities.SaveChanges();
        }

        private void InsertACSApplication(
            SignpostDataAccessLayer.Models.ACSApplication acsApplication, SignpostEntities signpostEntities)
        {
            ACSApplication acsApplicationEntity = new ACSApplication()
            {
                Id = acsApplication.id,
                EnvironmentId = acsApplication.environmentId,
                ExternalApplicationId = acsApplication.externalApplicationId,
                Name = acsApplication.name
            };

            signpostEntities.ACSApplications.Add(acsApplicationEntity);

            signpostEntities.SaveChanges();
        }

        private void InsertACSApplicationSites(
            SignpostDataAccessLayer.Models.ACSApplication acsApplication, SignpostEntities signpostEntities)
        {
            foreach (int andromedaSiteId in acsApplication.acsApplicationSites)
            {
                ACSApplicationSite acsApplicationSiteEntity = new ACSApplicationSite()
                {
                    Id = Guid.NewGuid(),
                    ACSApplicationId = acsApplication.id,
                    AndromedaSiteId = andromedaSiteId
                };

                signpostEntities.ACSApplicationSites.Add(acsApplicationSiteEntity);
            }

            signpostEntities.SaveChanges();
        }
    }
}
