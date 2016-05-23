using System;
using MyAndromeda.Core;

namespace MyAndromeda.Menus.Events
{
    public interface IFtpEvents : IDependency
    {
        void MenuUploaded(DatabaseUpdatedEventContext context);

        void MenuUploading(DatabaseUpdatedEventContext context);

        void TransactionLog(DatabaseUpdatedEventContext newOrUpdatedDatabaseContext, string message);

        void FtpLoading(DatabaseUpdatedEventContext context, string startPath);

        void FtpFilesLoading(DatabaseUpdatedEventContext newOrUpdatedDatabaseContext, string path);

        void MenuDownloading(DatabaseUpdatedEventContext context);

        void MenuDownloaded(DatabaseUpdatedEventContext context);

        void Error(DatabaseUpdatedEventContext context, Exception e);
    }
}