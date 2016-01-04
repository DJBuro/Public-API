using System;
using System.IO;
using System.Linq;
using System.Net.FtpClient;
using MyAndromeda.Menus.Context.Ftp;
using MyAndromeda.Menus.Events;

namespace MyAndromeda.Menus.Ftp.Helpers
{
    internal static class FtpClientDownload 
    {
        internal static void ExtractFileToTemp(
            IMenuZipService menuZipService, 
            FtpMenuContext downloadMenuContext, string pathToDownload, string pathToExtract,
            IZipMenuEvents[] zipEvents,
            DatabaseUpdatedEventContext eventContext,
            Action<Exception> failure,
            Action<string> success)
        {
            foreach (var ev in zipEvents)
            {
                ev.MenuExtracting(eventContext);
            }

            try
            {
                menuZipService.UnzipFile(downloadMenuContext.AndromedaId, pathToDownload, pathToExtract);
                downloadMenuContext.HasUnzippedMenu = true;
            }
            catch (Exception)
            {
                foreach (var ev in zipEvents)
                {
                    ev.MenuExtractionFailed(eventContext);
                }
            }

            foreach (var ev in zipEvents)
            {
                ev.MenuExtracted(eventContext);
            }
        }

        internal static FtpListItem FindMenuFolder(
            FtpClient ftpClient,
            FtpMenuContext downloadMenuContext,
            string startPath,
            IFtpEvents[] ftpEvents,
            DatabaseUpdatedEventContext eventContext,
            Action<Exception> failure,
            Action<string> success)
        {
            FtpListItem folder = null;
            foreach (var ev in ftpEvents)
            {
                ev.FtpLoading(eventContext, startPath);
            }

            try
            {
                if(!ftpClient.IsConnected)
                    ftpClient.Connect();

                var folderList = ftpClient.GetListing(startPath);

                folder = folderList.FirstOrDefault(e => e.Name == downloadMenuContext.AndromedaId.ToString());

                if (folder != null)
                {
                    downloadMenuContext.HasFoundFolder = true;
                }

                success(folder.FullName);
            }
            catch (Exception e) 
            {
                failure(e);
            }

            return folder;
        }

        internal static FtpListItem FindMenuItem(FtpClient ftpClient,
            FtpMenuContext downloadMenuContext,
            string path,
            IFtpEvents[] ftpEvents,
            DatabaseUpdatedEventContext eventContext,
            Action<Exception> failure,
            Action<string> success)
        {
            foreach (var ev in ftpEvents)
            {
                ev.FtpFilesLoading(eventContext, path);
            }

            var files = ftpClient.GetListing(path);
            var menuZip = files.FirstOrDefault(e => e.Name.StartsWith("menu", StringComparison.CurrentCultureIgnoreCase) && e.Name.EndsWith("7z"));
            var localMenuExists = File.Exists(downloadMenuContext.ExpectedMenuPath);

            if (menuZip != null)
            {
                downloadMenuContext.HasFoundFile = true;
                downloadMenuContext.FtpFileModifiedStamp = menuZip.Modified;

                if (!downloadMenuContext.LastUpdatedTimestamp.HasValue)
                {
                    return menuZip;
                }
                if (downloadMenuContext.LastUpdatedTimestamp < downloadMenuContext.FtpFileModifiedStamp || !localMenuExists)
                {
                    return menuZip;
                }
            }
            else
            {
                downloadMenuContext.HasFoundFile = false;
            }

            return null;
        }

        internal static string ReadFileToString(
            FtpClient ftpClient,
            FtpListItem item,
            Action<Exception> failure,
            Action<string> success) 
        {
            string results = string.Empty;

            try
            {
                using (var remoteStream = ftpClient.OpenRead(item.FullName))
                {
                    var sr = new StreamReader(remoteStream);
                    results = sr.ReadToEnd();
                }

                success(results);
            }
            catch (Exception e) 
            {
                failure(e);
            }

            return results;
        }

        internal static void StreamToFile(
            FtpClient ftpClient,
            FtpMenuContext downloadMenuContext,
            string path,
            FtpListItem item,
            IAccessDatabaseEvent[] events,
            DatabaseUpdatedEventContext eventContext,
            Action<Exception> failure,
            Action<string> success)
        {
            /* ********************************
             * 1. Make sure directories exist 
             * ********************************/

            var directory = Path.GetFullPath(path).Replace(Path.GetFileName(path), string.Empty);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(directory);
            }

            foreach (var ev in events)
            {
                ev.Notify(downloadMenuContext.AndromedaId, string.Format("Stream file started: {0}", item.FullName));
            }

            /* ********************** 
             * 2. Get the menu file 
             * **********************/

            try 
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    var buffer = new byte[8192];
                    var offset = 0;

                    var remoteStream = ftpClient.OpenRead(item.FullName);

                    while (offset < remoteStream.Length)
                    {
                        try
                        {
                            int len;
                            while ((len = remoteStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fs.Write(buffer, 0, len);
                                offset += len;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }

                    success(item.FullName);
                }
            }
            catch (Exception e) 
            {
                failure(e);
            }
            finally 
            {
                foreach (var ev in events)
                {
                    ev.Notify(downloadMenuContext.AndromedaId, string.Format("FileStream closed: {0}", path));
                }
            }

            downloadMenuContext.HasDownloadedMenu = true;
        }
    }
}