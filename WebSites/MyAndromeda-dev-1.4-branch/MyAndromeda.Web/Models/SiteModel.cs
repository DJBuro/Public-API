using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromedaDataAccess.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyAndromeda.Web.Models
{
    public class SiteModel
    {
        public bool Editable { get; set; }

        public SiteDetails SiteDetails { get; set; }
        //public List<Employee> Employees { get; set; }
        public Dictionary<string, List<TimeSpanBlock>> OpeningTimesByDay { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDay { get; set; }
        public bool ChangeIsOpen { get; set; }
        public Dictionary<string, bool> IsOpen { get; set; }

        public string AddOpeningStartTime { get; set; }
        public string AddOpeningEndTime { get; set; }

        [Required(ErrorMessage="Please enter the employees first name")]
        [StringLength(64, ErrorMessage = "Employees first name cannot contain more than 64 characters")]
        public string AddEmployeeFirstname { get; set; }

        [Required(ErrorMessage="Please enter the employees surname")]
        [StringLength(64, ErrorMessage = "Employees surname cannot contain more than 64 characters")]
        public string AddEmployeeSurname { get; set; }

        [Required(ErrorMessage="Please enter the employees role")]
        [StringLength(32, ErrorMessage = "Employees role cannot contain more than 32 characters")]
        public string AddEmployeeRole { get; set; }

        [Required(ErrorMessage = "Please enter the employee's phone number")]
        [StringLength(32, ErrorMessage = "Employees phone number cannot contain more than 32 characters")]
        public string AddEmployeePhone { get; set; }

        public string ExternalSiteId { get; set; }
       
        public int CountryId { get; set; }

        public List<Country> Countries { get; set; }
    }
}