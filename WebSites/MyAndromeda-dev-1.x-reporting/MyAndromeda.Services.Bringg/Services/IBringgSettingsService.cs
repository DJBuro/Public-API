using MyAndromeda.Core;
using System;
namespace MyAndromeda.Services.Bringg.Services
{
    public interface IBringgSettingsService : IDependency
    {
        MyAndromeda.Services.Bringg.Models.BringgAuth Get(int androAdminStoreId);
    }
}
