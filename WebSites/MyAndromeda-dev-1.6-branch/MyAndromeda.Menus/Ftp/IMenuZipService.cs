using System;
using System.IO;
using System.Web.Hosting;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using SevenZip;

namespace MyAndromeda.Menus.Ftp
{
    public interface IMenuZipService : IDependency
    {
        /// <summary>
        /// Zips the file.
        /// </summary>
        /// <param name="fileToZip">The file to zip.</param>
        /// <param name="fileToZipTo">The file to zip to.</param>
        void ZipFile(string fileToZip, string fileToZipTo);

        /// <summary>
        /// Unzips the file.
        /// </summary>
        /// <param name="fileToUnZip">The file to un zip.</param>
        /// <param name="fileToUnZipTo">The file to un zip to.</param>
        void UnzipFile(int andromedaId, string fileToUnZip, string fileToUnZipTo);
    }

#pragma warning disable JustCode_CSharp_TypeFileNameMismatch // Types not matching file names
    public class MenuZipService : IMenuZipService
#pragma warning restore JustCode_CSharp_TypeFileNameMismatch // Types not matching file names
    {
        private readonly IMyAndromedaLogger logger;

        public MenuZipService(IMyAndromedaLogger logger)
        { 
            this.logger = logger;
            this.Init();
        }

        public void ZipFile(string fileToZip, string fileToZipTo)
        {
            try
            {
                if (!File.Exists(fileToZip)) 
                {
                    this.logger.Debug(format: "Menu not found {0}", args: new object[] { fileToZip });
                    return;
                }

                var zip = new SevenZipCompressor();
                
                zip.CompressionLevel = CompressionLevel.Fast;
                zip.CompressionMode = CompressionMode.Create;
                zip.EncryptHeaders = true;
                zip.DefaultItemName = fileToZipTo;

                using (var fs = new FileStream(fileToZipTo, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    zip.CompressFiles(fs, fileToZip);

                    fs.Close();
                }
            }
            catch (Exception e) 
            {
                this.logger.Error(e);
            }
        }

        public void UnzipFile(int andromedaId, string fileToUnZip, string fileToUnZipTo)
        {
            try
            {
                using (var zip = new SevenZipExtractor(fileToUnZip, "zangaboo"))
                {
                    if (!zip.Check())
                    {
                        throw new Exception(message: "Zip check failed");
                    }

                    foreach (var file in zip.ArchiveFileData)
                    {
                        if (file.FileName.IndexOf(value: "menu", comparisonType: StringComparison.InvariantCultureIgnoreCase)< 0)
                        {
                            continue;
                        }

                        using (var fileStream = new FileStream(fileToUnZipTo, FileMode.Create, FileAccess.Write, FileShare.Write, 1024 * 4, true))
                        {
                            zip.ExtractFile(file.FileName, fileStream);
                        }
                    }
                }
            }
            catch (Exception e) 
            {
                this.logger.Error(e);
            }
        }

        private void Init() 
        {
            string x64Path = Path.Combine(
                HostingEnvironment.MapPath(virtualPath: "~"), 
                path2: "App_Data", 
                path3: "7zip", 
                path4: "7z64.dll");

            string x86Path = Path.Combine(
                HostingEnvironment.MapPath(virtualPath: "~"), 
                path2: "App_Data", 
                path3: "7zip", 
                path4: "7z86.dll");

            if (IntPtr.Size == 8) //x64
            {
                if (File.Exists(x64Path))
                {
                    SevenZip.SevenZipExtractor.SetLibraryPath(x64Path);
                }
                else
                {
                    throw new IOException(string.Format(format: "dll path does not exist {0}", arg0: x64Path));
                }
            }
            else //x86 ... probably 
            {
                if (File.Exists(x86Path))
                {
                    SevenZip.SevenZipExtractor.SetLibraryPath(x86Path);
                }
                else
                {
                    throw new IOException(string.Format(format: "dll path does not exist {0}", arg0: x86Path));
                }
            }
        }
    }
}