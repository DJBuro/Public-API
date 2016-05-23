using System.IO;
using System.Threading.Tasks;
namespace MyAndromeda.Storage.Azure
{
    public interface IBlobStorageService : IStorageService
    {
        void RenameBlobs(string from, string to);

        Task<Stream> DownloadBlob(string location);

        bool DeleteFile(string location);
    }
}