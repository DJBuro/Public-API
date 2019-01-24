﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroAdminDataAccess.Domain;
using AndroAdminDataAccess.DataAccess;
using System.Transactions;

namespace AndroAdminDataAccess.EntityFramework.DataAccess
{
    public class AMSServerDAO : IAMSServerDAO
    {
        public string ConnectionStringOverride { get; set; }

        public IList<Domain.AMSServer> GetAll()
        {
            List<Domain.AMSServer> amsServers = new List<Domain.AMSServer>();

             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var table = entitiesContext.AMSServers; 
                var query = table;

                foreach (var entity in query)
                {
                    Domain.AMSServer model = new Domain.AMSServer()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description
                    };

                    amsServers.Add(model);
                }
            }

            return amsServers;
        }

        public void Add(Domain.AMSServer amsServer)
        {
             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                AMSServer entity = new AMSServer()
                {
                    Name = amsServer.Name,
                    Description = amsServer.Description
                };

                entitiesContext.AMSServers.Add(entity);
                entitiesContext.SaveChanges();
            }
        }

        public void Update(Domain.AMSServer amsServer)
        {
             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from s in entitiesContext.AMSServers
                            where amsServer.Id == s.Id
                            select s;

                var entity = query.FirstOrDefault();

                if (entity != null)
                {
                    entity.Name = amsServer.Name;
                    entity.Description = amsServer.Description;

                    entitiesContext.SaveChanges();
                }
            }
        }

        public Domain.AMSServer GetById(int id)
        {
            Domain.AMSServer model = null;

             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from s in entitiesContext.AMSServers
                            where id == s.Id
                            select s;

                var entity = query.FirstOrDefault();

                if (entity != null)
                {
                    model = new Domain.AMSServer()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description
                    };
                }
            }

            return model;
        }

        public Domain.AMSServer GetByName(string name)
        {
            Domain.AMSServer model = null;

             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from s in entitiesContext.AMSServers
                            where name == s.Name
                            select s;

                var entity = query.FirstOrDefault();

                if (entity != null)
                {
                    model = new Domain.AMSServer()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description
                    };
                }
            }

            return model;
        }

        public void Delete(int amsServerId)
        {
             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                // Delete the StoreAMSServerFTPSite objects that reference the AMS server
                var storeAMSServerFTPSiteQuery = from s in entitiesContext.StoreAMSServerFtpSites
                                                 where s.StoreAMSServer.AMSServerId == amsServerId
                                                 select s;

                foreach (var entity in storeAMSServerFTPSiteQuery)
                {
                    entitiesContext.StoreAMSServerFtpSites.Remove(entity);
                }

                // Delete the StoreAMSServer objects that reference the AMS server
                var storeAmsServerQuery = entitiesContext.StoreAMSServers.Where(s => s.AMSServerId == amsServerId);

                foreach (var entity in storeAmsServerQuery)
                {
                    entitiesContext.StoreAMSServers.Remove(entity);
                }

                // Delete the AMS Server
                var amsServerQuery = from s in entitiesContext.AMSServers
                                     where amsServerId == s.Id
                                     select s;

                AMSServer amsServerEntity = amsServerQuery.FirstOrDefault();

                if (amsServerEntity != null)
                {
                    entitiesContext.AMSServers.Remove(amsServerEntity);

                    entitiesContext.SaveChanges();
                }
            }
        }

        public IList<Domain.AMSServer> GetByChainId(int chainId)
        {
            List<Domain.AMSServer> amsServers = new List<Domain.AMSServer>();

            this.GetByChainIdRecursive(chainId, amsServers);

            return amsServers;
        }

        private void GetByChainIdRecursive(int chainId, IList<Domain.AMSServer> amsServers)
        {
            // Get the AMS servers that the specified chain is allowed to access
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from s in entitiesContext.AMSServers
                            join x in entitiesContext.AMSServerChains
                            on s.Id equals x.AMSServerId
                            where x.ChainId == chainId
                            select s;

                foreach (var entity in query)
                {
                    Domain.AMSServer model = new Domain.AMSServer()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description
                    };

                    amsServers.Add(model);
                }
            }

            // Find any parent chains
            List<int> parentChainIds = new List<int>();
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from s in entitiesContext.ChainChains
                            where s.ChildChainId == chainId
                            select s;

                foreach (var entity in query)
                {
                    parentChainIds.Add(entity.ParentChainId);
                }
            }

            // Get the AMS servers that the parent chain is allowed to access
            foreach (int parentChainId in parentChainIds)
            {
                this.GetByChainIdRecursive(parentChainId, amsServers);
            }
        }
    }
}
