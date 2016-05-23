using System.Threading.Tasks;
using MyAndromeda.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBackgrounder;

namespace MyAndromeda.Web.Services
{
    public class KeepAliveJob : Job
    {
        public KeepAliveJob(TimeSpan fromMinutes, TimeSpan fromSeconds) : base("KeepAlive", fromMinutes, fromSeconds)
        {
        }

        public override Task Execute()
        {
            return new Task(this.DoWork);
        }

        public void DoWork() 
        {
            KeepAlive.Poke();
        }
    }
}