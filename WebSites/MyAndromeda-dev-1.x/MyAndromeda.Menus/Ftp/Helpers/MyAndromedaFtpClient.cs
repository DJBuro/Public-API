using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Configuration;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using MyAndromeda.Menus.Ftp.Helpers;

namespace MyAndromeda.Menus.Ftp.Helpers
{
    public class MyAndromedaFtpClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        /// <value>
        /// The credentials.
        /// </value>
        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpClient"/> class.
        /// </summary>
        /// <param name="credentials">The login credentials.</param>
        public MyAndromedaFtpClient(NetworkCredential credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(paramName: "credentials");

            this.Credentials = credentials;
            this.UsePassive = MenuFtpSettings.TransferMode.Equals(value: "Passive", comparisonType: StringComparison.InvariantCultureIgnoreCase);
        }

        public MyAndromedaFtpClient(): this(new NetworkCredential(MenuFtpSettings.UserName, MenuFtpSettings.Password))
        {
            
        }

        public bool UsePassive { get; set; }

        public void Dispose() { }

        /// <summary>
        /// Gets the directories that are contained in the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// An enumeration of <see cref="FlagFtp.FtpDirectoryInfo"/>.
        /// </returns>
        public IEnumerable<FtpDirectoryInfo> GetDirectories(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException(paramName: "directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The directory isn't a valid FTP URI", paramName: "directory");

            directory = this.NormalizeUri(directory);

            return this.GetFileSystemInfos(directory, FtpFileSystemInfoType.Directory)
                .Cast<FtpDirectoryInfo>();
        }

        /// <summary>
        /// Gets the files that are contained in the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// An enumeration of <see cref="FlagFtp.FtpFileInfo"/>.
        /// </returns>
        public IEnumerable<FtpFileInfo> GetFiles(Uri directory)
        {
            IEnumerable<FtpFileInfo> result = Enumerable.Empty<FtpFileInfo>();

            if (directory == null)
                throw new ArgumentNullException(paramName: "directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The directory isn't a valid FTP URI", paramName: "directory");

            directory = this.NormalizeUri(directory);

            try
            {
                result = this.GetFileSystemInfos(directory, FtpFileSystemInfoType.File)
                    .Cast<FtpFileInfo>();
            }
            catch (WebException e) 
            {
                if (!e.Message.Contains(value: "550")) {
                    throw e;
                }
                
            }
            
            return result;
        }

        /// <summary>
        /// Opens the specified file for read access.
        /// </summary>
        /// <param name="file">The file to open.</param>
        /// <returns>
        /// An FTP stream to read from the file.
        /// </returns>
        public Stream OpenRead(FtpFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(paramName: "file");

            //var stream = this.CreateClient().OpenRead(file.Uri);
            //return new FtpStream(stream, file.Length);

            return this.OpenRead(file.Uri);
        }

        

        /// <summary>
        /// Opens the specified file for read access.
        /// </summary>
        /// <param name="file">The URI of the file to open.</param>
        /// <returns>
        /// A <see cref="FlagFtp.FtpStream"/> to read from the file.
        /// </returns>
        public Stream OpenRead(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException(paramName: "file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "file");

            file = this.NormalizeUri(file);

            long fileSize = this.GetFileSize(file);

            Stream result = null;

            try {

                var response = this.CreateResponse(file, WebRequestMethods.Ftp.DownloadFile);
                var responseStream = response.GetResponseStream();

                result = responseStream;
            }
            catch (Exception) 
            {
                throw;
            }


            return result;

            //return new FtpStream(this.CreateClient().OpenRead(file), fileSize);
        }

        /// <summary>
        /// Opens the specified file for write access.
        /// </summary>
        /// <param name="file">The file to open.</param>
        /// <returns>
        /// A <see cref="System.IO.Stream"/> to write to the file.
        /// </returns>
        public Stream OpenWrite(FtpFileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(paramName: "file");
            }

            return this.OpenWrite(file.Uri);
        }

        /// <summary>
        /// Opens the specified file for write access.
        /// </summary>
        /// <param name="file">The URI of the file to open.</param>
        /// <returns>
        /// A <see cref="System.IO.Stream"/> to write to the file.
        /// </returns>
        public Stream OpenWrite(Uri file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(paramName: "file");
            }

            if (file.Scheme != Uri.UriSchemeFtp)
            {
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "file");
            }

            file = this.NormalizeUri(file);

            return 
                this
                    .CreateRequest(file, WebRequestMethods.Ftp.UploadFile)
                    .GetRequestStream();
        }

        /// <summary>
        /// Deletes the specified FTP file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        public void DeleteFile(FtpFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(paramName: "file");

            this.DeleteFile(file.Uri);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The URI of the file to delete.</param>
        public void DeleteFile(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException(paramName: "file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "file");

            file = this.NormalizeUri(file);

            using (this.CreateResponse(file, WebRequestMethods.Ftp.DeleteFile))
            { }
        }

        public void RenameFile(Uri file, Uri renameTo) 
        {
            if (file == null)
                throw new ArgumentNullException(paramName: "file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "file");

            if (renameTo == null)
                throw new ArgumentNullException(paramName: "renameTo");

            if (renameTo.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "renameTo");

            using (this.CreateRenameRequest(file, renameTo, WebRequestMethods.Ftp.Rename)) 
            { }
        }

        /// <summary>
        /// Creates the specified directory on the FTP server.
        /// </summary>
        /// <param name="directory">The URI of the directory to create.</param>
        public void CreateDirectory(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException(paramName: "directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The directory isn't a valid FTP URI", paramName: "directory");

            directory = this.NormalizeUri(directory);

            using (this.CreateResponse(directory, WebRequestMethods.Ftp.MakeDirectory))
            { }
        }

        /// <summary>
        /// Deletes the specified FTP directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public void DeleteDirectory(FtpDirectoryInfo directory)
        {
            if (directory == null)
                throw new ArgumentNullException(paramName: "directory");

            this.DeleteDirectory(directory.Uri);
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The URI of the directory to delete.</param>
        public void DeleteDirectory(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException(paramName: "directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The directory isn't a valid FTP URI", paramName: "directory");

            directory = this.NormalizeUri(directory);

            using (this.CreateResponse(directory, WebRequestMethods.Ftp.RemoveDirectory))
            { }
        }

        /// <summary>
        /// Gets the <see cref="FlagFtp.FtpFileInfo"/> for the specified URI.
        /// </summary>
        /// <param name="file">The URI of the file.</param>
        /// <returns>
        /// A <see cref="FlagFtp.FtpFileInfo"/> that contains information about the file.
        /// </returns>
        public FtpFileInfo GetFileInfo(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException(paramName: "file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "file");

            file = this.NormalizeUri(file);

            DateTime lastWriteTime = this.GetTimeStamp(file);
            long length = this.GetFileSize(file);

            return new FtpFileInfo(file, lastWriteTime, length);
        }

        /// <summary>
        /// Gets the <see cref="FlagFtp.FtpDirectoryInfo"/> for the specified URI.
        /// </summary>
        /// <param name="directory">The URI of the directory.</param>
        /// <returns>
        /// A <see cref="FlagFtp.FtpDirectoryInfo"/> that contains information about the directory.
        /// </returns>
        public FtpDirectoryInfo GetDirectoryInfo(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException(paramName: "directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The directory isn't a valid FTP URI", paramName: "directory");

            directory = this.NormalizeUri(directory);

            return new FtpDirectoryInfo(directory);
        }

        /// <summary>
        /// Determines if the specified file exists on the FTP server.
        /// </summary>
        /// <param name="file">The URI of the file.</param>
        /// <returns>
        /// True, if the file exists; otherwise false.
        /// </returns>
        public bool FileExists(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException(paramName: "file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "file");

            file = this.NormalizeUri(file);

            var folder = new Uri(file, relativeUri: "./");
            IEnumerable<FtpFileInfo> files = this.GetFiles(folder);

            IEnumerable<string> uris = files.Select(e => this.NormalizeUri(e.Uri).AbsoluteUri);

            return
                uris.Any(f => file.AbsoluteUri.Equals(f));
                //files.Any(f => this.NormalizeUri(f.Uri).AbsoluteUri == file.AbsoluteUri);
        }

        /// <summary>
        /// Determines if the specified directory exists on the FTP server.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// True, if the directory exists; otherwise false.
        /// </returns>
        public bool DirectoryExists(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException(paramName: "directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The directory isn't a valid FTP URI", paramName: "directory");

            directory = this.NormalizeUri(directory);

            IEnumerable<FtpDirectoryInfo> directories = this.GetDirectories(new Uri(directory, relativeUri: "./"));

            return directories.Any(dir => this.NormalizeUri(dir.Uri).AbsoluteUri == directory.AbsoluteUri);
        }

        /// <summary>
        /// Gets the files or directories from the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// An enumeration of <see cref="FlagFtp.FtpFileSystemInfo"/> with the type of the specified type argument.
        /// </returns>
        private IEnumerable<FtpFileSystemInfo> GetFileSystemInfos(Uri directory, FtpFileSystemInfoType type)
        {
            if (directory == null)
                throw new ArgumentNullException(paramName: "directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The directory isn't a valid FTP URI", paramName: "directory");

            using (var response = (this.CreateResponse(directory, WebRequestMethods.Ftp.ListDirectoryDetails)))
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        string all = reader.ReadToEnd();

                        FtpFileSystemInfo[] infos = GetFileSystemInfosFromResponseFirst(all, directory).ToArray();

                        if (infos.Length == 0) 
                        {
                            infos = GetFileSystemInfosFromDos(all, directory).ToArray();
                        }

                        

                        switch (type)
                        {
                            case FtpFileSystemInfoType.Directory:
                                return infos.Where(info => info.Type == FtpFileSystemInfoType.Directory)
                                    .Cast<FtpFileSystemInfo>();

                            case FtpFileSystemInfoType.File:
                                return infos.Where(info => info.Type == FtpFileSystemInfoType.File)
                                    .Cast<FtpFileSystemInfo>();
                        }
                    }
                }
            }

            throw new InvalidOperationException(message: "Method should not reach this code!");
        }


        public IEnumerable<FtpFileSystemInfo> GetFileSystemInfosFromResponseFirst(string block, Uri directory) 
        {
            var regex = new Regex(pattern: @"^(?<FileOrDirectory>[d-])(?<Attributes>[rwxts-]{3}){3}\s+\d{1,}\s+.*?(?<FileSize>\d{1,})\s+(?<Date>\w+\s+\d{1,2}\s+(?:\d{4})?)(?<YearOrTime>\d{1,2}:\d{2})?\s+(?<Name>.+?)\s?$", options: RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            MatchCollection matches = regex.Matches(block);

            var infos = matches.Cast<Match>()
                            .Select(
                                match =>
                                new
                                {
                                    Date = match.Groups["Date"].Value,
                                    IsDirectory = match.Groups["FileOrDirectory"].Value == "d",
                                    FileLength = long.Parse(match.Groups["FileSize"].Value),
                                    Name = match.Groups["Name"].Value,
                                    FullName = new Uri(new Uri(directory + "/"), match.Groups["Name"].Value)
                                })
                            .Where(info => info.Name != "." && info.Name != "..");

            return infos
                .Select(info =>
                    info.IsDirectory ? new FtpDirectoryInfo(this.NormalizeUri(info.FullName)) as FtpFileSystemInfo:
                                       new FtpFileInfo(this.NormalizeUri(info.FullName), DateTime.Parse(info.Date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal), info.FileLength) as FtpFileSystemInfo
                );
        }

        public IEnumerable<FtpFileSystemInfo> GetFileSystemInfosFromDos(string block, Uri directory)
        {
            var regex = new Regex(
                pattern: @"(?<Date>\d+-\d+-\d+\s+\d+:\d+\w+)\s+((?<FileSize>\d*)\s+(?<Name>.*)\s?$ | (?<Directory><DIR>)\s+(?<Name>.*)\s?$)", 
                options:
                        //var regex = new Regex(@"(?<Date>\d+-\d+-\d+\s+\d+:\d+\w+)\s+(?<FileSize>\d*)\s+(?<Name>.*)\s?$",
                        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            MatchCollection matches = regex.Matches(block);

            var infos = matches.Cast<Match>()
                            .Select(
                                match =>
                                new
                                {
                                    IsDirectory = match.Groups["Directory"].Success,
                                    Date = match.Groups["Date"].Value,
                                    FileLength = match.Groups["FileSize"].Success ? long.Parse(match.Groups["FileSize"].Value) : 0,
                                    Name = match.Groups["Name"].Value,
                                    FullName = new Uri(new Uri(directory + "/"), match.Groups["Name"].Value)
                                })
                            .Where(info => info.Name != "." && info.Name != "..");

            return infos
                .Select(info =>
                    info.IsDirectory ? new FtpDirectoryInfo(this.NormalizeUri(info.FullName)) as FtpFileSystemInfo:
                                       new FtpFileInfo(this.NormalizeUri(info.FullName), DateTime.Parse(info.Date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal), info.FileLength) as FtpFileSystemInfo
                );
        }

        /// <summary>
        /// Gets the time stamp for the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// A <see cref="System.DateTime"/> object the represents the last write time of the file.
        /// </returns>
        private DateTime GetTimeStamp(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException(paramName: "file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "file");

            file = this.NormalizeUri(file);

            using (var response = this.CreateResponse(file, WebRequestMethods.Ftp.GetDateTimestamp))
            {
                return response.LastModified;
            }
        }

        /// <summary>
        /// Gets the size of the specified file in bytes.
        /// </summary>
        /// <param name="file">The file URI.</param>
        /// <returns>
        /// The file size in bytes.
        /// </returns>
        private long GetFileSize(Uri file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(paramName: "file");
            }
            if (file.Scheme != Uri.UriSchemeFtp)
            {
                throw new ArgumentException(message: "The file isn't a valid FTP URI", paramName: "file");
            }

            file = this.NormalizeUri(file);

            using (var response = this.CreateResponse(file, WebRequestMethods.Ftp.GetFileSize))
            {
                return response.ContentLength;
            }
        }

        /// <summary>
        /// Creates a <see cref="System.Net.FtpWebResponse"/> from the specified request URI, request method and the necessary credentials.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestMethod">The request method.</param>
        /// <returns>
        /// A <see cref="System.Net.FtpWebRequest"/> with the specified request uri, request method and the necessary credentials.
        /// </returns>
        private FtpWebResponse CreateResponse(Uri requestUri, string requestMethod)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException(paramName: "requestUri");
            }
            if (requestUri.Scheme != Uri.UriSchemeFtp)
            {
                throw new ArgumentException(message: "The request URI isn't a valid FTP URI", paramName: "requestUri");
            }

            requestUri = this.NormalizeUri(requestUri);

            WebResponse request = this.CreateRequest(requestUri, requestMethod).GetResponse();
            var response = request as FtpWebResponse;

            return response;
        }

        private FtpWebResponse CreateRenameRequest(Uri requestUri, Uri destinationUri, string requestMethod) 
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException(paramName: "requestUri");
            }
            if (requestUri.Scheme != Uri.UriSchemeFtp)
            {
                throw new ArgumentException(message: "The request URI isn't a valid FTP URI", paramName: "requestUri");
            }

            requestUri = this.NormalizeUri(requestUri);
            destinationUri = this.NormalizeUri(destinationUri);

            var request = WebRequest.Create(requestUri) as FtpWebRequest;
            
            request.Method = requestMethod;
            request.Credentials = this.Credentials;
            request.UsePassive = this.UsePassive;
            request.UseBinary = true;
            request.RenameTo = destinationUri.LocalPath;

            var response = request.GetResponse() as FtpWebResponse;

            return response;
        }

        /// <summary>
        /// Creates a <see cref="System.Net.FtpWebRequest"/> from the specified request URI, request method and the necessary credentials.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestMethod">The request method.</param>
        /// <returns>
        /// A <see cref="System.Net.FtpWebRequest"/> with the specified request uri, request method and the necessary credentials.
        /// </returns>
        private FtpWebRequest CreateRequest(Uri requestUri, string requestMethod)
        {
            if (requestUri == null)
                throw new ArgumentNullException(paramName: "requestUri");

            if (requestUri.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The request URI isn't a valid FTP URI", paramName: "requestUri");

            requestUri = this.NormalizeUri(requestUri);

            var request = (FtpWebRequest)WebRequest.Create(requestUri);

            request.Method = requestMethod;
            request.Credentials = this.Credentials;
            request.UsePassive = this.UsePassive;
            request.UseBinary = true;

            return request;
        }

        /// <summary>
        /// Creates a <see cref="System.Net.WebClient"/> with the necessary credentials.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Net.WebClient"/> with the necessary credentials
        /// </returns>
        //private WebClient CreateClient()
        //{
        //    var client = new WebClient { Credentials = this.Credentials,  };

        //    return client;
        //}

        /// <summary>
        /// Normalizes the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A normalized URI</returns>
        private Uri NormalizeUri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(paramName: "Uri");

            if (uri.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException(message: "The URI isn't a valid FTP URI", paramName: "uri");

            string path = uri.AbsoluteUri;

            //Cut the "ftp://" off
            path = path.Substring(6);
            path = path.Replace(oldValue: "//", newValue: "/")
                .Replace(oldValue: @"\\", newValue: "/")
                .Replace(oldValue: @"\", newValue: "/");

            return new Uri("ftp://" + path);
        }
    }
}
