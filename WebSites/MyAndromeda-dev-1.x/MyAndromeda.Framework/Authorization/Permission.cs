using System;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Framework.Authorization
{
    

    public class Permission : IPermission
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get;set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category that the permission belongs tp.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the description of the permission - what it is supposed to do.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the permission.
        /// </summary>
        /// <value>The type of the permission.</value>
        public PermissionType PermissionType { get; set; }
    }
}