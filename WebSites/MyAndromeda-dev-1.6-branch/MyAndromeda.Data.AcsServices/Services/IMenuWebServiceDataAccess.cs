using System.Collections.Generic;
using System.Threading.Tasks;
using MyAndromeda.Core;

namespace MyAndromeda.Data.AcsServices.Services
{
    /// <summary>
    /// external call please 
    /// </summary>
    public interface IMenuWebServiceDataAccess : IDependency
    {
        Task<string> FetchFromServiceAsync(IEnumerable<string> serverEndpoints);
    }
}