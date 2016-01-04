using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering
{
    public class Email
    {
        public string HostHeader { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServerType { get; set; }
        public string Server { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set;}
        public string AttachmentFilename { get; set; }
        public string Attachment { get; set; }
    }
}