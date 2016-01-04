using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.Model.MyAndromeda
{
    public static class MenuDbExtensions
    {
        public static void Copy(this SiteMenuFtpBackupUploadTask item, SiteMenuFtpBackupUploadTask copyFrom) 
        {
            item.TryTask = copyFrom.TryTask;
            item.TaskStarted = copyFrom.TaskStarted;

            item.LastStartedUtc = copyFrom.LastStartedUtc;
            item.LastTriedUtc = copyFrom.LastTriedUtc;
            item.LastTryCount = copyFrom.LastTryCount;

            item.LastCompletedUtc = copyFrom.LastCompletedUtc;
            item.TaskComplete = copyFrom.TaskComplete;
        }

        public static void Copy(this SiteMenuFtpBackupDownloadTask item, SiteMenuFtpBackupDownloadTask copyFrom) 
        {
            item.TryTask = copyFrom.TryTask;
            item.TaskStarted = copyFrom.TaskStarted;

            item.LastStartedUtc = copyFrom.LastStartedUtc;
            item.LastTriedUtc = copyFrom.LastTriedUtc;
            item.LastTryCount = copyFrom.LastTryCount;

            item.LastCompletedUtc = copyFrom.LastCompletedUtc;
            item.TaskCompleted = copyFrom.TaskCompleted;

            //download specific 
            item.LastModifiedFtpDateUtc = copyFrom.LastModifiedFtpDateUtc;
        }

        public static void Copy(this SiteMenuPublishTask entity, SiteMenuPublishTask model) 
        {
            entity.TryTask = model.TryTask;
            entity.TaskStarted = model.TaskStarted;
            entity.TaskComplete = model.TaskComplete;
            entity.LastTriedUtc = model.LastTriedUtc;
            entity.LastTryCount = model.LastTryCount;

            entity.LastStartedUtc = model.LastStartedUtc;
            entity.LastCompletedUtc = model.LastCompletedUtc;
            
            //publish specific
            entity.LastKnownFtpSitePublish = model.LastKnownFtpSitePublish;

            entity.PublishOn = model.PublishOn;
        }
    }
}