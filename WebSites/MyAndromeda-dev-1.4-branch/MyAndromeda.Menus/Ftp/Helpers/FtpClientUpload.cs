using System;
using System.IO;
using System.Net.FtpClient;
using MyAndromeda.Menus.Context.Ftp;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Ftp;

namespace MyAndromeda.Menus.Ftp.Helpers
{
    internal static class FtpClientUpload 
    {
        internal static void UploadPublishDateFile(FtpClient ftpClient, FtpMenuContext downloadMenuContext, DateTime publishDate, Action<string> notify, Action<Exception> failure, Action<string> completedPath)
        {
            string dateToPublishFilePathTemp = string.Format(MenuFtpService.FtpPathFormat, downloadMenuContext.AndromedaId, "Date.txt.temp");
            string dateToPublishFilePath = string.Format(MenuFtpService.FtpPathFormat, downloadMenuContext.AndromedaId, "Date.txt");

            try
            {
                using (var dateModifiedWriteStream = ftpClient.OpenWrite(dateToPublishFilePathTemp, FtpDataType.Binary))
                {
                    using (MemoryStream txtMemoryStream = new MemoryStream())
                    {
                        using (TextWriter tw = new StreamWriter(txtMemoryStream))
                        {
                            //todo change to when the menu gets published. 
                            notify("write line: " + publishDate.ToString("yyyy-MM-dd"));
                            tw.WriteLine(publishDate.ToString("yyyy-MM-dd"));
                            tw.Flush();
                            txtMemoryStream.Seek(0, SeekOrigin.Begin);
                            txtMemoryStream.CopyTo(dateModifiedWriteStream);
                        }
                    }
                }

                notify("Check if date file exists");
                if (ftpClient.FileExists(dateToPublishFilePath))
                {
                    notify("delete file");
                    ftpClient.DeleteFile(dateToPublishFilePath);
                }
                else 
                {
                    notify("no date file to delete: " + dateToPublishFilePath);
                }

                ftpClient.Rename(dateToPublishFilePathTemp, dateToPublishFilePath);
                
                notify("renamed!");

                completedPath(dateToPublishFilePath);
            }
            catch (Exception e)
            {
                failure(e);
            }
        }

        internal static void UploadVersionFile(FtpClient ftpClient, FtpMenuContext context, string version, Action<string> notify, Action<Exception> failure, Action<string> completedPath)
        {
            string versionFilePathTemp = string.Format(MenuFtpService.FtpPathFormat, context.AndromedaId, "Version.txt.temp");
            string versionFilePath = string.Format(MenuFtpService.FtpPathFormat, context.AndromedaId, "Version.txt");

            try
            {
                using (var menuVersionWriteStream = ftpClient.OpenWrite(versionFilePathTemp, FtpDataType.Binary))
                {
                    using (MemoryStream txtMemoryStream = new MemoryStream())
                    {
                        using (TextWriter tw = new StreamWriter(txtMemoryStream))
                        {
                            notify("write line: " + version);
                            tw.WriteLine(version);
                            tw.Flush();
                            txtMemoryStream.Seek(0, SeekOrigin.Begin);
                            txtMemoryStream.CopyTo(menuVersionWriteStream);
                        }
                    }
                }
                
                notify("Check if file exists");
                if (ftpClient.FileExists(versionFilePath))
                {
                    notify("Delete the file: "+ versionFilePath);
                    ftpClient.DeleteFile(versionFilePath);
                }
                else
                {
                    notify("no file to delete: " + versionFilePath);
                }

                string notifyRename = string.Format("renaming version temp file ({0}) to: {1}", versionFilePathTemp, versionFilePath);
                notify(notifyRename);
                ftpClient.Rename(versionFilePathTemp, versionFilePath);
                notify("renamed!");

                completedPath(versionFilePath);
            }
            catch (Exception e)
            {
                failure(e);
            }
        }

        internal static void UploadMenuFile(FtpClient ftpClient, FtpMenuContext context, Action<string> notify, Action<Exception> failure, Action<string> completedPath)
        {
            try
            {
                var zipFileLocalPath = context.ExpectedZipPath;

                string tempPath = string.Format(MenuFtpService.FtpPathFormat, context.AndromedaId, "menu.7z.temp");
                string menuPath = string.Format(MenuFtpService.FtpPathFormat, context.AndromedaId, "menu.7z");
                //upload

                using (var menuWriteStream = ftpClient.OpenWrite(tempPath, FtpDataType.Binary))
                {
                    using (var fsZipFile = new FileStream(zipFileLocalPath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        notify("copy file system zip file into ftp menu stream.");
                        fsZipFile.CopyTo(menuWriteStream);
                        fsZipFile.Close();
                        notify("copy fsZipFileIntoFtpStream");
                    }
                }

                notify("Check if file exists");
                
                //rename
                if (ftpClient.FileExists(menuPath))
                {
                    notify("delete file");
                    ftpClient.DeleteFile(menuPath);
                }
                else 
                {
                    notify("no file to delete: " + menuPath);
                }

                string notifyRename = string.Format("renaming menu temp file ({0}) to: {1}", tempPath, menuPath);
                notify(notifyRename);

                ftpClient.Rename(tempPath, menuPath);

                notify("renamed!");

                completedPath(menuPath);
            }
            catch (Exception e)
            {
                failure(e);
            }
        }

        internal static void ZipProcess(FtpMenuContext downloadMenuContext, IMenuZipService menuZipService, IZipMenuEvents[] zipEvents, DatabaseUpdatedEventContext context)
        {
            foreach (var ev in zipEvents)
            {
                ev.MenuZipping(context);
            }

            menuZipService.ZipFile(downloadMenuContext.ExpectedMenuPath, downloadMenuContext.ExpectedZipPath);

            foreach (var ev in zipEvents)
            {
                ev.MenuZipped(context);
            }
        }
    }
}