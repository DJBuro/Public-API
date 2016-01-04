using MyAndromeda.Data.DataWarehouse.Domain.Marketing;
using MyAndromedaDataAccess.Domain.DataWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAndromeda.Data.DataWarehouse.Domain.Marketing
{
    public class CustomerModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>The surname.</value>
        public string Surname { get; set; }

        public string Email
        {
            get
            {
                return this.GetEmail();
            }
        }

        /// <summary>
        /// Gets or sets the contact details.
        /// </summary>
        /// <value>The contact details.</value>
        public ICollection<ContactModel> ContactDetails { get; set; }
    }
}
