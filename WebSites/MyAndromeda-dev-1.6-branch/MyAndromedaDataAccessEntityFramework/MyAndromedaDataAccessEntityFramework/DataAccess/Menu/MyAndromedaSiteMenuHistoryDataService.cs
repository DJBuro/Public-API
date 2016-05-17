using System;
using System.Linq;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Data.DataAccess.Menu;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public class MyAndromedaSiteMenuHistoryDataService : IMyAndromedaSiteMenuHistoryDataService
    {
        public void Create(Guid siteMenuId, DateTime createdOnUtc, DateTime publishedOnUtc, string userName, bool publishAll, bool publishMenu, bool publishThumbnails)
        {
            using (var dbContext = new MyAndromedaDbContext()) 
            {
                var table = dbContext.SiteMenuPublishTaskHistories;
                var record = table.Create();

                record.SiteMenuId = siteMenuId;
                record.CreatedOnUtc = createdOnUtc;
                record.PublishOnUtc = publishedOnUtc;
                record.PublishedBy = userName;
                record.PublishAll = publishAll;
                record.PublishMenu = publishMenu;
                record.PublishThumbnails = publishThumbnails;
                
                table.Add(record);
            }
        }
    }
}