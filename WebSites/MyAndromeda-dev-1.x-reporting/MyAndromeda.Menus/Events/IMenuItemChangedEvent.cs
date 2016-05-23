using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromedaDataAccess.Domain;
using MyAndromeda.Core.User;

namespace MyAndromeda.Menus.Events
{
    public interface IMenuItemChangedEvent : IDependency
    {
        /// <summary>
        /// Updates the menu items.
        /// </summary>
        /// <param name="context">The context.</param>
        Task UpdatedMenuItemsAsync(EditMenuItemsContext context);

        /// <summary>
        /// Updates the toppings.
        /// </summary>
        /// <param name="context">The context.</param>
        Task UpdatedToppingsAsync(EditToppingItemsContext context);
    }
}
