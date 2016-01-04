using System;
using System.Linq;
using MyAndromeda.Data.AcsOrders.Model;
using System.Linq.Expressions;
using MyAndromedaDataAccess.Domain.Reporting;
using System.Collections.Generic;

namespace MyAndromeda.Data.AcsOrders.Services
{
    //public class OrderDataService : IOrderReportingDataService 
    //{
    //    public OrderDataService ()
    //    {
    //        var dbContext = new Model.AcsOrderDbContext();

    //        this.OrderHeaderTable = dbContext.Set<OrderHeader>();
    //        this.OrderLineTable = dbContext.Set<OrderLine>();
    //    }
        
    //    internal IQueryable<OrderHeader> OrderHeaderTable { get; set; }
    //    internal IQueryable<OrderLine> OrderLineTable { get; set; }
        
    //    public IQueryable<OrderHeader> GetOrders(Expression<Func<OrderHeader, bool>> query)
    //    {
    //        var data = OrderHeaderTable.Where(query);

    //        return data;
    //    }

    //    public IEnumerable<SummaryByDay<decimal>> GetTotalOrdersByDay(Expression<Func<OrderHeader, bool>> query)
    //    {
    //        var data = OrderHeaderTable.Where(query);

    //        var groupedData = data.GroupBy(e =>
    //            new
    //            {
    //                e.TimeStamp.Day,
    //                e.TimeStamp.Month,
    //                e.TimeStamp.Year
    //            }).Select(e => new { 
    //                Day = e.Key.Day,
    //                Month = e.Key.Month,
    //                Year = e.Key.Year,
    //                OrderCount = e.Count(),
    //                Total = e.Sum(item => item.FinalPrice),
    //                MinPrice = e.Min(item => item.FinalPrice),
    //                MaxPrice = e.Max(item => item.FinalPrice),
    //                AvgPrice = e.Average(item => item.FinalPrice),
    //                CollectionCount = e.Count(item => item.OrderType == "COLLECTION"),
    //                DeliveryCount = e.Count(item => item.OrderType == "DELIVERY")
    //            }).ToArray();

    //        var results = groupedData.Select(e => new SummaryByDay<decimal>() { 
    //            Total = e.Total,
    //            Count = e.OrderCount,
    //            Average = e.AvgPrice,
    //            Max = e.MaxPrice,
    //            Min = e.MinPrice,
    //            Day = new DateTime(e.Year, e.Month, e.Day),
    //            DeliveryCount = e.DeliveryCount,
    //            CollectionCount = e.CollectionCount
    //        }).OrderByDescending(e=> e.Day).ToArray();

    //        return results;
    //    }


    //    public IEnumerable<SummaryOfLineItem> GetOrderLineSummary(Expression<Func<OrderLine, bool>> query)
    //    {
    //        var data = this.OrderLineTable.Where(query);

    //        var groupedData = data
    //            .Select(e=> new {
    //                e.ProductID,
    //                e.Description,
    //                e.Qty,
    //                e.Price,
    //            })
    //            .GroupBy(e => new { e.ProductID, e.Description, e.Price })
    //            .Select(e => 
    //                new { 
    //                    Name = e.Key.Description,
    //                    Price = e.Key.Price,
    //                    OrderCount = e.Count(),
    //                    Quantity = e.Sum(row => row.Qty),
    //                    SumPrice = e.Sum(row => row.Price * row.Qty)
    //                })
    //            .OrderByDescending(e=> e.OrderCount)
    //            .ToArray();

    //        var results = groupedData.Select(e=> new SummaryOfLineItem(){
    //            OrderCount = e.OrderCount,
    //            ItemPrice = e.Price,
    //            ItemsQuantitySold = e.Quantity,
    //            SumPrice = e.SumPrice.GetValueOrDefault(),
    //            Description = e.Name
    //        }).ToArray();

    //        return results;
    //    }

    //    public int GetOrderLineMax(Expression<Func<OrderLine, bool>> query)
    //    {
    //        var data = this.OrderLineTable.Where(query);

    //        var max = data.GroupBy(e => e.Description).Select(e => new
    //        {
    //            Name = e.Key,
    //            Count = e.Count()
    //        }).Max(e => (int?)e.Count);

    //        return max.GetValueOrDefault();
    //    }

    //    public LineItemLimits GetLimits(Expression<Func<OrderLine, bool>> query)
    //    {
    //        var dataQuery = this.OrderLineTable.Where(query);

    //        var limits = dataQuery.GroupBy(e => new { e.Description, e.Price })
    //            .Select(e => new { 
    //                Name = e.Key.Description,
    //                OrderCount = e.Count(),
    //                QuantitySold = e.Sum(item => item.Qty),
    //                SumOfPrice = e.Sum(item => item.Price)
    //            })
    //            .ToArray();

    //        if (limits.Length == 0)
    //        {
    //            return new LineItemLimits()
    //            {
    //                AmountOfOrders = new Limit() { Max = 0, Min = 0 },
    //                SumPriceOfAllItemsInAllOrders = new Limit() { Max = 0, Min = 0 },
    //                QuantitySoldInAllOrders = new Limit() { Max = 0, Min = 0 }
    //            };
    //        }

    //        return new LineItemLimits()
    //        {
    //            AmountOfOrders = new Limit() { Min = limits.Min(e => e.OrderCount), Max = limits.Max(e => e.OrderCount) },
    //            QuantitySoldInAllOrders = new Limit() { Min = limits.Min(e => e.QuantitySold.GetValueOrDefault()), Max = limits.Max(e => e.QuantitySold.GetValueOrDefault()) },
    //            SumPriceOfAllItemsInAllOrders = new Limit() { Min = limits.Min(e => e.SumOfPrice.GetValueOrDefault()), Max = limits.Max(e => e.SumOfPrice.GetValueOrDefault()) }
    //        };
    //    }


    //    public IQueryable<OrderHeader> Query(Expression<Func<OrderHeader, bool>> query)
    //    {
    //        if (query == null)
    //        {
    //            return OrderHeaderTable;
    //        }

    //        return OrderHeaderTable.Where(query);
    //    }
    //}
}


