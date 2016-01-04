using System;

namespace MyAndromeda.Core.Site
{
    public interface ISite 
    {
        int Id { get; set; }
        int AndromediaSiteId { get; set; }
    }

    public class SiteIds : ISite 
    {
        public int Id
        {
            get;
            set;
        }

        public int AndromediaSiteId
        {
            get;
            set;
        }
    }
}