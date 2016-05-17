using MyAndromeda.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using MyAndromeda.Menus.Ftp;
using MyAndromeda.Menus.Ftp.Helpers;

namespace MyAndromeda.Menus.Ftp.Helpers
{
    public class FtpMenuFileGroup 
    {
        public FtpFileInfo Menu { get; set; }
        public FtpFileInfo Version { get; set; }
        public FtpFileInfo DateStamp { get; set; }
    }
    
    public static class FtpService
    {

        public static MyAndromedaFtpClient Create() 
        {
            return new MyAndromedaFtpClient();
        }

        public static Uri RemoteMenuUri(this MyAndromedaFtpClient client) 
        {
            var host = MyAndromeda.Configuration.MenuFtpSettings.Host;

            return new Uri(string.Format("ftp://{0}/Menus/", host));
        }

        private static Uri RemoteFtpUri(this MyAndromedaFtpClient client, int id) 
        {
            var host = MyAndromeda.Configuration.MenuFtpSettings.Host;

            return new Uri(string.Format("ftp://{0}/Menus/{1}",host, id));
        }

        private static Uri RemoteFtpMenuUri(this MyAndromedaFtpClient client, int id) 
        {
            var host = MyAndromeda.Configuration.MenuFtpSettings.Host;

            return new Uri(string.Format("ftp://{0}/Menus/{1}/menu.7z", host, id));
        }

        private static Uri RemoteTempFtpMenuUri(this MyAndromedaFtpClient client, int id) 
        {
            var host = MyAndromeda.Configuration.MenuFtpSettings.Host;

            return new Uri(string.Format("ftp://{0}/Menus/{1}/temp.7z", host, id));
        }

        public static string LocalZipDownloadPath(this MyAndromedaFtpClient client, int id)
        {
            var location = MyAndromeda.Configuration.MenuFtpSettings.LocalFolder;

            var fileLocation = string.Format(location, id)
                .Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"))
                .Replace("[File]", "menu.7z")
                .Replace("[file]", "menu.7z");

            return fileLocation;
        }

        //public static string LocalZipPath(this MyAndromedaFtpClient client, int id) 
        //{
        //    var location =  MyAndromeda.Configuration.MenuFtpSettings.LocalFolder;
            
        //    var fileLocation =  string.Format(location, id)
        //        .Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"))
        //        .Replace("[File]", "menu.7z");

        //    return fileLocation;
        //}

        public static bool HasFolder(this MyAndromedaFtpClient client, int id) 
        {
            return client.DirectoryExists(client.RemoteFtpUri(id));
        }
        public static bool HasFile(this MyAndromedaFtpClient client, int id) 
        {
            return client.FileExists(client.RemoteFtpMenuUri(id));        
        }

        public static FtpMenuFileGroup GetFiles(this MyAndromedaFtpClient client, int id) 
        {
            var files = client.GetFiles(client.RemoteFtpUri(id));

            var menuFile = files.FirstOrDefault(e => e.Name.Equals("menu.7z", StringComparison.InvariantCultureIgnoreCase));
            var menuVersionFile = files.FirstOrDefault(e => e.Name.Equals("Version.txt", StringComparison.InvariantCultureIgnoreCase));
            var dateStampFile = files.FirstOrDefault(e => e.Name.Equals("Date.txt", StringComparison.InvariantCultureIgnoreCase));

            return new FtpMenuFileGroup() 
            {
                DateStamp = dateStampFile,
                Menu = menuVersionFile,
                Version = menuVersionFile 
            };
        }

        public static void DownloadMenu(this MyAndromedaFtpClient client, int id) 
        {
            //var buffer = new byte[8192];
            //var offset = 0;
            var path = client.LocalZipDownloadPath(id);
            var uri = new Uri(path);
            var parent = new Uri(uri, "./");

            if (!Directory.Exists(parent.LocalPath)) 
            {
                Directory.CreateDirectory(parent.LocalPath);
            }

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
            { 
                using (var stream = client.OpenRead(client.RemoteFtpMenuUri(id))) 
                {
                    var buffer = new byte[32 * 1024];
                    int read;

                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs.Write(buffer, 0, read);
                    }
                    //while (offset < stream.Length)
                    //{
                    //    try
                    //    {
                    //        int len;
                    //        while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                    //        {
                    //            fs.Write(buffer, 0, len);
                    //            offset += len;
                    //        }
                    //    }
                    //    catch (Exception)
                    //    {
                    //        throw;
                    //    }
                    //}
                }
            }
        }

        public static void UploadZip(this MyAndromedaFtpClient client, int id) 
        {
            var zipPath = client.LocalZipDownloadPath(id);
            var buffer = new byte[8192];
            var offset = 0;

            using (var fs = File.OpenRead(zipPath))
            {
                using(var stream = client.OpenWrite(client.RemoteTempFtpMenuUri(id)))
                {
                    try
                    {
                        int len; 
                        while((len = fs.Read(buffer, 0, buffer.Length)) > 0) 
                        {
                            stream.Write(buffer, 0, len);
                            offset += len;
                        }
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                } 
            }
        }

        public static void RenameTempZipToFinal(this MyAndromedaFtpClient client, int id) 
        {
            var remoteTempZipPath = client.RemoteTempFtpMenuUri(id);
            var remoteZipPath = client.RemoteFtpMenuUri(id);

            client.RenameFile(remoteTempZipPath, remoteZipPath);
        }

        public static void DeleteFile(this MyAndromedaFtpClient client, int id) 
        {
            var remoteZipPath = client.RemoteFtpMenuUri(id);

            client.DeleteFile(remoteZipPath);
        }

        //public static FtpConnection CreateAndLogin() 
        //{
        //    var ftpConnection = new FtpConnection(MenuFtpSettings.Host, MenuFtpSettings.UserName, MenuFtpSettings.Password);

        //    ftpConnection.Open();
        //    ftpConnection.Login();

        //    return ftpConnection;
        //}
        
        //public static bool CheckFolderExists(this FtpConnection ftp, int id) 
        //{
        //    return ftp.DirectoryExists(id.ToString());
        //}

        //public static bool IsCurrentFolder(this FtpConnection ftp, int id) 
        //{
        //    return ftp.GetCurrentDirectory().EndsWith(id.ToString());
        //}

        //public static bool GetFileGroup(this FtpConnection ftp, int id, Action<FtpMenuFileGroup> fileGroupAction) 
        //{
        //    if (!ftp.IsCurrentFolder(id)) 
        //    {
        //        if (ftp.CheckFolderExists(id)) { 
        //            ftp.SetCurrentDirectory(id.ToString());
        //        }
        //    }

        //    var files = ftp.GetFiles();

        //    var menuFile = files.FirstOrDefault(e => e.Name.Equals("menu", StringComparison.InvariantCultureIgnoreCase) && e.Extension.Equals("7z", StringComparison.InvariantCultureIgnoreCase));
        //    var menuVersionFile = files.FirstOrDefault(e => e.Name.Equals("Version", StringComparison.InvariantCultureIgnoreCase) && e.Extension.Equals("txt"));
        //    var dateStampFile = files.FirstOrDefault(e => e.Name.Equals("Date", StringComparison.InvariantCultureIgnoreCase) && e.Extension.Equals("txt"));

        //    var valid = menuFile != null && menuVersionFile != null && dateStampFile != null; 
        //    var fileGroup = new FtpMenuFileGroup()
        //    {
        //        Menu = menuFile,
        //        Version = menuVersionFile,
        //        DateStamp = dateStampFile
        //    };

        //    if (valid && fileGroupAction != null) { fileGroupAction(fileGroup); }

        //    return valid;
        //}

        //public static void DownloadFile(this FtpConnection ftp, int id) 
        //{
        //    var downloadTo = string.Format(@"|DataDirectory|\Menus\{0}\", id);
        //    var fullPath = downloadTo.Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"));
        //    fullPath = Path.Combine(fullPath, "temp.7z");

        //    ftp.GetFile("menu.7z", fullPath, false);
        //}

        //public static void UploadFile(this FtpConnection ftp, int id) 
        //{
        //    if (!ftp.IsCurrentFolder(id)) { ftp.SetCurrentDirectory(id.ToString()); }

        //    var uploadFrom = string.Format(@"|DataDirectory|\Menus\{0}\", id);
        //    var fullPath = uploadFrom.Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"));
        //    fullPath = Path.Combine(fullPath, "menu.7z");

        //    ftp.PutFile(fullPath, "menu.7z");
        //}

        //public static DateTime? CheckDateStampUtc(int id) 
        //{
        //    DateTime? result = null;
            
        //    using (var ftp = CreateAndLogin())
        //    {
        //        if (ftp.DirectoryExists(id.ToString()))
        //        {
        //            ftp.SetCurrentDirectory(id.ToString());
        //            var files = ftp.GetFiles();

        //            var menu = files.FirstOrDefault(e => e.Name.Equals("menu", StringComparison.InvariantCultureIgnoreCase));
        //            result = menu.LastWriteTimeUtc;
        //        }
        //    }

        //    return result;
        //}


        //public static void UploadFile(int id)
        //{
        //    var downloadTo = string.Format(@"|DataDirectory|\Menus\{0}\", id);
        //    var fullPath = downloadTo.Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"));
        //    fullPath = Path.Combine(fullPath, "menu.7z");
            
        //    using (var ftp = CreateAndJoin()) 
        //    {
        //        ftp.SetCurrentDirectory(id.ToString());
        //        ftp.PutFile(fullPath, "menu.7z");
        //    }
        //}

        //public static void CheckFolder(int id)
        //{
        //    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(string.Format("ftp://www.androupdate.com/Menus/{0}", id));

        //    request.Method = WebRequestMethods.Ftp.ListDirectory;
        //    request.Credentials = new NetworkCredential(MenuFtpSettings.UserName, MenuFtpSettings.Password);
        //    request.UsePassive = true;

        //    var response = request.GetResponse() as FtpWebResponse;
        //    var stream = response.GetResponseStream();
        //    var streamReader = new StreamReader(stream, System.Text.Encoding.ASCII);
        //    var result = streamReader.ReadToEnd();

        //    var items = ListFromDirectory(result);

        //    response.Close();
        //}

        //public static bool Download(int id)
        //{
        //    var result = false;
        //    var downloadTo = string.Format(@"|DataDirectory|\Menus\{0}\", id);
        //    var fullPath = downloadTo.Replace("|DataDirectory|", HostingEnvironment.MapPath("~/app_data"));

        //    if (!Directory.Exists(fullPath))
        //        Directory.CreateDirectory(fullPath);

        //    fullPath = Path.Combine(fullPath, "menu.7z");

        //    var request = FtpWebRequest.Create(string.Format("ftp://www.androupdate.com/Menus/{0}/menu.7z", id)) as FtpWebRequest;

        //    request.Method = WebRequestMethods.Ftp.DownloadFile;
        //    request.Credentials = new NetworkCredential(MenuFtpSettings.UserName, MenuFtpSettings.Password);
        //    request.UsePassive = true;

        //    try
        //    {
        //        var response = request.GetResponse() as FtpWebResponse;
        //        var responseStream = response.GetResponseStream();

        //        var file = File.Create(fullPath);
        //        var buffer = new byte[32 * 1024];
        //        int read;

        //        while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            file.Write(buffer, 0, read);
        //        }

        //        file.Close();
        //        responseStream.Close();
        //        response.Close();
        //        result = true;
        //    }
        //    catch (WebException e)
        //    {
        //        if (e.Message == "The remote server returned an error: (550) File unavailable (e.g., file not found, no access).")
        //        {
        //            result = false;
        //        }
        //    }

        //    return result;
        //}

        //public static IEnumerable<string> ListFromDirectory(string result)
        //{
        //    string[] dataRecords = result.Split('\n');

        //    return dataRecords;
        //}
    }
}
