using MyAndromeda.Core;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface ICompareAccessDbVersionDataService : IDependency
    {
        LatestMenuVersion GetLatest(int andromedaId);
    }
}