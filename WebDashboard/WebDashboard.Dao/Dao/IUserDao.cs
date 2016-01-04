using System.Collections.Generic;
using WebDashboard.Dao.Domain;

namespace WebDashboard.Dao
{
    public interface IUserDao : IGenericDao<User, int>
    {
        User FindByEmailAndPassword(string emailAddress, string password);
        IList<User> FindAllByHeadOffice(HeadOffice headOffice);
        User CheckEmailExists(string emailAddress);
    }
}
