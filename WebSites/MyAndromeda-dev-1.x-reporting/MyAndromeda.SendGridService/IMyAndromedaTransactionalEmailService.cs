using System;
using System.Threading.Tasks;
using MyAndromeda.Core;
using Postal;

namespace MyAndromeda.SendGridService
{
    public interface IMyAndromedaTransactionalEmailService: IEmailService, ITransientDependency
    {
        Task SendAsync(Email email, int andromedaSiteId, Guid orderId, Guid? customerId); 
    }
}