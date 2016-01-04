using System;
using Microsoft.AspNet.SignalR;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Menus.Events;

namespace MyAndromeda.SignalRHubs.Handlers
{
    public class NewMenuDatabaseEventHandler : IAccessDatabaseEvent, IFtpEvents, IZipMenuEvents
    {
        private readonly IHubContext hubContext;

        public NewMenuDatabaseEventHandler() 
        {
            this.hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.StoreHub>();
        }

        public void Error(DatabaseUpdatedEventContext context, Exception e)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Error: '{1}' at {2}", context.AndromedaId, e.Message, DateTime.UtcNow));
        }

        public void MenuUploaded(DatabaseUpdatedEventContext context)
        {
            string message = "Uploaded menu";
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- '{1}' at {2}", context.AndromedaId, message, DateTime.UtcNow));
        }

        public void MenuUploading(DatabaseUpdatedEventContext context)
        {
            string message = "Uploading menu";
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- '{1}' at {2}", context.AndromedaId, message, DateTime.UtcNow));
        }

        public void TransactionLog(DatabaseUpdatedEventContext context, string message)
        {
            hubContext.Clients.All
                .transactionLog(string.Format("All: {0}:- '{1}' at {2}", context.AndromedaId, message, DateTime.UtcNow));
            hubContext.Clients
                .Group(hubContext.GetRoleGroup(ExpectedRoles.Administrator))
                .transactionLog(string.Format("{0}:- '{1}' at {2}", context.AndromedaId, message, DateTime.UtcNow));
        }

        public void FtpLoading(DatabaseUpdatedEventContext context, string startPath)
        {
            hubContext.Clients.All.ping(string.Format("{0}:- Ftp Loading Start path: '{1}' at {2}", context.AndromedaId, startPath, DateTime.UtcNow));
        }

        public void FtpFilesLoading(DatabaseUpdatedEventContext context, string path)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Checking FTP File Path: '{1}' at {2}", context.AndromedaId, path, DateTime.UtcNow));
        }

        public void CheckingDatabasePublishTime(DatabaseUpdatedEventContext eventContext)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(eventContext.AndromedaId)).checkingDatabasePublishTime(eventContext);
        }

        public void CheckingDatabase(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("Store group: {0}", context.AndromedaId));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).checkingDatabaseEvent(context);
        }

        public void MenuDownloading(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}: - Store group DownloadingDatabase: {1}", context.AndromedaId, DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).downloadingDatabaseEvent(context);
        }

        public void MenuDownloaded(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Store DownloadedDatabase: {1}", context.AndromedaId, DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).downloadedDatabaseEvent(context);
        }

        public void MenuExtracting(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Store group ExtractingDatabase: {1}", context.AndromedaId, DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).extractingDatabaseEvent(context);
        }

        public void MenuExtracted(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Store group ExtractedDatabase: {1}", context.AndromedaId, DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).extractedDatabaseEvent(context);
        }

        public void MenuZipped(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Menu Zipped: {1}", context.AndromedaId, DateTime.UtcNow));
        }

        public void MenuZipping(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Menu Zipping: {1}", context.AndromedaId, DateTime.UtcNow));
        }

        public void MenuExtractionFailed(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Extracting failed: {1}", context.AndromedaId, DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).extractedDatabaseEvent(context);
        }

        public void ComparingDatabases(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Store group ComparingDatabases: {1}", context.AndromedaId, DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).comparingDatabaseEvent(context);
        }

        public void CopiedDatabase(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Store group CopiedDatabase: {1}", context.AndromedaId, DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).copiedDatabaseEvent(context);
        }

        public void DatabaseNotChanged(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Store group DatabaseNotChanged: {1}", context.AndromedaId, DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).notChangedDatabaseEvent(context);
        }

        public void Notify(int andromediaId, string message)
        {
            hubContext.Clients.Group(andromediaId.ToString()).ping(string.Format("{0}:- Notify: {1}, {2}", andromediaId, message, DateTime.UtcNow));
        }

        public void FinishedCheckingForDatabase(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Finished checking for menu: {1}", context.AndromedaId, DateTime.UtcNow));
        }

        public void StartedCheckingForDatabase(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Started checking for menu: {1}", context.AndromedaId, DateTime.UtcNow));
        }

        public void FailedDownloadingDatabase(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("{0}:- Failed downloading menu for: {1}", context.AndromedaId, DateTime.UtcNow));
        }

        public void NewDatabaseLoaded(DatabaseUpdatedEventContext context)
        {
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).ping(string.Format("Store group NewDatabaseLoaded: {0}", DateTime.UtcNow));
            hubContext.Clients.Group(hubContext.GetStoreGroup(context.AndromedaId)).newDatabaseEvent(context);
        }
    }
}