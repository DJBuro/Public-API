namespace MyAndromeda.Menus.Services.Media
{
    //public interface IFileMediaLibraryService : IMediaLibraryService 
    //{
    //}
    //[DependencyControl(Order = 1)]
    //public class FileMediaLibraryService : IFileMediaLibraryService
    //{
    //    private readonly IActiveMenuContext activeMenuContext;
    //    private readonly IMediaResizeService resizeService;
    //    private readonly WorkContextWrapper workContextWrapper;
    //    public FileMediaLibraryService(
    //        WorkContextWrapper workContextWrapper,
    //        IActiveMenuContext activeMenuContext,
    //        IMediaResizeService resizeService)
    //    {
    //        this.workContextWrapper = workContextWrapper;
    //        this.activeMenuContext = activeMenuContext;
    //        this.resizeService = resizeService;
    //        var server = this.activeMenuContext.MediaServer;
    //        this.RemoteUrlLocation = string.Format(server.ContentPath, this.workContextWrapper.Current.CurrentSite.Site.ExternalSiteId);
    //        if(string.IsNullOrWhiteSpace(this.activeMenuContext.MediaServer.StoragePath))
    //        {
    //            this.activeMenuContext.MediaServer.StoragePath = "~/content/stores/menu/{0}/";
    //        }
    //        var localStorage = string.Format(
    //            this.activeMenuContext.MediaServer.StoragePath , this.workContextWrapper.Current.CurrentSite.Site.ExternalSiteId);
    //        this.LocalStorageLocation = localStorage.StartsWith("~/") ?
    //                workContextWrapper.HttpContext.Server.MapPath(localStorage) :
    //                localStorage;
    //    }
    //    public string LocalStorageLocation { get; set; }
    //    public string RemoteUrlLocation { get; set; }
    //    public string Name
    //    {
    //        get
    //        {
    //            return "Local";
    //        }
    //    }
    //    public void RemoveMedia(string folderPath, string fileName)
    //    {
    //        var storage = this.LocalStorageLocation;
    //        var destination = Path.Combine(storage, folderPath);
    //        var physicalPath = Path.Combine(destination, fileName);
    //        File.Delete(physicalPath);
    //    }
    //    public IEnumerable<FileResult> ImportMedia(MemoryStream postFile, string folderPath, string fileName, int andromedaSiteId)
    //    {
    //        var storage = this.LocalStorageLocation;
    //        var destination = Path.Combine(storage, folderPath);
    //        if (!Directory.Exists(destination))
    //        {
    //            Directory.CreateDirectory(destination);
    //        }
    //        foreach (var profile in this.resizeService.ResizeImage(andromedaSiteId, postFile))
    //        {
    //            var result = ProvideResult(profile, folderPath, fileName, destination);
    //            yield return result;
    //        }
    //        yield break;
    //    }
    //    private FileResult ProvideResult(Models.ResizeSizeTaskContext profile, string relativeFolderPath, string fileName, string destination)
    //    {
    //        var extension = Path.GetExtension(fileName);
    //        var name = Path.GetFileName(fileName).Replace(extension, string.Empty);
    //        //include profile name
    //        var newFileName = string.Format("{0}{1}", name, profile.Name);
    //        //extension already begins with a dot
    //        var fullpath = string.Format("{0}/{1}{2}", destination, newFileName, ".png");
    //        //extension);
    //        using (var fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
    //        {
    //            profile.Result.WriteTo(fileStream);
    //        }
    //        var complexName = newFileName + ".png";
    //        var remoteLocation = this.RemoteUrlLocation.EndsWith(@"/") ?
    //            this.RemoteUrlLocation :
    //            this.RemoteUrlLocation + @"/";
    //        var remoteFullPath = string.Format("{0}{1}/{2}", remoteLocation, relativeFolderPath, complexName);
    //        return new FileResult(complexName, remoteFullPath, profile.Height.ToString(), profile.Width.ToString());
    //    }
    //}
}