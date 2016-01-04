using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class CmsPages : List<CmsPage> 
    {
    
    }
    
    public class CmsPage
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Enabled { get; set; }
    }
}