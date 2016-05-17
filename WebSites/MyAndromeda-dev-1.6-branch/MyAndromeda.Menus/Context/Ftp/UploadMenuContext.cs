using System;

namespace MyAndromeda.Menus.Context.Ftp
{
    public class UploadMenuContext
    {
        public string RemotePath { get; set; }

        public bool? HasZipped { get; set; }

        public bool? HasFolder { get; set; }

        public bool? UploadedFile { get; set; }

        public DateTime? FtpFileModifiedStamp { get; set; }
    }
}