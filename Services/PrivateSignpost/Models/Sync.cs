using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PrivateSignpost.Models
{
    public class Sync
    {
        public int fromVersion { get; set; }
        public int toVersion { get; set; }
        public List<ACSApplicationSync> acsApplicationSyncs { get; set; }
    }
}
