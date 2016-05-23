using MyAndromedaDataAccess.Domain.DataWarehouse;

namespace MyAndromeda.Data.DataWarehouse.Domain.Marketing
{
    public class ContactModel 
    {
        /// <summary>
        /// Gets or sets the type of the contact.
        /// </summary>
        /// <value>The type of the contact.</value>
        public ContactType ContactType { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}