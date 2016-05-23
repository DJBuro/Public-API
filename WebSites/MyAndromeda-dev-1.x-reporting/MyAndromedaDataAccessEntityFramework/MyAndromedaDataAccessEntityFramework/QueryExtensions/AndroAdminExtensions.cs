﻿using System;
using System.Data.Entity;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.QueryExtensions
{
    public static class AndroAdminExtensions
    {
        public static IQueryable<ACSApplication> GetAcsApplications(this DbSet<Store> Store, int storeId) 
        {
            return Store.Where(e => e.Id == storeId).SelectMany(e => e.ACSApplicationSites.Select(appplicationSite => appplicationSite.ACSApplication));
        }
    }
    //public static class DataWarehouseExtensions
    //{
    //    /// <summary>
    //    /// Filters the customer for contact details that allow marketing.
    //    /// </summary>
    //    /// <param name="customerQuery">The customer query.</param>
    //    /// <returns></returns>
    //    public static IQueryable<Model.CustomerDataWarehouse.CustomerRecord> FilterForMarketingByEmailType(this IQueryable<CustomerRecord> customerQuery) 
    //    {
    //        return customerQuery.Where(e => e.Contacts.Any(
    //            contact =>
    //                      contact.ContactTypeId == MyAndromedaDataAccess.Domain.DataWarehouse.ContactType.Email &&
    //                      contact.MarketingLevelId == MyAndromedaDataAccess.Domain.DataWarehouse.MarketingLevelType.ThirdParty));
    //    }
    //}
}