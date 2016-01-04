using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromedaDataAccess.Domain.Reporting;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public class SalesDataService : ISalesDataService
    {
        private readonly DataWarehouseDbContext dbContext;

        public SalesDataService(DataWarehouseDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IQueryable<OrderHeader> Query
        {
            get
            {
                return this.dbContext.Set<OrderHeader>();
            }
        }

        /// <summary>
        /// Sales by the day.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public IEnumerable<SummaryByDay<decimal>> SalesByDay(Expression<Func<OrderHeader, bool>> filter)
        {
            var data = this.Query.Where(filter);
            var groupedData = data.GroupBy(e => new
            {
                e.TimeStamp.Day,
                e.TimeStamp.Month,
                e.TimeStamp.Year
            }).Select(e => new
            {
                e.Key.Day,
                e.Key.Month,
                e.Key.Year,
                Items = e,
                DeliveryQuantity = e.Count(item => item.OrderType == "DELIVERY"),
                collectionQuantity = e.Count(item => item.OrderType == "COLLECTION")
            }).ToArray();

            var results = groupedData.Select(e => new SummaryByDay<decimal>()
            {
                Total = e.Items.Sum(item => item.FinalPrice),
                Average = e.Items.Average(item => item.FinalPrice),
                Max = e.Items.Max(item => item.FinalPrice),
                Min = e.Items.Min(item => item.FinalPrice),
                CollectionCount = e.collectionQuantity,
                DeliveryCount = e.DeliveryQuantity
            }).ToArray();

            return results;
        }
    }
}