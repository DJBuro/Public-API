using System;
using System.Linq;
using System.Net;
using System.Net.FtpClient;
using MyAndromeda.Configuration;
using MyAndromeda.Core;
using MyAndromeda.Data.MenuDatabase.Context;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Context.Ftp;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Ftp.Helpers;
using System.Threading;

namespace MyAndromeda.Menus.Ftp
{
    public interface IMenuFtpService : ITransientDependency , IDisposable
    {
        /// <summary>
        /// Copies the menu down.
        /// </summary>
        /// <param name="andromedaId">The andromeda id.</param>
        /// <param name="lastDownloaded">The last downloaded.</param>
        /// <returns></returns>
        FtpMenuContext CopyMenuDown(int andromedaId, DateTime? lastDownloaded);

        /// <summary>
        /// Checks the FTP 'to publish' date - when the menu can go live.
        /// </summary>
        /// <param name="andromedaId">The andromeda id.</param>
        /// <returns></returns>
        DateTime? CheckFtpToPublishDate(int andromedaId);

        /// <summary>
        /// Copies the menu up.
        /// </summary>
        /// <param name="andromedaId">The andromeda id.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        FtpMenuContext CopyMenuUp(int andromedaId, DateTime publishDate, string version);
    }

#pragma warning disable JustCode_CSharp_TypeFileNameMismatch // Types not matching file names
    public class MenuFtpService : IMenuFtpService
#pragma warning restore JustCode_CSharp_TypeFileNameMismatch // Types not matching file names
    {
        public const string FtpPathFormat = "Menus/{0}/{1}";

        //private static FtpListener ftpListener;
        private readonly string startPath;

        private readonly IAccessDatabaseEvent[] events;
        private readonly IFtpEvents[] ftpEvents;
        private readonly IZipMenuEvents[] zipEvents;
        private readonly IMyAndromedaLogger logger;
        
        public MenuFtpService(
            IMenuZipService menuZipService,
            IAccessDatabaseEvent[] newMenuDatabaseEvents,
            IFtpEvents[] ftpEvents,
            IZipMenuEvents[] zipEvents,
            IMyAndromedaLogger logger) 
        {
            this.MenuZipService = menuZipService;
            this.events = newMenuDatabaseEvents;
            this.ftpEvents = ftpEvents;
            this.zipEvents = zipEvents;

            this.startPath = MenuFtpSettings.RootFolder;

            //if (ftpListener == null)
            //{ 
            //    //FtpTrace bit
            //    ftpListener = new FtpListener(this.ftpEvents);
            //    FtpTrace.AddListener(ftpListener);
            //}

            this.logger = logger;
        }

        public static FtpClient CreateClient() 
        {
            var ftpClient = new FtpClient();
            ftpClient.Host = MenuFtpSettings.Host;
            var ftpCredentials = new System.Net.NetworkCredential(MenuFtpSettings.UserName, MenuFtpSettings.Password);
            ftpClient.Credentials = ftpCredentials;
            ftpClient.InternetProtocolVersions = FtpIpVersion.IPv4;
            //https://netftp.codeplex.com/discussions/535879
            ftpClient.StaleDataCheck = false;
            string mode = MenuFtpSettings.TransferMode;
            //ftpClient.DataConnectionType = mode.Equals("Passive", StringComparison.InvariantCultureIgnoreCase) ?
            //    //FtpDataConnectionType.PASVEX :
            //    //FtpDataConnectionType.EPSV :
            //    FtpDataConnectionType.AutoPassive :
            //    FtpDataConnectionType.AutoActive;
            ftpClient.DataConnectionType = FtpDataConnectionType.AutoPassive;

            return ftpClient;
        }

        public IMenuZipService MenuZipService { get; private set; }

        public void Dispose()
        {
            ///this.ftpClient.Dispose();
        }

        public FtpMenuContext CopyMenuDown(int andromedaId, DateTime? lastDownloaded)
        {
            var eventContext = new DatabaseUpdatedEventContext(andromedaId);
            
            FtpMenuContext ftpMenuContext = this.DownloadMenu(eventContext);

            return ftpMenuContext;
        }


        public DateTime? CheckFtpToPublishDate(int andromedaId)
        {
            var eventContext = new DatabaseUpdatedEventContext(andromedaId);

            return this.ReadFtpPublishDate(andromedaId, eventContext);
        }
  
        private DateTime ReadFtpPublishDate(int andromedaId, DatabaseUpdatedEventContext eventContext)
        {
            DateTime result = DateTime.MinValue; 
            foreach (var ev in this.events)
            {
                ev.CheckingDatabasePublishTime(eventContext);
            }

            FtpMenuContext ftpContext = GetFtpMenuContext(andromedaId);

            using (var ftpClient = CreateClient()) 
            { 
                FtpListItem ftpItemFile = FtpClientDownload.FindMenuItem(ftpClient, ftpContext, startPath, ftpEvents, eventContext,
                    failure: (ex) => LogFailureAction(ftpClient, where: "Find menu publish file failed", ex: ex, eventContext: eventContext),
                    success: (path) => LogSuccessAction(message: "find menu publish file folder succeeded", path: path)
                );

                if (ftpItemFile == null) { return DateTime.MinValue; }

                string dateStringValue = FtpClientDownload.ReadFileToString(ftpClient, ftpItemFile,
                    failure: (ex) => LogFailureAction(ftpClient, where: "Read menu publish file failed", ex: ex, eventContext: eventContext),
                    success: (date) => LogSuccessAction(message: "Read FTP Date succeeded", path: date));

                DateTime parsedValue;

                if (DateTime.TryParse(dateStringValue, out parsedValue))
                {
                    result = parsedValue;
                }
            }

            return result;
        }

        public FtpMenuContext CopyMenuUp(int andromedaId, DateTime publishDate, string version)
        {
            var eventContext = new DatabaseUpdatedEventContext(andromedaId)
            { 
                Version = version
            };

            FtpMenuContext ftpContext = GetFtpMenuContext(andromedaId);

            //zip process
            FtpClientUpload.ZipProcess(ftpContext, this.MenuZipService, this.zipEvents, eventContext);
            
            //ftp process
            if (this.UploadMenu(publishDate, version, eventContext))
            {
                this.logger.Debug(message: "Menu published to ftp successfully");
                ftpContext.HasUpdatedMenu = true;
            }
            else 
            {
                this.logger.Error(message: "Menu didn't publish to ftp successfully");
                ftpContext.HasUpdatedMenu = false;
            }

            //return what has happened.
            return ftpContext;
        }

        private FtpMenuContext DownloadMenu(DatabaseUpdatedEventContext eventContext) 
        {
            //ftpListener.AndromedaId = eventContext.AndromedaId;
            FtpMenuContext ftpContext = GetFtpMenuContext(eventContext.AndromedaId);
            MenuConnectionStringContext connectionStringContext = GetConnectionStringContext(eventContext.AndromedaId);

            foreach (var ev in this.events)
            {
                ev.CheckingDatabase(eventContext);
            }

            using (var ftpClient = CreateClient()) 
            { 

                FtpListItem folder = FtpClientDownload.FindMenuFolder(ftpClient, ftpContext, startPath, ftpEvents, eventContext,
                    failure: (ex) => LogFailureAction(ftpClient, string.Format(format: "Find menu 'folder' failed for {0}", arg0: eventContext.AndromedaId), ex, eventContext),
                    success: (path) => LogSuccessAction(message: "find menu folder succeeded", path: path)
                );

                //return if not found
                if (folder == null)
                {
                    ftpContext.HasFoundFolder = false;
                    return ftpContext;
                }

                FtpListItem menuFile = FtpClientDownload.FindMenuItem(ftpClient, ftpContext, folder.FullName, ftpEvents, eventContext,
                    failure: (ex) => LogFailureAction(ftpClient, string.Format(format: "Find menu 'file' failed for {0}", arg0: eventContext.AndromedaId), ex, eventContext),
                    success: (path) => LogSuccessAction(message: "find menu item succeeded", path: path)
                );

                if (menuFile == null)
                {
                    ftpContext.HasFoundFile = false;
                    return ftpContext;
                }

                string pathToDownload = connectionStringContext.LocalZipPath;

                FtpClientDownload.StreamToFile(
                    ftpClient, 
                    ftpContext, 
                    pathToDownload, 
                    menuFile,
                    events, 
                    eventContext,
                    failure: (ex) => LogFailureAction(ftpClient, where: "Download the menu file", ex: ex, eventContext: eventContext),
                    success: (path) => LogSuccessAction(message: "Write the menu file locally succeeded", path: path)
                );

                foreach (var ftpEvent in this.ftpEvents)
                {
                    ftpEvent.MenuDownloaded(eventContext);
                }

                string pathToExtract = connectionStringContext.LocalTempPath;

                FtpClientDownload.ExtractFileToTemp(this.MenuZipService, ftpContext,
                    pathToDownload,
                    pathToExtract,
                    zipEvents,
                    eventContext,
                    failure: (ex) => LogFailureAction(ftpClient, where: "Extraction of zip", ex: ex, eventContext: eventContext),
                    success: (path) => LogSuccessAction(message: "Extraction completed", path: path));
            }

            return ftpContext;
        }
        
        private bool UploadMenu(DateTime publishDate, string version, DatabaseUpdatedEventContext eventContext)
        {
            //ftp logging 
            foreach (var ev in this.ftpEvents) { ev.MenuUploading(eventContext); }

            if (string.IsNullOrWhiteSpace(version)) 
            {
                throw new ArgumentException(message: "version is missing");
            }

            bool failed = false;
            
            Action<string, Exception> failure = (from, ex) =>
            {
                this.logger.Error(from);
                this.logger.Error(ex);

                failed = true;
            };

            Action<string> logCompletedPath = (path) => {
                
                this.logger.Debug(format: "Path uploaded successfully: {0}.", args: new object[] { path });
                this.logger.Debug(format: "AndromedaId: {0}; Version: {1}; PublishDate: {2}", args: new object[] { eventContext.AndromedaId, version, publishDate.ToString("yyyy-MM-dd") });

                failed = false;
            };

            FtpMenuContext ftpContext = GetFtpMenuContext(eventContext.AndromedaId);

            int retryCount = 5;

            this.logger.Debug("Start uploading menu file: " + eventContext.AndromedaId);
            if (!failed) 
            {
                string menuUploadFailedMessage = "failure in uploading menu file. Retries left: {0}";
                for (var i = retryCount; i > 0; i--)
                {
                    using (var ftpClient = CreateClient()) 
                    { 
                        FtpClientUpload.UploadMenuFile(ftpClient,
                            ftpContext,
                            (msg) => this.logger.Debug(msg),
                            (ex) => failure(string.Format(menuUploadFailedMessage, i), ex),
                            (finalPath) => logCompletedPath(finalPath));

                        if (!failed) { i = 0; }
                        if (failed) { Thread.Sleep(millisecondsTimeout: 300); }
                    }
                }   

            }
            this.logger.Debug("completed uploading menu file: " + !failed);

            if (!failed) 
            {
                this.logger.Debug(message: "Start updating version file");

                string failedMsg = "Upload Version File. Reties left: {0}";
                for (var i = retryCount; i > 0; i--)
                {
                    using (var ftpClient = CreateClient()) 
                    { 
                        FtpClientUpload.UploadVersionFile(ftpClient,
                            ftpContext,
                            version,
                            (msg) => this.logger.Debug(msg),
                            (ex) => failure(string.Format(failedMsg, i), ex),
                            (path) => logCompletedPath(path));
                    
                        //keep retrying until failed == false
                        if (!failed) { i = 0; }
                        if (failed) { Thread.Sleep(millisecondsTimeout: 300); }
                    }
                }

                this.logger.Debug("Completed updating version file: " + !failed);
            }

            if (!failed) 
            {
                this.logger.Debug(message: "Start updating publish date file");

                string publishFailedMessage = "Upload Published File. Reties left: {0}"; 
                for (var i = retryCount; i > 0; i--)
                {
                    using (var ftpClient = CreateClient()) 
                    { 
                        FtpClientUpload.UploadPublishDateFile(ftpClient,
                            ftpContext,
                            publishDate,
                            (msg) => this.logger.Debug(msg),
                            (ex) => failure(string.Format(publishFailedMessage, i), ex),
                            (path) => logCompletedPath(path));
                    
                        if (!failed) { i = 0; }
                        if (failed) { Thread.Sleep(millisecondsTimeout: 300); }
                    }
                }

                this.logger.Debug("Completed updating publish date file: " + !failed);
            }

            foreach (var ev in this.ftpEvents) { ev.MenuUploaded(eventContext); }

            return !failed;
        }

        private void LogFailureAction(FtpClient client, string where, Exception ex, DatabaseUpdatedEventContext eventContext)
        {
            this.logger.Error(
                format: "Menu (check publish date) FTP Error - Connection with: [host: {0}, username: {1}, password: [removed]]", 
                args: new object[] { client.Host, client.Credentials.UserName });

            this.logger.Error(where);
            this.logger.Error(ex);

            foreach (var ev in this.ftpEvents)
            {
                ev.Error(eventContext, ex);
            }
        }

        private void LogSuccessAction(string message, string path)
        {
            this.logger.Debug(message);
            this.logger.Debug(path);
        }

        private static FtpMenuContext GetFtpMenuContext(int andromediaId)
        {
            MenuConnectionStringContext connectionStringContext = GetConnectionStringContext(andromediaId);

            var ftpMenuContext = new FtpMenuContext();

            ftpMenuContext.AndromedaId = andromediaId;
            ftpMenuContext.ExpectedMenuPath = connectionStringContext.LocalFullPath;
            ftpMenuContext.ExpectedZipPath = connectionStringContext.LocalZipPath;
            ftpMenuContext.ExpectedTempMenuPath = connectionStringContext.LocalTempPath;
            ftpMenuContext.AndromedaId = andromediaId;

            return ftpMenuContext;
        }

        private static MenuConnectionStringContext GetConnectionStringContext(int andromediaId)
        {
            return new MenuConnectionStringContext(andromediaId);
        }

    }
}
