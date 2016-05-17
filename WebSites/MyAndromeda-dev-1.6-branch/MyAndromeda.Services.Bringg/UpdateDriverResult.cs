using System;
using System.Linq;

namespace MyAndromeda.Services.Bringg
{
    public enum UpdateDriverResult
    {
        CantFindOrderInWarehouse,
        NoBringgTaskId,
        NoDriverName,
        NoDriverPhoneNumber,
        Success,
        UnknownError
    }
}
