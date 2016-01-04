using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromedaDataAccess.Domain;
using MyAndromeda.Core.User;

namespace MyAndromeda.Menus.Events
{
    public interface IMenuItemChangedEvent : IDependency
    {
        void UpdatedMenuItems(EditMenuItemsContext context);
        void UpdatedToppings(EditToppingItemsContext context);
    }

    public class EditMenuItemsContext 
    {
        public EditMenuItemsContext(MyAndromedaUser editor, Site site, IEnumerable<MyAndromedaMenuItem> items, string section) 
        {
            this.Editor = editor;
            this.Site = site;
            this.EditedItems = items;
            this.Section = section;
        }

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        /// <value>The section.</value>
        public string Section { get; private set; }

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
        /// Gets or sets the edited items.
        /// </summary>
        /// <value>The edited items.</value>
        public IEnumerable<MyAndromedaMenuItem> EditedItems { get; private set; }
    }

    public class EditToppingItemsContext 
    {
        public EditToppingItemsContext(MyAndromedaUser editor, Site site, IEnumerable<MyAndromedaTopping> toppings, string section) 
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
