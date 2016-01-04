using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace MyAndromeda.Framework.Messaging.Mail
{
    //public class EmailHost : IRegisteredObject
    //{
    //    private readonly object itemLock = new object();
    //    private bool shuttingDown;

    //    public EmailHost()
    //    {
    //        HostingEnvironment.RegisterObject(this);

    //        this.Created = DateTime.UtcNow;
    //    }

    //    public void Stop(bool immediate)
    //    {
    //        lock (itemLock)
    //        {
    //            shuttingDown = true;
    //        }

    //        HostingEnvironment.UnregisterObject(this);
    //    }

    //    public void DoWork(Action work)
    //    {
    //        lock (itemLock)
    //        {
    //            if (shuttingDown)
    //            {
    //                return;
    //            }
    //            work();
    //            Completed = true;
    //        }
    //    }

    //    public DateTime Created { get; set; }
    //    public bool Completed { get; set; }
    //}
}
