using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers
{
    public class TestHub : Hub
    {
        public TestHub() { }

        public DateTime Ping()
        {
            return DateTime.UtcNow;
        }
    }
}