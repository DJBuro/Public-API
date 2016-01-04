using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Services.Ibs;
using MyAndromeda.Services.Ibs.Models;
using MyAndromeda.Services.Ibs.IbsWebOrderApi;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class LoginService : ILoginService 
    {
        private readonly IIbsStoreDevice ibsStoreDevice;
        private readonly IbsGatewayService ibsGatewayService;


        public LoginService(IIbsStoreDevice ibsStoreDevice, IbsGatewayService ibsGatewayService)
        {
            this.ibsGatewayService = ibsGatewayService;
            this.ibsStoreDevice = ibsStoreDevice;
        }

        public async Task<Models.TokenResult> LoginAsync(int andromedaSiteId, IbsStoreSettings request) 
        {
            Models.TokenResult result = null;

            using (WebOrdersAPISoapClient client = await ibsGatewayService.CreateClient(andromedaSiteId)) 
            {
                var r = await client.LogonAsync(request.CompanyCode, request.UserName, request.Password, request.AppGuid);

                result = new TokenResult() 
                {
                    Token = r.m_szToken,
                    ExpiresUtc = r.m_dtTokenExpire
                };
            }

            return result;
        }

        public async Task<TokenResult> LoginAsync(int andromedaSiteId)
        {
            var settings = await this.ibsStoreDevice.GetIbsStoreDeviceAsync(andromedaSiteId);

            return await this.LoginAsync(andromedaSiteId, settings);
        }
    }
}
