using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Users.ViewModels
{
    public class UserStoreViewModel
    {
        public int StoreId { get; set; }
        public int AndromedaSiteId { get; set; }
        public string Name { get; set; }

        public int UserId { get; set; }

        public string Id
        {
            get
            {
                return string.Format("{0}|{1}", this.StoreId, this.UserId);
            }
        }
    }
}