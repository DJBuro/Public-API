﻿using MyAndromeda.Logging.NlogImplementation;
using Ninject.Extensions.Logging;
using Ninject;
using Ninject.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Logging
{
    public class LoggingModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            //log4net.Config.XmlConfigurator.Configure();
            //Bind<IMyAndromedaLogger>().ToMethod(MyAndromeda.Logging.LoggerFactory.GetLogger);

            this.Bind<ILoggerFactory>().ToConstant(new NLogLoggerFactory());

            this.Bind<IMyAndromedaLogger>().ToMethod((context) => {

                IMyAndromedaLogger logger = null;
                Type type = context.Request.Target == null 
                    ? typeof(IMyAndromedaLogger) 
                    : context.Request.Target.Member.DeclaringType;

                try
                {
                    logger = new NlogImplementation.MyAndromedaNLogger(type);
                    //logger = new NlogImplementation.NLogLogger(type);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine("logger cannot be created");
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }

                return logger;
            });
        }
    }
}
