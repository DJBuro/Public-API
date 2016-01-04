using System.Collections.Generic;
using MyAndromeda.Core.User;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Menus.Events
{
    public class EditToppingItemsContext 
    {
        public EditToppingItemsContext(
            MyAndromedaUser editor, 
            Site site, 
            IEnumerable<MyAndromedaTopping> toppings, 
            string section) 
        {
            this.Editor = editor;
            this.Site = site;
            this.Toppings = toppings;
            this.Section = section;
        }

        /// <summary>
        /// Gets or sets the editor.
        /// </summary>
        /// <value>The editor.</value>
        public MyAndromedaUser Editor { get; private set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>The site.</value>
        public Site Site { get; private set; }

        /// <summary>
        /// Gets or sets the toppings.
        /// </summary>
        /// <value>The toppings.</value>
        public IEnumerable<MyAndromedaTopping> Toppings { get; private set; }

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        /// <value>The section.</value>
        public string Section { get; private set; }
    }
}