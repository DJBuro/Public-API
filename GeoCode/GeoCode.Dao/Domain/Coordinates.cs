

namespace GeoCode.Dao.Domain
{
    public class Coordinates
    {        
        public long Id { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public Coordinates()
        {
        }

        public Coordinates(float longitude,float latitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }


    }
}