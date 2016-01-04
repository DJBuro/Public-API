using MyAndromedaDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.ViewModels
{
    public class ChainListViewModel
    {
        public Chain[] FlatternedChains { get; set; }
        public Chain[] TreeViewChain { get; set; }
    }
}