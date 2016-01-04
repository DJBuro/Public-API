using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_PostCodes.Models
{
    public class PostcodeSector
    {
        public string Sector { get; set; }
        public string Postcode { get; set; }
        public double Eastings { get; set; }
        public double Northings { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double Distance { get; set; }
    }

    public class LatLong
    {
        public string Postcode { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}