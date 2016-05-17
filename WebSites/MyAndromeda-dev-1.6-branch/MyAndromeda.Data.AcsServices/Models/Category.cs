using System.Collections.Generic;
using System.Diagnostics;

namespace MyAndromeda.Data.AcsServices.Models
{
    [DebuggerDisplay("{Id} - {Name}")]
    public class Category
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public IList<Category> Children { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        public int ParentId { get; set; }
    }
}