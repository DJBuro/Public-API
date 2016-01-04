namespace MyAndromeda.Storage.Azure
{
    public interface IBlobStorageService : IStorageService
    {
        void RenameBlobs(string from, string to);
        bool DeleteFile(string location);
    }
}