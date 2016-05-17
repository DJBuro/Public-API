using System;
using System.Globalization;

namespace MyAndromeda.Web.Controllers.Api.Data
{

    public class CustomerViewModel 
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public string Longitude { get; set; }

        public string Postcode { get; set; }

        public string Directions { get; set; }

        public decimal[] GeoLocation 
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(Latitude)) { return new decimal[]{ 0,0 }; }
                var a = decimal.Parse(Latitude, CultureInfo.InvariantCulture); //Convert.ToDecimal(Latitude, );
                var b = decimal.Parse(Longitude, CultureInfo.InvariantCulture); //Convert.ToDecimal(Longitude);

                return new[] { a,b };
            } 
        }
    }

}