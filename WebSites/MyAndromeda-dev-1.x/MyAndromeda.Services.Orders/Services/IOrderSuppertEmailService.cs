using System;
using System.Collections.Generic;
using MyAndromeda.Services.Orders.Emails;
using MyAndromeda.Core;

namespace MyAndromeda.Services.Orders.Services
{
    public interface IOrderSuppertEmailService : IDependency
    {
        OrderWatchingEmail CreateEmailBasedOnOrderIds(IEnumerable<Guid> ids);
    }
}
