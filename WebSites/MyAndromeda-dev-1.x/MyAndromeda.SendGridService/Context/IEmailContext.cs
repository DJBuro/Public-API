using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain.Marketing;

namespace MyAndromeda.SendGridService.Context
{
    public interface IEmailContext : IDependency
    {
        EmailSettings EmailSettings { get; }
    }
}
