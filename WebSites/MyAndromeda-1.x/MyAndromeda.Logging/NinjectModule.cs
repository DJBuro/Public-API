using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Logging
{
    public class FrameworkModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            //log4net.Config.XmlConfigurator.Configure();
            Bind<IMyAndromedaLogger>().ToMethod(MyAndromeda.Logging.LoggerFactory.GetLogger);
        }
    }
}
