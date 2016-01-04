using AndroAdmin.Dao.Domain;

namespace AndroAdmin.Dao
{
    public interface IAndroUserDao : IGenericDao<AndroUser, int>
    {
        AndroUser FindByEmailAddressPassword(string emailAddress, string password);
    }
}
