using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Model
{
    public class Address
    {
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public string Prem1 { get; set; }
        public string Prem2 { get; set; }
        public string Prem3 { get; set; }
        public string Prem4 { get; set; }
        public string Prem5 { get; set; }
        public string Prem6 { get; set; }
        public string Org1 { get; set; }
        public string Org2 { get; set; }
        public string Org3 { get; set; }
        public string RoadNum { get; set; }
        public string RoadName { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string Dps { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Locality { get; set; }
        //public string Directions { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            string addressText = "";
            addressText += String.IsNullOrEmpty(this.RoadNum) ? "" : this.RoadNum + " ";
            addressText += String.IsNullOrEmpty(this.RoadName) ? "" : this.RoadName + ", ";
            addressText += String.IsNullOrEmpty(this.Org1) ? "" : this.Org1 + ", ";
            addressText += String.IsNullOrEmpty(this.Org2) ? "" : this.Org2 + ", ";
            addressText += String.IsNullOrEmpty(this.Org3) ? "" : this.Org3 + ", ";
            addressText += String.IsNullOrEmpty(this.Prem1) ? "" : this.Prem1 + ", ";
            addressText += String.IsNullOrEmpty(this.Prem2) ? "" : this.Prem2 + ", ";
            addressText += String.IsNullOrEmpty(this.Prem3) ? "" : this.Prem3 + ", ";
            addressText += String.IsNullOrEmpty(this.Prem4) ? "" : this.Prem4 + ", ";
            addressText += String.IsNullOrEmpty(this.Prem5) ? "" : this.Prem5 + ", ";
            addressText += String.IsNullOrEmpty(this.Prem6) ? "" : this.Prem6 + ", ";
            addressText += String.IsNullOrEmpty(this.Locality) ? "" : this.Locality + ", ";
            addressText += String.IsNullOrEmpty(this.Town) ? "" : this.Town + ", ";
            addressText += String.IsNullOrEmpty(this.County) ? "" : this.County + ", ";
            addressText += String.IsNullOrEmpty(this.State) ? "" : this.State + ", ";
            addressText += String.IsNullOrEmpty(this.Postcode) ? "" : this.Postcode + ", ";

            // Is there an orphaned comma on the end?
            if (addressText.EndsWith(", "))
            {
                addressText = addressText.Remove(addressText.Length - 2);
            }

            return addressText;
        }
    }
}
