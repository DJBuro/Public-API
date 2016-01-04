using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.WebOrdering
{
    public class Logger
    {
        private static ILog log = null;
        public static ILog Log 
        { 
            get
            {
                if (Logger.log == null)
                {
                    var x = log4net.Config.XmlConfigurator.Configure();

                    Logger.log = LogManager.GetLogger(typeof(Global));
                }

                return Logger.log;
            } 
        }
    }
}
