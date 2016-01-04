using System;
using System.Linq;

namespace MyAndromeda.Menus.Context.Ftp
{
    public class FtpMenuContext
    {
        public int AndromedaId { get; set; }

        public string ExpectedZipPath { get; set; }

        public string ExpectedTempMenuPath { get; set; }

        public string ExpectedMenuPath { get; set; }
        
        public bool? HasFoundFolder { get; set; }

        public bool? HasFoundFile { get; set; }

        public DateTime? FtpFileModifiedStamp { get; set; }

        public DateTime? LastUpdatedTimestamp { get; set; }

        public bool? HasDownloadedMenu { get; set; }

        public bool? HasUnzippedMenu { get; set; }

        public bool? HasUpdatedMenu { get; set; }
    }
}