using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsServices.Events;
using MyAndromeda.Data.AcsServices.Models;

namespace MyAndromeda.Menus.Handlers
{
    public class MenuLoadedHandler : IMenuLoadingEvent
    {
        public MenuLoadedHandler() { }

        public int Order
        {
            get { return 100; }
        }

        const string Name = "I cant remember what this was going to do";
        public string HandlerName
        {
            get
            {
                return Name;
            }
        }

        public void Loaded(IMenuLoadingContext context)
        {
            // TODO: Implement this method
        }

    }
}
