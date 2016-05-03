using System.Collections.Generic;

namespace MyAndromeda.Web.Controllers.Api.User
{

    public class StoreModel 
    {
        public int Id { get; internal set; }
        public int AndromedaSiteId { get; set; }
        public string ExternalSiteId { get; set; }
        public string Name { get; set; }
        
        public bool HasRameses { get; set; }

        public List<StoreEnrollment> StoreEnrollments { get; set; }
        public int ChainId { get; internal set; }
        public string ChainName { get; internal set; }
        //public bool HasAcsReports { get; set; }
        //public bool HasAmsReports { get; set; }
    }

}