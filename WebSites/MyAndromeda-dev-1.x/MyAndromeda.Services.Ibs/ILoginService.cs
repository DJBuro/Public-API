using MyAndromeda.Core;
using System;
using MyAndromeda.Services.Ibs.Models;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs
{
    public interface ILoginService : ITransientDependency
    {
        Task<TokenResult> LoginAsync(int andromedaSiteId, IbsStoreSettings request);
        Task<TokenResult> LoginAsync(int andromedaSiteId);
    }
}
