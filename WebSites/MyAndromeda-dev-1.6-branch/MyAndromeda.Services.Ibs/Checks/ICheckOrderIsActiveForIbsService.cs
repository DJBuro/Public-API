using System;
using MyAndromeda.Core;

namespace MyAndromeda.Services.Ibs.Checks
{

    public interface ICheckOrderIsActiveForIbsService : ISingletonDependency
    {
        bool IsOrderProcessing(Guid orderHeaderId);

        void AddOrderToBeProcessed(Guid orderHeaderId, int andromedaStoreId);

        void RemoveOrderFromProcessing(Guid orderHeaderId);
    }

}