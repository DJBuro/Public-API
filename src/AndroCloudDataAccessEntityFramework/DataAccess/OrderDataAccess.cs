﻿using System;
using System.Data;
using System.Linq;
using AndroCloudDataAccess.DataAccess;
using System.Collections.Generic;
using AndroCloudDataAccessEntityFramework.Model;

namespace AndroCloudDataAccessEntityFramework.DataAccess
{
    //public class OrderDataAccess : IOrdersDataAccess
    //{
    //    public string ConnectionStringOverride { get; set; }

    //    public string GetById(Guid externalOrderId, out AndroCloudDataAccess.Domain.Order order)
    //    {
    //        order = null;

    //        using (ACSEntities acsEntities = new ACSEntities())
    //        {
    //            DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

    //            var acsQuery = from o in acsEntities.Orders
    //                           where o.ID == externalOrderId
    //                           select o;

    //            var acsQueryEntity = acsQuery.FirstOrDefault();

    //            if (acsQueryEntity != null)
    //            {
    //                order = new AndroCloudDataAccess.Domain.Order();
    //                order.ID = acsQueryEntity.ID;
    //                order.StoreOrderId = acsQueryEntity.ExternalID;
    //                order.InternetOrderNumber = acsQueryEntity.InternetOrderNumber.GetValueOrDefault(-1);
    //                order.RamesesStatusId = acsQueryEntity.OrderStatu.RamesesStatusId;
    //            }
    //        }

    //        return "";
    //    }

    //    public string GetByExternalIdApplicationId(string externalOrderId, string externalApplicationId, out AndroCloudDataAccess.Domain.Order order)
    //    {
    //        order = null;

    //        using (ACSEntities acsEntities = new ACSEntities())
    //        {
    //            DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

    //            var acsQuery = from o in acsEntities.Orders
    //                           join a in acsEntities.ACSApplications
    //                           on o.ApplicationID equals a.Id
    //                           where o.ExternalID == externalOrderId
    //                           && a.ExternalApplicationId == externalApplicationId
    //                           select o;

    //            var acsQueryEntity = acsQuery.FirstOrDefault();

    //            if (acsQueryEntity != null)
    //            {
    //                order = new AndroCloudDataAccess.Domain.Order();
    //                order.ID = acsQueryEntity.ID;
    //                order.StoreOrderId = acsQueryEntity.ExternalID;
    //                order.InternetOrderNumber = acsQueryEntity.InternetOrderNumber.GetValueOrDefault(-1);
    //                order.RamesesStatusId = acsQueryEntity.OrderStatu.RamesesStatusId;
    //            }
    //        }

    //        return "";
    //    }

    //    public string GetByInternetOrderNumberAndSiteId(int internetOrderNumber, Guid siteId, out AndroCloudDataAccess.Domain.Order order)
    //    //public string GetByInternetOrderNumber(int internetOrderNumber, out AndroCloudDataAccess.Domain.Order order)
    //    {
    //        order = null;

    //        using (ACSEntities acsEntities = new ACSEntities())
    //        {
    //            DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

    //            var acsQuery = from o in acsEntities.Orders
    //                           where o.InternetOrderNumber == internetOrderNumber
    //                           && o.SiteID == siteId
    //                           select o;

    //            var acsQueryEntity = acsQuery.FirstOrDefault();

    //            if (acsQueryEntity == null)
    //            {

    //            }
    //            else
    //            {
    //                order = new AndroCloudDataAccess.Domain.Order();
    //                order.ID = acsQueryEntity.ID;
    //                order.StoreOrderId = acsQueryEntity.ExternalID;
    //                order.InternetOrderNumber = acsQueryEntity.InternetOrderNumber.GetValueOrDefault(-1);
    //                order.RamesesStatusId = acsQueryEntity.OrderStatu.RamesesStatusId;
    //            }
    //        }

    //        return "";
    //    }

    //    public string GetByExternalOrderNumber(string externalOrderId, out AndroCloudDataAccess.Domain.Order order)
    //    {
    //        order = null;

    //        using (ACSEntities acsEntities = new ACSEntities())
    //        {
    //            DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

    //            var acsQuery = from o in acsEntities.Orders
    //                           where o.ExternalID == externalOrderId
    //                           select o;

    //            var acsQueryEntity = acsQuery.FirstOrDefault();

    //            if (acsQueryEntity != null)
    //            {
    //                order = new AndroCloudDataAccess.Domain.Order();
    //                order.ID = acsQueryEntity.ID;
    //                order.StoreOrderId = acsQueryEntity.ExternalID;
    //                order.InternetOrderNumber = acsQueryEntity.InternetOrderNumber.GetValueOrDefault(-1);
    //                order.RamesesStatusId = acsQueryEntity.OrderStatu.RamesesStatusId;
    //            }
    //        }

    //        return "";
    //    }

    //    public string Update(Guid orderId, Guid orderStatusId)
    //    {
    //        using (ACSEntities acsEntities = new ACSEntities())
    //        {
    //            DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

    //            var acsQuery = from o in acsEntities.Orders
    //                           where o.ID == orderId
    //                           select o;

    //            var acsQueryEntity = acsQuery.FirstOrDefault();

    //            // Update the order record
    //            if (acsQueryEntity != null)
    //            {
    //                acsQueryEntity.StatusId = orderStatusId;
    //                acsEntities.SaveChanges();
    //            }
    //        }

    //        return "";
    //    }

    //    public string GetByApplicationIdOrderId(int applicationId, Guid orderId, out AndroCloudDataAccess.Domain.Order order)
    //    {
    //        order = null;

    //        using (ACSEntities acsEntities = new ACSEntities())
    //        {
    //            DataAccessHelper.FixConnectionString(acsEntities, this.ConnectionStringOverride);

    //            var acsQuery = from o in acsEntities.Orders
    //                           join s in acsEntities.Sites
    //                           on o.SiteID equals s.ID
    //                           join sg in acsEntities.ACSApplicationSites
    //                           on s.ID equals sg.SiteId
    //                           join p in acsEntities.ACSApplications
    //                           on sg.ACSApplicationId equals p.Id
    //                           join sm in acsEntities.SiteMenus
    //                           on s.ID equals sm.SiteID
    //                           where p.Id == applicationId
    //                           && o.ID == orderId
    //                           select o;

    //            var acsQueryEntity = acsQuery.FirstOrDefault();

    //            if (acsQueryEntity != null)
    //            {
    //                order = new AndroCloudDataAccess.Domain.Order();
    //                order.ID = acsQueryEntity.ID;
    //                order.StoreOrderId = acsQueryEntity.ExternalID;
    //                order.InternetOrderNumber = acsQueryEntity.InternetOrderNumber.GetValueOrDefault(-1);
    //                order.RamesesStatusId = acsQueryEntity.OrderStatu.RamesesStatusId;
    //            }
    //        }

    //        return "";
    //    }
    //}
}
