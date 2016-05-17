using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using System.Linq.Expressions;
using MyAndromeda.Data.DailyReporting.Model.CodeFirst;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DailyReporting.Services
{
    public interface IDailyReportingSalesAggregateServices : IDependency 
    {
        /// <summary>
        /// Queries the by day async.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="androAdminIds">The andro admin ids.</param>
        /// <param name="query">The query.</param>
        /// <param name="transform">The transform.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryByDayAsync<T>(long[] androAdminIds, Expression<Func<Model.CodeFirst.DailySummary, bool>> query, Func<Model.CodeFirst.DailySummary, T> transform);

        /// <summary>
        /// Queries the by hour async.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="androAdminIds">The andro admin ids.</param>
        /// <param name="query">The query.</param>
        /// <param name="transform">The transform.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryByHourAsync<T>(long[] androAdminIds, Expression<Func<Model.CodeFirst.HourlyServiceMetric, bool>> query, Func<Model.CodeFirst.HourlyServiceMetric, T> transform);

        /// <summary>
        /// Saleses the by day async.
        /// </summary>
        /// <param name="androAdminIds">The andro admin ids.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        Task<IEnumerable<DailyMetricGroup>> SalesByDayAsync(long[] androAdminIds, Expression<Func<Model.CodeFirst.DailySummary, bool>> query);

        /// <summary>
        /// Saleses the by hour async.
        /// </summary>
        /// <param name="andromediaIds">The andromedia ids.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        Task<IEnumerable<DailyMetricGroup>> SalesByHourAsync(long[] andromediaIds, Expression<Func<Model.CodeFirst.HourlyServiceMetric, bool>> query);
    }

    public class SalesAggregateServices : IDailyReportingSalesAggregateServices 
    {
        public async Task<IEnumerable<T>> QueryByDayAsync<T>(Int64[] androAdminIds, Expression<Func<DailySummary, bool>> query, Func<DailySummary, T> transform)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();
            using (var dbContext = new Model.CodeFirst.DailyReportingCodeFirstDbContext()) 
            {
                long? firstId = androAdminIds.First();
                var table = dbContext.DailySummaries;
                
                var tableQuery = androAdminIds.Length > 1 ?
                    table.Where(e => androAdminIds.Contains(e.NStoreId ?? 0)) :
                    table.Where(e => firstId == e.NStoreId);

                var queryResult = await tableQuery.Where(query).ToArrayAsync();

                results = queryResult.Select(e => transform(e)).ToArray();
            }

            return results;
        }

        public async Task<IEnumerable<T>> QueryByHourAsync<T>(long[] andromediaIds, Expression<Func<HourlyServiceMetric, bool>> query, Func<HourlyServiceMetric, T> transform)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();

            using (var dbContext = new Model.CodeFirst.DailyReportingCodeFirstDbContext()) 
            {
                long? firstId = andromediaIds.First();
                var table = dbContext.HourlyServiceMetrics;

                var tableQuery = andromediaIds.Length > 1 ?
                    table.Where(e => andromediaIds.Contains(e.NStoreId)) :
                    table.Where(e => firstId == e.NStoreId);

                var queryResult = await tableQuery.Where(query).ToArrayAsync();

                results = queryResult.Select(e => transform(e)).ToArray();
            }

            return results;
        }

        public async Task<IEnumerable<DailyMetricGroup>> SalesByDayAsync(long[] androAdminIds, Expression<Func<DailySummary, bool>> query)
        {
            IEnumerable<DailyMetricGroup> results;
            using (var dbContext = new Model.CodeFirst.DailyReportingCodeFirstDbContext()) 
            {
                long? firstId = androAdminIds.First();
                DbSet<DailySummary> table = dbContext.DailySummaries;

                IQueryable<DailySummary> tableQuery = table;
                if (androAdminIds.Length > 1)
                {
                    tableQuery = table.Where(e => androAdminIds.Contains(e.NStoreId ?? 0));
                }
                else
                {
                    tableQuery = table.Where(e => firstId == e.NStoreId);
                }
                
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

                var result = await resultQuery.ToArrayAsync();

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

        public async Task<IEnumerable<DailyMetricGroup>> SalesByHourAsync(long[] andromediaIds, Expression<Func<HourlyServiceMetric, bool>> query)
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

                var result = await resultQuery.ToArrayAsync();
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
                        AvgDoorTime = Convert.ToDecimal(e.Performance.NAvgDoor.GetValueOrDefault()), //e.Performance.AvgDoorTime.GetValueOrDefault() / 60,
                        AvgOutTheDoor = Convert.ToDecimal(e.Performance.NAvgDoor.GetValueOrDefault()),
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
}