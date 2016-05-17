using System.Threading.Tasks;
using MyAndromeda.Services.Ibs;
using MyAndromeda.Services.Ibs.Events;
using MyAndromeda.Services.Ibs.Models;
using System.Monads;
using System;
using System.Linq;
using MyAndromeda.Framework.Helpers;
using MyAndromeda.Services.Ibs.Checks;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class OrderService : IOrderService 
    {
        private readonly IIbsOrderEvents[] orderEvents;
        private readonly IbsGatewayService ibsGatewayService;


        public OrderService(IIbsOrderEvents[] orderEvents, IbsGatewayService ibsGatewayService)
        {
            this.ibsGatewayService = ibsGatewayService;
            this.orderEvents = orderEvents;
        }

        public async Task<AddOrderResult> SendOrder(
            int andromedaSiteId,
            Models.TokenResult token, 
            Models.LocationResult location,
            Models.CustomerResultModel customer,
            Models.AddOrderRequest request) 
        {
            AddOrderResult model = null;

            token
                .Token
                .CheckNull("Token");

            location
                .LocationCode
                .CheckNull("LocationCode");

            await this.orderEvents.ForEachAsync(async (ev)=> {
                await ev.OrderSendingAsync(location, customer, request);
            });

            using (IbsWebOrderApi.WebOrdersAPISoapClient client = await this.ibsGatewayService.CreateClient(andromedaSiteId)) 
            {
                
                var result = await client.AddOrderAsync(
                    token.Token,
                    location.LocationCode,
                    request.Commit,
                    request.OrderType,
                    request.WantedOrderDay, request.WantedOrderMonth, request.WantedOrderYear,
                    request.TimeSlotFrom, request.TimeSlotTo,
                    request.CustomerNo, request.CustomerDetails,
                    request.LocationTimeCat,
                    request.DeliveryInstructions,
                    request.UserReference,
                    request.CostCentre,
                    request.Covers,
                    request.Items.ToArray(),
                    request.PhotoBytes,
                    request.TableNumber,
                    request.TransactionId,
                    request.OrderPlacedDay, request.OrderPlacedMonth, request.OrderPlacedYear,
                    request.OrderPlacedHour, request.OrderPlacedMin,
                    request.RoomSelection,
                    request.AreaSelection,
                    request.SendEmail,
                    request.LoyaltyCardNumber,
                    request.PayOnCollectionOrDelivery,
                    request.PrintAtStore,
                    request.ProductionType,
                    request.TrainingMode,
                    request.ConfirmOnPos);


                if (result.AddOrderResult.m_objError.m_bError)
                {
                    var error = result.AddOrderResult.m_objError;
                    var failed = new Models.AddOrderFailure();
                    
                    failed.Error = error.m_szError;

                    await this.orderEvents.ForEachAsync(async (ev) =>
                    {
                        await ev.OrderFailedAsync(location, customer, failed);
                    });
                    
                    throw new Exception(result.AddOrderResult.m_objError.m_szError);
                }

                model = new AddOrderResult() 
                { 
                    Id = result.AddOrderResult.m_lOrderNo,
                    CustomerId = result.AddOrderResult.m_lCustomerNo
                };

                
            }

            await this.orderEvents.ForEachAsync(async (ev) =>
            {
                await ev.OrderSentAsync(location, customer, model);
            });

            return model; 
        }
    }
}