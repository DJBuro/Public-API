using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Services.Vouchers.Events
{
    public interface IInvalidCodeEventHandler : IDependency 
    {
        void InvalidCodeEntered(InvalidCodeEventContext context);
    }

}