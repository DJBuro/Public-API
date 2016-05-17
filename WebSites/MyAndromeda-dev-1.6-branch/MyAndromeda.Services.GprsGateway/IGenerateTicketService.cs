using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using System.Text;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Services.GprsGateway
{
    public interface IGenerateTicketService : IDependency 
    {
        /// <summary>
        /// Generates the specified store.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        Task<string> Generate(Store store, OrderHeader order);
    }
}