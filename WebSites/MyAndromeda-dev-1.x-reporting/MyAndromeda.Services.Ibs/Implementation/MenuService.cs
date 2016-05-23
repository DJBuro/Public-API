using System;
using MyAndromeda.Services.Ibs.IbsWebOrderApi;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Logging;
using Newtonsoft.Json;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class MenuService : IMenuService 
    {
        private readonly IbsGatewayService ibsGatewayService;
        private readonly IMyAndromedaLogger logger;

        public MenuService(IbsGatewayService ibsGatewayService, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.ibsGatewayService = ibsGatewayService;
        }

        public async Task<Models.MenuResult> GetMenu(int andromedaSiteId, Models.TokenResult token, Models.LocationResult location)
        {
            Models.MenuResult result = null;

            using (WebOrdersAPISoapClient client = await this.ibsGatewayService.CreateClient(andromedaSiteId))
            {
                var r = await client.GetMenuAsync(token.Token, location.LocationCode, false);

                //var a = JsonConvert.SerializeObject(r);
                //this.logger.Error(a);

                result = new Models.MenuResult()
                {
                    WebOrderingPriceLevel = r.m_iWebOrderingPriceLevel,
                    Items = r.m_arrItems.Select(e => new Models.MenuSection()
                    {
                        Id = e.m_lRef,
                        Description = e.m_szDescription,
                        SubCategroyItems = e.m_arrOrderSubCat == null 
                            ? new List<Models.MenuSubCatItem>()
                            : e.m_arrOrderSubCat.Select(k => new Models.MenuSubCatItem()
                            {
                                Id = k.m_lRef,
                                Description = k.m_szDescription,
                                Items = k.m_arrPlus == null 
                                ? new List<Models.MenuItem>()
                                : k.m_arrPlus.Select(m => new Models.MenuItem()
                                {
                                    
                                    PluNumber = m.m_lPluNo,
                                    PluType = m.m_lPluType,
                                    KpDescription = m.m_szKPDescription,
                                    Description = m.m_szDescription,
                                    Details = m.m_szDetails,
                                    InUse = m.m_bInUse,
                                    Prices = m.m_arrPrices == null 
                                    ? new List<Models.PriceModel>() 
                                    : m.m_arrPrices.Select(p => new Models.PriceModel() { 
                                        ModNo = p.m_iModNo,
                                        Price = p.m_dPrice
                                    }).ToList()

                                }).ToList()
                            })
                            .ToList()
                    }).ToList(),

                    MajorGroups = r.m_arrMajorGroups == null 
                        ? new List<Models.MenuGroup>()
                        : r.m_arrMajorGroups.Select(e => new Models.MenuGroup()
                        {
                            Description = e.m_szDescription,
                            Id = e.m_lMajorGroupNo,
                            PrimaryGroupId = e.m_lPrimaryGroupNo
                        }).ToList(),

                    Modifiers = r.m_arrModifiers == null 
                        ? new List<Models.Modifier>()
                        : r.m_arrModifiers.Select(e => new Models.Modifier()
                        {
                            Factor = e.m_dFactor,
                            Id = e.m_iModNo,
                            Description = e.m_szDescription,
                            Prefix = e.m_szPrefix
                        }).ToList(),

                    PrimaryGroups = r.m_arrPrimaryGroups == null 
                        ? new List<Models.PrimaryGroup>() 
                        : r.m_arrPrimaryGroups.Select(e => new Models.PrimaryGroup()
                        {
                            Id = e.m_lPrimaryGroupNo,
                            Description = e.m_szDescription,
                        }).ToList(),

                    SalesGroups = r.m_arrSalesGroups == null 
                        ? new List<Models.SalesGroup>() 
                        : r.m_arrSalesGroups.Select(e => new Models.SalesGroup()
                        {
                            Id = e.m_lSalesGroupNo,
                            MajorGroupId = e.m_lMajorGroupNo,
                            Description = e.m_szDescription
                        }).ToList()
                };
            }

            return result;
        }
    }
}