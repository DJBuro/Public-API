using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Core.Site;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Core;
using System.Threading.Tasks;

namespace MyAndromeda.Menus.Events
{
    public interface IMenuPublishEvents : ITransientDependency
    {
        Task Publishing(MenuPublishContext context);

        Task Published(MenuPublishContext context);
    }

    public class MenuPublishContext
    {
        public bool Cancel { get; set; }

        public int AndromedaSiteId { get; set; }
    }


}