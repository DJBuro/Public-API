using System;
using System.Linq;
using MyAndromeda.Data.Model;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.DataAccess.Menu;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public class MyAndromedaMenuSyncDataService : IMyAndromedaMenuSyncDataService 
    {
        public MyAndromedaMenuSyncDataService() 
        { 
        
        }

        public void SyncMenuThumbnails(int andromedaSiteId, string xml, string json)
        {
            using (var androAdminDbContext = new AndroAdminDbContext()) 
            {
                var menuThumbnailTable = androAdminDbContext.StoreMenuThumbnails;
                var query = menuThumbnailTable.Where(e => e.Store.AndromedaSiteId == andromedaSiteId);
                var menuThumbnailResult = query.SingleOrDefault() ?? this.CreateThumbnailRow(androAdminDbContext, andromedaSiteId);

                menuThumbnailResult.LastUpdate = DateTime.UtcNow;
                menuThumbnailResult.XmlMenuThumbnailData = xml;
                menuThumbnailResult.JsonMenuThumbnailsData = json;

                //update version for cloud sync
                menuThumbnailResult.Version = Data.Model.DataVersionHelper.GetNextDataVersion(androAdminDbContext);

                androAdminDbContext.SaveChanges();
            }
        }

        public void SyncActualMenu(int andromedaSiteId, string xml, string json, int? version)
        {
            using (var androAdminDbContext = new Model.AndroAdmin.AndroAdminDbContext())
            {
                var table = androAdminDbContext.StoreMenus;
                var query = table.Where(e => e.Store.AndromedaSiteId == andromedaSiteId);
                var menuRow = query.ToArray();

                var dataVersion = androAdminDbContext.GetNextDataVersionForEntity();

                Model.AndroAdmin.StoreMenu xmlRecord =
                                                      menuRow.FirstOrDefault(e => e.MenuType == "XML") ?? this.CreateXmlMenuRow(androAdminDbContext, andromedaSiteId);
                Model.AndroAdmin.StoreMenu jsonRecord =
                                                       menuRow.FirstOrDefault(e => e.MenuType == "JSON") ?? this.CreateJsonMenuRow(androAdminDbContext, andromedaSiteId);

                xmlRecord.DataVersion = dataVersion;
                jsonRecord.DataVersion = dataVersion;

                jsonRecord.MenuData = json;

                xmlRecord.Version = version.GetValueOrDefault();
                jsonRecord.Version = version.GetValueOrDefault();

                xmlRecord.LastUpdated = DateTime.UtcNow;
                jsonRecord.LastUpdated = DateTime.UtcNow;

                androAdminDbContext.SaveChanges();
            }
        }

        public bool IsThereAnyPointSyncing(int andromedaSiteId)
        {
            bool result = false;
            using (var myAndromediaDbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var menuTable = myAndromediaDbContext.SiteMenus;
                var menuTableQuery = menuTable.Where(e => e.AndromediaId == andromedaSiteId);
                var menuResult = menuTableQuery.SingleOrDefault();

                if (menuResult == null)
                {
                    return false;
                }

                using (var androAdminDbContex = new Model.AndroAdmin.AndroAdminDbContext()) 
                {
                    var menuThumbnailTable = androAdminDbContex.StoreMenuThumbnails;
                    var menuThumbnailTableQuery = menuThumbnailTable.Where(e => e.Store.AndromedaSiteId == andromedaSiteId);
                    var menuThumbnailResult = menuThumbnailTableQuery.SingleOrDefault();

                    //no sync record yet 
                    if (menuThumbnailResult == null)
                    {
                        return true;
                    }

                    result = menuThumbnailResult.LastUpdate < menuResult.LastUpdatedUtc; 
                }
            }

            return result;
        }

        private StoreMenu CreateXmlMenuRow(Model.AndroAdmin.AndroAdminDbContext dbContext, int andromedaSiteId) 
        {
            var store = dbContext.Stores.Single(e => e.AndromedaSiteId == andromedaSiteId);

            var table = dbContext.StoreMenus;
            var xmlEntity = table.Add(table.Create());
            xmlEntity.MenuType = "XML";
            xmlEntity.Store = store;
            xmlEntity.LastUpdated = DateTime.UtcNow;
            
            return xmlEntity;
        }

        private StoreMenu CreateJsonMenuRow(Model.AndroAdmin.AndroAdminDbContext dbContext, int andromedaSiteId) 
        {
            var store = dbContext.Stores.Single(e => e.AndromedaSiteId == andromedaSiteId);

            var table = dbContext.StoreMenus;
            var jsonEntity = table.Add(table.Create());
            jsonEntity.MenuType = "JSON";
            jsonEntity.Store = store;
            jsonEntity.LastUpdated = DateTime.UtcNow;

            return jsonEntity;
        }
        
        private StoreMenuThumbnail CreateThumbnailRow(Model.AndroAdmin.AndroAdminDbContext dbContext, int andromediaSiteId) 
        {
            var table = dbContext.StoreMenuThumbnails;
            var entity = table.Add(table.Create());

            entity.XmlMenuThumbnailData = string.Empty;
            entity.JsonMenuThumbnailsData = string.Empty;

            entity.Store = dbContext.Stores.Single(e => e.AndromedaSiteId == andromediaSiteId);

            dbContext.SaveChanges();
            return entity;
        }
    }
}