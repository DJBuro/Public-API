using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Bringg.Services.Checks
{

    public interface ICheckOrdersForBringgDontDirectingInclude 
    {
        bool IsOrderProcessing(Guid orderHeaderId);

        void AddOrderToBeProcessed(Guid orderHeaderId, int andromedaStoreId);

        void RemoveOrderFromProcessing(Guid orderHeaderId);
    }

}
