using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Menus.Ftp.Helpers;

namespace MyAndromeda.Menus.Ftp.Helpers
{
    public enum FtpFileSystemInfoType
    {
        /// <summary>
        /// A normal file
        /// </summary>
        File,
        /// <summary>
        /// A directory
        /// </summary>
        Directory
    }

    /// <summary>
    /// The base class for FTPFileInfo and FTPDirectoryInfo
    /// </summary>
    public abstract class FtpFileSystemInfo
    {
        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName
        {
            get { return this.Uri.AbsoluteUri; }
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return Path.GetFileName(this.FullName); }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public FtpFileSystemInfoType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystemInfo"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="type">The type.</param>
        protected internal FtpFileSystemInfo(Uri uri, FtpFileSystemInfoType type)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uri.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The URI isn't a valid FTP Uri", "uri");

            this.Uri = uri;
            this.Type = type;
        }
    }

    /// <summary>
    /// Represents a file on a FTP-server
    /// </summary>
    public class FtpFileInfo : FtpFileSystemInfo
    {
        /// <summary>
        /// Gets the last write time.
        /// </summary>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public long Length { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileInfo"/> class.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        /// <param name="length">The file length.</param>
        internal FtpFileInfo(Uri path, DateTime lastWriteTime, long length)
            : base(path, FtpFileSystemInfoType.File)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            this.LastWriteTime = lastWriteTime;
            this.Length = length;
        }
    }
}
