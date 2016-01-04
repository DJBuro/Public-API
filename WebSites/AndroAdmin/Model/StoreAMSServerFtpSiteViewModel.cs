using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.Model
{
    public class StoreAMSServerFtpSiteViewModel
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string AMSServerName { get; set; }
        public string FTPServers { get; set; }
        public DateTime? LastFTPUploadDateTime { get; set; }
    }
}