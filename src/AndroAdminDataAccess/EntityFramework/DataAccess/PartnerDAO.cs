﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroAdminDataAccess.Domain;
using AndroAdminDataAccess.DataAccess;
using System.Data.Common;
using System.Transactions;

namespace AndroAdminDataAccess.EntityFramework.DataAccess
{
    public class PartnerDAO : IPartnerDAO
    {
        public string ConnectionStringOverride { get; set; }

        public IList<Domain.Partner> GetAll()
        {
            List<Domain.Partner> models = new List<Domain.Partner>();

             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from p in entitiesContext.Partners
                            select p;

                foreach (var entity in query)
                {
                    Domain.Partner model = new Domain.Partner()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        DataVersion = entity.DataVersion,
                        ExternalId = entity.ExternalId
                    };

                    models.Add(model);
                }
            }

            return models;
        }

        public Domain.Partner GetById(int partnerId)
        {
            Domain.Partner partner = null;

             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from s in entitiesContext.Partners
                            where partnerId == s.Id
                            select s;

                var entity = query.FirstOrDefault();

                if (entity != null)
                {
                    partner = new Domain.Partner()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        ExternalId = entity.ExternalId,
                        DataVersion = entity.DataVersion
                    };
                }
            }

            return partner;
        }

        public Domain.Partner GetByName(string name)
        {
            Domain.Partner partner = null;

             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from s in entitiesContext.Partners
                            where name == s.Name
                            select s;

                var entity = query.FirstOrDefault();

                if (entity != null)
                {
                    partner = new Domain.Partner()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        ExternalId = entity.ExternalId,
                        DataVersion = entity.DataVersion
                    };
                }
            }

            return partner;
        }

        public Domain.Partner GetByExternalId(string externalId)
        {
            Domain.Partner partner = null;

             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from s in entitiesContext.Partners
                            where externalId == s.ExternalId
                            select s;

                var entity = query.FirstOrDefault();

                if (entity != null)
                {
                    partner = new Domain.Partner()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        ExternalId = entity.ExternalId,
                        DataVersion = entity.DataVersion
                    };
                }
            }

            return partner;
        }

        public void Add(Domain.Partner partner)
        {
            using (System.Transactions.TransactionScope transactionScope = new TransactionScope())
            {
                using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
                {
                    DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                    entitiesContext.Database.Connection.Open();

                    // Get the next data version (see comments inside the function)
                    int newVersion = DataVersionHelper.GetNextDataVersion(entitiesContext);

                    Partner entity = new Partner()
                    {
                        Name = partner.Name,
                        ExternalId = partner.ExternalId,
                        DataVersion = newVersion
                    };

                    entitiesContext.Partners.Add(entity);
                    entitiesContext.SaveChanges();

                    // Commit the transacton
                    transactionScope.Complete();
                }
            }
        }

        public void Update(Domain.Partner partner)
        {
            using (System.Transactions.TransactionScope transactionScope = new TransactionScope())
            {
                using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
                {
                    DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                    entitiesContext.Database.Connection.Open();

                    // Get the next data version (see comments inside the function)
                    int newVersion = DataVersionHelper.GetNextDataVersion(entitiesContext);

                    var query = from s in entitiesContext.Partners
                                where partner.Id == s.Id
                                select s;

                    var entity = query.FirstOrDefault();

                    if (entity != null)
                    {
                        entity.Name = partner.Name;
                        entity.ExternalId = partner.ExternalId;
                        entity.DataVersion = newVersion;

                        entitiesContext.SaveChanges();

                        // Commit the transacton
                        transactionScope.Complete();
                    }
                }
            }
        }

        public IList<Domain.Partner> GetAfterDataVersion(int dataVersion)
        {
            List<Domain.Partner> models = new List<Domain.Partner>();

             
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);

                var query = from p in entitiesContext.Partners
                            where p.DataVersion > dataVersion
                            select p;

                foreach (var entity in query)
                {
                    Domain.Partner model = new Domain.Partner()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        DataVersion = entity.DataVersion,
                        ExternalId = entity.ExternalId
                    };

                    models.Add(model);
                }
            }

            return models;
        }
    }
}