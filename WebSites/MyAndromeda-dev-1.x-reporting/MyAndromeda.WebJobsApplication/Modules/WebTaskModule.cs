using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Services.Marketing.WebJobs;
using Ninject.Modules;
using Microsoft.WindowsAzure.Storage.Queue;

namespace MyAndromeda.WebJobs.EventMarketing.Modules
{
    public class WebTaskModule : NinjectModule
    {
        public override void Load()
        {
            //this.Bind<IQueueManager>().To<QueueManager>();
            //this.Bind<IQueueNames>().To<QueueNames>().InSingletonScope();
        }

        
    }
}
