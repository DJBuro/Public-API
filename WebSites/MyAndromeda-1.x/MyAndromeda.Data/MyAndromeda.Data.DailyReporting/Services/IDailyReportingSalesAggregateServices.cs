using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using System.Linq.Expressions;
using MyAndromeda.Data.DailyReporting.Model.CodeFirst;

namespace MyAndromeda.Data.DailyReporting.Services
{

    public interface IDailyReportingSalesAggregateServices : IDependency 
    {
        IEnumerable<T> QueryByDay<T>(long[] androAdminIds, Expression<Func<Model.CodeFirst.DailySummary, bool>> query, Func<Model.CodeFirst.DailySummary, T> transform);
        IEnumerable<T> QueryByHour<T>(long[] androAdminIds, Expression<Func<Model.CodeFirst.HourlyServiceMetrics, bool>> query, Func<Model.CodeFirst.HourlyServiceMetrics, T> transform);

        IEnumerable<DailyMetricGroup> SalesByDay(long[] androAdminIds, Expression<Func<Model.CodeFirst.DailySummary, bool>> query);
        IEnumerable<DailyMetricGroup> SalesByHour(long[] andromediaIds, Expression<Func<Model.CodeFirst.HourlyServiceMetrics, bool>> query);
    }

    public class SalesAggregateServices : IDailyReportingSalesAggregateServices 
    {
        public IEnumerable<T> QueryByDay<T>(Int64[] androAdminIds, Expression<Func<DailySummary, bool>> query, Func<DailySummary, T> transform)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();
            using (var dbContext = new Model.CodeFirst.DailyReportingCodeFirstDbContext()) 
            {
                long? firstId = androAdminIds.First();
                var table = dbContext.DailySummary;
                
                var tableQuery = androAdminIds.Length > 1 ?
                    table.Where(e => androAdminIds.Contains(e.NStoreId ?? 0)) :
                    table.Where(e => firstId == e.NStoreId);

                var queryResult = tableQuery.Where(query).ToArray();

                results = queryResult.Select(e => transform(e));
            }

            return results;
        }

        public IEnumerable<T> QueryByHour<T>(long[] andromediaIds, Expression<Func<HourlyServiceMetrics, bool>> query, Func<HourlyServiceMetrics, T> transform)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var dbContext = new Model.CodeFirst.DailyReportingCodeFirstDbContext()) 
            {
                long? firstId = andromediaIds.First();
                var table = dbContext.HourlyServiceMetrics;

                var tableQuery = andromediaIds.Length > 1 ?
                    table.Where(e => andromediaIds.Contains(e.NStoreId)) :
                    table.Where(e => firstId == e.NStoreId);

                var queryResult = tableQuery.Where(query).ToArray();

                results = queryResult.Select(e => transform(e));
            }

            return results;
        }

        public IEnumerable<DailyMetricGroup> SalesByDay(long[] androAdminIds, Expression<Func<DailySummary, bool>> query)
        {
            IEnumerable<DailyMetricGroup> results;
            using (var dbContext = new Model.CodeFirst.DailyReportingCodeFirstDbContext()) 
            {
                long? firstId = androAdminIds.First();
                var table = dbContext.DailySummary;
                
                var tableQuery = androAdminIds.Length > 1 ?
                    table.Where(e => androAdminIds.Contains(e.NStoreId ?? 0)) :
                    table.Where(e => firstId == e.NStoreId);

                var resultQuery = tableQuery.Where(query).Select(e => new
                {
                    e.NStoreId,
                    e.TheDate,
                    
                    Orders = new {
                        e.TotalOrders,
                        e.TotalCancels,
                        e.NetSales,
                    },
                    DineIn = new {
                        e.DineInTotalOrders,
                        e.DineInNetSales,
                    },
                    CarryOut = new {
                        e.CarryOutTotalOrders,
                        e.CarryOutNetSales
                    },
                    Online = new {
                        e.DelTotalOrders,
                        e.DelNetSales,
                    },
                    Performance = new {
                        e.NumOrdersLt30Mins,
                        e.NumOrdersLt45Mins,
                        e.AvgDoorTime,
                        e.AvgMake,
                        e.AvgOutTheDoor,
                        e.RackTime
                    }
                });

                var result = resultQuery.ToArray();

                results = result.Select(e => new DailyMetricGroup()
                {
                    AndromediaSiteId = e.NStoreId,
                    Date = e.TheDate,
                    InStore = new OrderMetricGroup()
                    {
                        OrderCount = e.DineIn.DineInTotalOrders.GetValueOrDefault(),
                        Sales = e.DineIn.DineInNetSales.GetValueOrDefault()
                    },
                    Collection = new OrderMetricGroup()
                    {
                        OrderCount = e.CarryOut.CarryOutTotalOrders.GetValueOrDefault(),
                        Sales = e.CarryOut.CarryOutNetSales.GetValueOrDefault()
                    },
                    Delivery = new OrderMetricGroup()
                    {
                        OrderCount = e.Online.DelTotalOrders.GetValueOrDefault(),
                        Sales = e.Online.DelNetSales.GetValueOrDefault()
                    },
                    Combined = new OrderMetricGroup()
                    {
                        OrderCount = e.Orders.TotalOrders.GetValueOrDefault(),
                        Sales = e.Orders.NetSales.GetValueOrDefault(),
                        Cancelled = e.Orders.TotalCancels
                    },
                    Performance = new PerformanceMetricGroup() { 
                        AvgDoorTime = e.Performance.AvgDoorTime.GetValueOrDefault() / 60,
                        AvgMakeTime = e.Performance.AvgMake.GetValueOrDefault() / 60,
                        AvgOutTheDoor = e.Performance.AvgOutTheDoor.GetValueOrDefault() / 60,
                        NumOrdersLT30Mins = e.Performance.NumOrdersLt30Mins,
                        NumOrdersLT45Mins = e.Performance.NumOrdersLt45Mins
                    }
                }).ToArray();
            }

            return results;
        }

        public IEnumerable<DailyMetricGroup> SalesByHour(long[] andromediaIds, Expression<Func<HourlyServiceMetrics, bool>> query)
        {
            IEnumerable<DailyMetricGroup> results = Enumerable.Empty<DailyMetricGroup>();
            using (var dbContext = new Model.CodeFirst.DailyReportingCodeFirstDbContext())
            {
                long? firstId = andromediaIds.First();
                var table = dbContext.HourlyServiceMetrics;

                var tableQuery = andromediaIds.Length > 1 ?
                    table.Where(e => andromediaIds.Contains(e.NStoreId)) :
                    table.Where(e => firstId == e.NStoreId);

                var resultQuery = tableQuery.Where(query).Select(e => new
                {
                    e.NStoreId,
                    e.Thedate,
                    e.Thehour,
                    Orders = new
                    {
                        e.TotalOrders,
                        //e.ca,
                        e.OrderNet
                    },
                    DineIn = new
                    {
                        e.DineInCount,
                        //e.,
                    },
                    CarryOut = new
                    {
                        e.CollectionCount,
                    },
                    Online = new
                    {
                        e.DeliveryCount
                    },
                    Performance = new
                    {
                        //e.NumOrdersLT30Mins,
                        //e.NumOrdersLT45Mins,
                        //e.AvgDoorTime,
                        //e.AvgMake,
                        e.NAvgDoor,
                        e.NAvgRackTime,
                        e.Over15LessThan20,
                        e.Over20LessThan25,
                        e.Over25LessThan30,
                        e.Over30LessThan35,
                        e.Over35LessThan45,
                        e.Over45LessThan60,
                        e.Over60
                    }
                });

                var result = resultQuery.ToArray();
                results = result.Select(e => new DailyMetricGroup()
                {
                    AndromediaSiteId = e.NStoreId,
                    Date = e.Thedate.AddHours(e.Thehour),
                    InStore = new OrderMetricGroup()
                    {
                        OrderCount = e.DineIn.DineInCount.GetValueOrDefault(),
                        //Sales = e.DineIn.DineInNetSales
                    },
                    Collection = new OrderMetricGroup()
                    {
                        OrderCount = e.CarryOut.CollectionCount.GetValueOrDefault(),
                        //Sales = e.CarryOut.CarryOutNetSales
                    },
                    Delivery = new OrderMetricGroup()
                    {
                        OrderCount = e.Online.DeliveryCount.GetValueOrDefault(),
                       // Sales = e.Online.DelNetSales
                    },
                    Combined = new OrderMetricGroup()
                    {
                        OrderCount = e.Orders.TotalOrders.GetValueOrDefault(),
                        Sales = Convert.ToInt64(Math.Round(e.Orders.OrderNet.GetValueOrDefault() * 100, 0)),
                        //Canceled = e.Orders.TotalCancels
                    },
                    Performance = new PerformanceMetricGroup()
                    {
                        AvgDoorTime = e.Performance.NAvgDoor, //e.Performance.AvgDoorTime.GetValueOrDefault() / 60,
                        AvgOutTheDoor = e.Performance.NAvgDoor,
                        NumOrdersLT30Mins = e.Performance.Over15LessThan20 + e.Performance.Over20LessThan25 + e.Performance.Over25LessThan30,
                        NumOrdersLT45Mins = e.Performance.Over30LessThan35 + e.Performance.Over35LessThan45,
                        //AvgMakeTime = e.Performance.AvgMake.GetValueOrDefault() / 60,
                        //AvgOutTheDoor = e.Performance.AvgOutTheDoor.GetValueOrDefault() / 60,
                        //NumOrdersLT30Mins = e.Performance.NumOrdersLT30Mins,
                        //NumOrdersLT45Mins = e.Performance.NumOrdersLT45Mins
                    }
                }).ToArray();
            }

            return results;
        }
    }

    public class OrderMetricGroup 
    {
        public Int64 Sales { get; set; }
        public double AvgSale {
            get
            {
                if (this.Sales == 0 || this.OrderCount == 0) { return 0; }
                return Sales / OrderCount;
            }
        }
        public long OrderCount { get; set; }
        public long? Cancelled { get; set; }
    }

    public class DailyMetricGroup
    {
        public DailyMetricGroup() 
        {
        }

        public long? AndromediaSiteId { get; set; }
        public DateTime? Date { get; set; }

        public OrderMetricGroup InStore { get;set; }
        public OrderMetricGroup Collection {get;set;}
        public OrderMetricGroup Delivery {get;set;}
        
        /// <summary>
        /// Aggregates of the three above groups
        /// </summary>
        public OrderMetricGroup Combined {get;set;}

        public PerformanceMetricGroup Performance { get; set; }
    }

    public class PerformanceMetricGroup 
    {
        public double? AvgMakeTime { get; set; }
        public double? AvgRackTime { get; set; }
        public double? AvgDoorTime { get; set; }
        public double? AvgOutTheDoor { get; set; }
        public long? NumOrdersLT30Mins { get; set; }
        public long? NumOrdersLT45Mins { get; set; }
        public long? Over15lessThan20 { get; set; }
    }
}