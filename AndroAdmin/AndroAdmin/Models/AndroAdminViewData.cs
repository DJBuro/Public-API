using System.Collections.Generic;
using System.Web.Mvc;
using AndroAdmin.Dao.Domain;
using AndroAdmin.Mvc;

namespace AndroAdmin.Models
{
    public class AndroAdminViewData : SiteViewData
    {
        /// <summary>
        /// Masterpage, all other pages inherit from this.
        /// </summary>
        public class AndroAdminBaseViewData : SiteViewData
        {
            public AndroUser AndroUser;

        }

        public class IndexViewData : AndroAdminBaseViewData
        {
            public Dictionary<string, string> English;
            public Dictionary<string, string> French;
            public Dictionary<string, string> Polish;
            public Dictionary<string, string> Bulgarian;
            public Dictionary<string, string> Russian;

            public string TranslatingLanguage;
            public string TranslatingProject;
            public string EnglishWord;
            public string TranslatedWord;
            public string TranslatingWordId;
            public string TranslatingLanguageId;

        }

        public class AdminViewData : AndroAdminBaseViewData
        {
            public IList<AndroUser> AndroUsers;
            public AndroUser AndroEditUser;
            public IList<AndroUserPermission> AndroUserPermissions;
            public AndroUserPermission AndroUserPermission;

            public IEnumerable<SelectListItem> LanguageListItems;

            

            public IList<Project> Projects;
            public Project Project;
        }

    }
}
