using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using System;
using System.Collections.Generic;
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
}
