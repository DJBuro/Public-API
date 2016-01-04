using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;

namespace MyAndromeda.Menus.Services.Export
{
    public interface IAcsMenuXmlJsonSyncDataService : ITransientDependency
    {
        Task UpdateAcsMenuAsync(int andromedaSiteId);
    }
}
