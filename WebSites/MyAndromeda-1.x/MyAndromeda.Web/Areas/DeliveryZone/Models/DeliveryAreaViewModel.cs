using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.DeliveryZone.Model
{
    public class DeliveryAreaViewModel
    {
        public Guid Id { set; get; }
        public int SiteId { set; get; }
        public List<string> DeliveryAreasList { set; get; }
        public string DeliveryAreas { set; get; }
    }

    public static class DeliveryAreaViewModelExtensions
    {
        public static void ToViewModel(this IList<DeliveryArea> deliveryAreas, Model.DeliveryAreaViewModel deliveryZone)
        {
            deliveryZone.DeliveryAreasList = new List<string>();
            if (deliveryAreas != null)
            {
                foreach (var item in deliveryAreas)
                {
                    if (!string.IsNullOrEmpty(item.DeliveryArea1))
                    {
                        if (!deliveryZone.DeliveryAreasList.Contains(item.DeliveryArea1))
                            deliveryZone.DeliveryAreasList.Add(item.DeliveryArea1);
                    }                 

                }
                if (deliveryAreas.Count() != 0)
                {
                    deliveryZone.SiteId = deliveryAreas.FirstOrDefault().StoreId;
                }
                deliveryZone.DeliveryAreas = String.Join("\r\n", deliveryZone.DeliveryAreasList);
            }
        }

        public static void UpdateFromViewModel(this Model.DeliveryAreaViewModel deliveryZone, IList<DeliveryArea> deliveryAreas)
        {
            if (deliveryZone != null)
            {
                if (!string.IsNullOrEmpty(deliveryZone.DeliveryAreas))
                {
                    List<string> deliveryAreaCodes = deliveryZone.DeliveryAreas.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Where(s => (!string.IsNullOrEmpty(s.Trim()))).Select(s => s).ToList();
                    foreach (var item in deliveryAreaCodes)
                    {
                        deliveryAreas.Add(new DeliveryArea { StoreId = deliveryZone.SiteId, DeliveryArea1 = item.Trim(), Removed = false });
                    }
                }
            }
        }
    }
}