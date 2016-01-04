using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Core.Authorization
{
    public interface IUserRole
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets the associated permissions 
        /// </summary>
        /// <value>The effective permissions.</value>
        IEnumerable<IPermission> EffectivePermissions { get; set; }
    }

}
