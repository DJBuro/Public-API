using System.Collections.Generic;

namespace MyAndromeda.Framework.Authorization
{
    /// <summary>
    /// What the application expects first if there isn't a row configuration for the permission
    /// </summary>
    public class PermissionStereotype
    {
        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>The permission.</value>
        public Permission Permission { get; set; }

        /// <summary>
        /// Gets or sets the allow roles.
        /// </summary>
        /// <value>The allow roles.</value>
        public IEnumerable<string> AllowRoles { get; set; }
    }
}