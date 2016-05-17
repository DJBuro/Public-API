using MyAndromeda.Core;
using MyAndromeda.Services.Ibs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Ibs
{
    public interface IMenuService : ITransientDependency
    {
        /// <summary>
        /// Gets the menu.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        Task<MenuResult> GetMenu(int andromedaSiteId, Models.TokenResult token, Models.LocationResult location);
    }
}
