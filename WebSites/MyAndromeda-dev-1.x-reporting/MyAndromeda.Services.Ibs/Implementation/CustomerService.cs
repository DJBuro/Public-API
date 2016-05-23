using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Services.Ibs.Models;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IbsGatewayService ibsGatewayService;

        public CustomerService(IbsGatewayService ibsGatewayService)
        {
            this.ibsGatewayService = ibsGatewayService;
        }

        public async Task<CustomerResultModel> Get(int andromedaSiteId, Models.TokenResult tokenResult, Models.CustomerRequestModel request)
        {
            CustomerResultModel result = null;

            using (IbsWebOrderApi.WebOrdersAPISoapClient client = await this.ibsGatewayService.CreateClient(andromedaSiteId))
            {
                var customer = await client.GetCustomerAsync(tokenResult.Token, request.Email);

                if (customer.m_objError.m_bError) 
                {
                    return null;
                }

                result = new CustomerResultModel();
                result.CustomerNumber = customer.m_objCustomerRec.m_lCustomerID;
                result.FirstName = customer.m_objCustomerRec.m_szFirstName;


                if (customer.m_arrDeliveryAddresses != null)
                {
                    result.DeliveryAddresses = customer.m_arrDeliveryAddresses
                        .Select(e => new CustomerDeliveryAddressResultModel()
                        {
                            Adddress1 = e.m_szAdddress1,
                            Adddress2 = e.m_szAdddress2,
                            Adddress3 = e.m_szAdddress3,
                            Adddress4 = e.m_szAdddress4,
                            BuildingName = e.m_szBuildingName,
                            CompanyName = e.m_szCompanyName,
                            DeliveryInstructions = e.m_szDeliveryInstructions,
                            Email = e.m_szEmail,
                            ForeName = e.m_szForeName,
                            Latitude = e.m_dLatitude,
                            Location = e.m_szLocation,
                            Longitude = e.m_dLongitude,
                            Mobile = e.m_szMobile,
                            Phone = e.m_szPhone,
                            PostCode = e.m_szPostCode,
                            Surname = e.m_szSurname
                        })
                        .ToList();
                }

            }

            return result;
        }

        public async Task<CustomerResultModel> Add(int andromedaSiteId, TokenResult tokenResult, AddCustomerRequestModel request)
        {
            CustomerResultModel result = new CustomerResultModel();

            using (IbsWebOrderApi.WebOrdersAPISoapClient client = await this.ibsGatewayService.CreateClient(andromedaSiteId))
            {
                var customer = await client.UpdateCustomerAsync(tokenResult.Token,
                    request.Email,
                    request.Customer,
                    request.LoyaltyCard
                );


                result.CustomerNumber = customer.m_lCustomerNo;

                if (customer.m_arrDeliveryAddresses != null)
                {
                    result.DeliveryAddresses = customer.m_arrDeliveryAddresses
                        .Select(e => new CustomerDeliveryAddressResultModel()
                        {
                            Adddress1 = e.m_szAdddress1,
                            Adddress2 = e.m_szAdddress2,
                            Adddress3 = e.m_szAdddress3,
                            Adddress4 = e.m_szAdddress4,
                            BuildingName = e.m_szBuildingName,
                            CompanyName = e.m_szCompanyName,
                            DeliveryInstructions = e.m_szDeliveryInstructions,
                            Email = e.m_szEmail,
                            ForeName = e.m_szForeName,
                            Latitude = e.m_dLatitude,
                            Location = e.m_szLocation,
                            Longitude = e.m_dLongitude,
                            Mobile = e.m_szMobile,
                            Phone = e.m_szPhone,
                            PostCode = e.m_szPostCode,
                            Surname = e.m_szSurname
                        })
                        .ToList();
                }
            }

            return result;
        }
    }
}
