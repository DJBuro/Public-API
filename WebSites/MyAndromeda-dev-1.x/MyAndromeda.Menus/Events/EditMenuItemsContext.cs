using System.Collections.Generic;
using MyAndromeda.Core.User;
using MyAndromeda.Data.AcsServices.Models;
using Domain = MyAndromeda.Data.Domain;

namespace MyAndromeda.Menus.Events
{
    public class EditMenuItemsContext 
    {
        public EditMenuItemsContext(MyAndromedaUser editor, Domain.SiteDomainModel site, IEnumerable<MyAndromedaMenuItem> items, string section) 
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
        public Domain.SiteDomainModel Site { get; private set; }

        /// <summary>
        /// Gets or sets the edited items.
        /// </summary>
        /// <value>The edited items.</value>
        public IEnumerable<MyAndromedaMenuItem> EditedItems { get; private set; }
    }
}