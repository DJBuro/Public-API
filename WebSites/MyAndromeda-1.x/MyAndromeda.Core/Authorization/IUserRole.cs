using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core.Authorization;
using System.Security;

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
