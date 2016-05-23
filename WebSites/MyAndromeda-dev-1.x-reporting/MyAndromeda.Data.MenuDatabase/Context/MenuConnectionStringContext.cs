using System.Configuration;
using System.Web;
using System;
using System.Linq;
using MyAndromeda.Framework.Contexts;
using System.Web.Hosting;

namespace MyAndromeda.Data.MenuDatabase.Context
{
    public class MenuConnectionStringContext 
    {
        private int andromedaSiteId;

        private readonly string connectionString;
        private readonly string localPath;

        public MenuConnectionStringContext(int andromedaSiteId) 
        {
            this.andromedaSiteId = andromedaSiteId;
            //this.currentSite = currentSite;

            this.connectionString = ConfigurationManager.ConnectionStrings["AttachMenuDbContext"].ConnectionString;
            this.localPath = ConfigurationManager.AppSettings["MyAndromeda.Menu.LocalFolder"];
        }

        public void Setup(int andromedaSiteId) 
        {
            this.andromedaSiteId = andromedaSiteId;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString 
        {
            get 
            {
                return string.Format(this.connectionString, this.andromedaSiteId);
            }
        }

        /// <summary>
        /// Gets the temp connection string.
        /// </summary>
        /// <value>The temp connection string.</value>
        public string TempConnectionString
        {
            get 
            {
                return string.Format(this.connectionString, this.andromedaSiteId).Replace("menu.mdb", "temp.mdb");
            }
        }

        private string LocalPath() 
        {
            var relativePath = string.Format(localPath, this.andromedaSiteId);
            var fullPath = relativePath.Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"));

            return fullPath;
        }

        /// <summary>
        /// Gets the zip path.
        /// </summary>
        /// <value>The zip path.</value>
        public string LocalZipPath
        {
            get 
            {
                return this.LocalPath()
                    .Replace("[File]", "menu.7z");
            }
        }

        /// <summary>
        /// Gets the temp path.
        /// </summary>
        /// <value>The temp path.</value>
        public string LocalTempPath
        {
            get 
            {
                return this.LocalPath()
                    .Replace("[File]", "temp.mdb");
            }
        }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value>The full path.</value>
        public string LocalFullPath 
        {
            get 
            {
                return this.LocalPath()
                    .Replace("[File]", "menu.mdb");
            }
        }
    }
}
