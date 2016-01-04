
namespace GeoCode.Dao.Domain
{
    public class Address : IGeoAddress
    {
        #region IGeoAddress Members

        public string BuildingNumber { get; set;}

        public string BuildingName { get; set; }

        public string RoadName { get; set; }

        public string TownCity { get; set; }

        public string PostCode { get; set; }

        public string Country { get; set; }
        #endregion
    }
}
