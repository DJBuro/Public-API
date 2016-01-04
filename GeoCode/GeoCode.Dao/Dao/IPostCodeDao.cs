using GeoCode.Dao.Domain;

namespace GeoCode.Dao
{
    public interface IPostCodeDao : IGenericDao<PostCode, int>
    {
        PostCode Find(string postCode);
    }
}
