using MyAndromeda.Services.Ibs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class IPaymentProviders : IGetPaymentTypes
    {
        private readonly IbsGatewayService ibsGatewayService;

        public IPaymentProviders(IbsGatewayService ibsGatewayService)
        {
            this.ibsGatewayService = ibsGatewayService;
        }

        public async Task<IEnumerable<PaymentTypeModel>> List(
            int andromedaSiteId,
            TokenResult token,
            LocationResult location) 
        {
            IEnumerable<PaymentTypeModel> result = Enumerable.Empty<PaymentTypeModel>();

            using (IbsWebOrderApi.WebOrdersAPISoapClient client = await this.ibsGatewayService.CreateClient(andromedaSiteId)) 
            {
                var response = await client.GetMediasAsync(token.Token, location.LocationCode, 0);

                if (response.m_objError.m_bError) 
                {
                    var error = response.m_objError;

                    throw new Exception(error.m_szError);
                }

                result = response.m_arrMedias.Select(e => new PaymentTypeModel { 
                    OtherCurrency = e.m_bOtherCurrency,
                    MediaType = e.m_eMediaType,
                    MediaNumber = e.m_lMediaNo, 
                    Description = e.m_szDescription,
                    Location= e.m_szLocation
                });
                
            }

            return result;
        }
    }
}
