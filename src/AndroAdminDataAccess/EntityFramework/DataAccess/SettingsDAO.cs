﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroAdminDataAccess.Domain;
using AndroAdminDataAccess.DataAccess;

namespace AndroAdminDataAccess.EntityFramework.DataAccess
{
    public class SettingsDAO : ISettingsDAO
    {
        public string ConnectionStringOverride { get; set; }

        public string GetByName(string name, out string value)
        {
            value = null;

            //using (AndroAdminEntities androAdminEntities = ConnectionStringOverride == null ? new AndroAdminEntities() : new AndroAdminEntities(this.ConnectionStringOverride))
            using (AndroAdminEntities entitiesContext = new AndroAdminEntities())
            {
                if(!string.IsNullOrWhiteSpace(this.ConnectionStringOverride))
                {
                    DataAccessHelper.FixConnectionString(entitiesContext, this.ConnectionStringOverride);
                }

                var query = from p in entitiesContext.Settings
                            where p.Name == name
                            select p;

                var entity = query.FirstOrDefault();

                if (entity != null)
                {
                    value = entity.Value;
                }
            }

            return "";
        }
    }
}