using GeoCode.Dao.Domain;

namespace GeoCode.Dao
{
    public interface IGeoCode
    {
        Coordinates GeoCodeAddress(string externalStoreId, string buildingNumber, string buildingName, string roadName, string townCity, string postCode, string country);
    }
}
