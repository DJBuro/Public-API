using System.Threading.Tasks;
using System.Linq;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class LocationService : ILocationService
    {
        private readonly IbsGatewayService ibsGatewayService;

        public LocationService(IbsGatewayService ibsGatewayService) 
        {
            this.ibsGatewayService = ibsGatewayService;
        }

        public async Task<Models.Locations> LoadLocationsAsync(int andromedaSiteId, Models.TokenResult tokenResult) 
        {
            var locations = new Models.Locations();

            using (IbsWebOrderApi.WebOrdersAPISoapClient client = await this.ibsGatewayService.CreateClient(andromedaSiteId)) 
            {
                var locationResult = await client.GetLocationsAsync(tokenResult.Token, "");

                foreach(var location in locationResult.m_arrLocations)
                {
                    locations.Add(new Models.LocationResult() { 
                        LocationCode = location.m_szLocation,
                        Description = location.m_szDescription,
                        SiteReference = location.m_szSiteReference,
                        Contact = location.m_szContact,
                        Address1 = location.m_szAddress1,
                        Address2 = location.m_szAddress2,
                        Address3 = location.m_szAddress3,
                        Address4 = location.m_szAddress4,
                        PostCode = location.m_szPostcode,
                        CountryCode = location.m_szCountryCode,
                        Latitude = location.m_dLatitude,
                        Longitude = location.m_dLongitude,
                        DeliveryOffline = location.m_bWebOrderDeliveryOffLine,
                        CollectionOffline = location.m_bWebOrderCollectionOffline,
                    });
                }
            }

            return locations;
        }
    }
}