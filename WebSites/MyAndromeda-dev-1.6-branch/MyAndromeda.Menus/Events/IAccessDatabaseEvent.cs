using System;
using MyAndromeda.Core;

namespace MyAndromeda.Menus.Events
{
    public interface IAccessDatabaseEvent : IDependency 
    {
        void CheckingDatabasePublishTime(DatabaseUpdatedEventContext eventContext);

        void CheckingDatabase(DatabaseUpdatedEventContext context);

        void ComparingDatabases(DatabaseUpdatedEventContext context);

        /// <summary>
        /// Made the temp database the primary one over the existing.
        /// </summary>
        /// <param name="context">The context.</param>
        void CopiedDatabase(DatabaseUpdatedEventContext context);
        
        void DatabaseNotChanged(DatabaseUpdatedEventContext context);

        void Notify(int andromedaId, string message);

        void FinishedCheckingForDatabase(DatabaseUpdatedEventContext context);

        void StartedCheckingForDatabase(DatabaseUpdatedEventContext newOrUpdatedDatabaseContext);

        void FailedDownloadingDatabase(DatabaseUpdatedEventContext context);
    }
}