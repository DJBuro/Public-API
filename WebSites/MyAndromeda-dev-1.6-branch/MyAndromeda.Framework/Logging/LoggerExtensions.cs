using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Framework.Logging
{
    public static class LoggerExtensions
    {
        //public static void DebugItems<V>(this ILogger logger, IEnumerable<V> items, Func<V, string> bitToLog) 
        //{
        //    foreach (var item in items) 
        //    {
        //        logger.Debug(bitToLog(item));
        //    }
        //}

        public static void DebugItems<TModel>(this IMyAndromedaLogger logger, IEnumerable<TModel> items, Func<TModel, string> bitToLog)
        {
            foreach (var item in items)
            {
                logger.Debug(bitToLog(item));
            }
        }

        public static void LogWorkContext(this IMyAndromedaLogger logger, IWorkContext workContext)
        {
            logger.Debug(string.Format("Chain: {0}; AndromedaSiteId: {1}; UserName:{2}",
                workContext.CurrentChain.Available ? workContext.CurrentChain.Chain.Id.ToString() : "n/a",
                workContext.CurrentSite.Available ? workContext.CurrentSite.AndromediaSiteId.ToString() : "n/a",
                workContext.CurrentUser.Available ? workContext.CurrentUser.User.Username : "n/a"));
        }
    }


    public static class IQueryableExtensions
    {
        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceQuery<T>(this IQueryable<T> query)
        {
            ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

            var result = objectQuery.ToTraceString();
            foreach (var parameter in objectQuery.Parameters)
            {
                var name = "@" + parameter.Name;
                var value = "'" + parameter.Value.ToString() + "'";
                result = result.Replace(name, value);
            }

            return result;
        }

        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL and Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceString<T>(this IQueryable<T> query)
        {
            ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

            var traceString = new StringBuilder();

            traceString.AppendLine(objectQuery.ToTraceString());
            traceString.AppendLine();

            foreach (var parameter in objectQuery.Parameters)
            {
                traceString.AppendLine(parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
            }

            return traceString.ToString();
        }

        private static System.Data.Entity.Core.Objects.ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
        {
            var internalQueryField = query.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
            var internalQuery = internalQueryField.GetValue(query);
            var objectQueryField = internalQuery.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
            return objectQueryField.GetValue(internalQuery) as System.Data.Entity.Core.Objects.ObjectQuery<T>;
        }
    }
}
