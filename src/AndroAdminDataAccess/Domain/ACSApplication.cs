﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AndroAdminDataAccess.Domain
{
    public class ACSApplication
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Please enter an Application id")]
        [Display(Name = "External application id")]
        public virtual string ExternalApplicationId { get; set; }

        [Required(ErrorMessage = "Please enter a name for this application")]
        [Display(Name = "Name")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Please enter a customer name for this application")]
        [Display(Name = "Customer application name")]
        public virtual string ExternalApplicationName { get; set; }

        public virtual int PartnerId { get; set; }

        public virtual int DataVersion { get; set; }

        public virtual Guid EnvironmentId { get; set; }

        [Display(Name = "Environment")]
        public virtual string EnvironmentName { get; set; }
    }

    public static class ACSApplicationExtensions
    {
        public static ACSApplication ToDomainObject(this AndroAdminDataAccess.EntityFramework.ACSApplication model)
        {
            ACSApplication vm = new ACSApplication();
            vm.Id = model.Id;
            vm.Name = model.Name;
            vm.ExternalApplicationId = model.ExternalApplicationId;
            vm.PartnerId = model.PartnerId;
            vm.DataVersion = model.DataVersion;
            vm.EnvironmentId = model.Environment.Id;
            vm.EnvironmentName = model.Environment.Name;
            return vm;
        }
    }
}
