using MyAndromeda.Framework.Contexts;
using MyAndromedaDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Users.ViewModels
{
    public class CreateEditUserViewModel 
    {
        public int Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }

        public bool? NotifyUser { get; set; }

        public List<int> SelectedChainIds { get; set; }
        public List<int> SelectedStoreIds { get; set; }
    }
}