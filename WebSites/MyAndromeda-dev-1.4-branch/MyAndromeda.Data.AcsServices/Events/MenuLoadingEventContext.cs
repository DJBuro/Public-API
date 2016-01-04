using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Core;

namespace MyAndromeda.Data.AcsServices.Events
{
    public interface IMenuLoadingContext : IEventContext 
    {
        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>The menu.</value>
        MyAndromedaMenu Menu { get; set; }

        int AndromedaSiteId { get; set; }

        bool LoadedAcsMenu { get; set; }

        /// <summary>
        /// Gets or sets the added thumbnails.
        /// </summary>
        /// <value>The added thumbnails.</value>
        int AddedThumbnails { get; set; }
        bool LoadedThumbnails { get; set; }

        /// <summary>
        /// Gets or sets the added access data.
        /// </summary>
        /// <value>The added access data.</value>
        int AddedAccessData { get; set; }
        bool LoadedAccessData { get; set; }
    }

    public class MenuLoadingEventContext : IMenuLoadingContext
    {
        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>The menu.</value>
        public MyAndromedaMenu Menu { get; set; }

        public int AndromedaSiteId { get; set; }

        public bool LoadedAcsMenu { get;set; }

        public int AddedThumbnails { get;set; }

        public bool LoadedThumbnails
        {
            get;
            set;
        }

        public int AddedAccessData
        {
            get;
            set;
        }

        public bool LoadedAccessData
        {
            get;
            set;
        }
    }
}
