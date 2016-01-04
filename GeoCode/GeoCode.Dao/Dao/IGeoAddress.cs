
namespace GeoCode.Dao
{
    public interface IGeoAddress
    {
        string BuildingNumber { get; set; }
        string BuildingName { get; set; }
        string RoadName { get; set; }
        string TownCity { get; set; }
        string PostCode { get; set; }
        string Country { get; set; }
    }
}
