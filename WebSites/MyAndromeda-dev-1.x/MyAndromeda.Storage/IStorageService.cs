using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using System.IO;

namespace MyAndromeda.Storage
{
    public interface IStorageService: IDependency
    {
        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="stream">The stream.</param>
        void WriteFile(string location, string fileName, Stream stream);

        /// <summary>
        /// Creates the directory async.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        void CreateDirectory(string location, string name);

        List<Models.FileModel> List(string location);

        /// <summary>
        /// The remote location of the file.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        string RemoteLocation(string host);

        /// <summary>
        /// Get the content path.
        /// </summary>
        /// <param name="hostAddress">The host address.</param>
        /// <param name="contentPathFormat">The content path format.</param>
        /// <param name="externalSiteId">The external site id.</param>
        /// <returns></returns>
        string ContentPath(string hostAddress, string contentPathFormat, string externalSiteId);


    }
}
