using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Dashboard.Dao.Dao;
using Dashboard.Dao.Domain;

namespace Dashboard.Dao
{
    public interface IUserDao : IGenericDao<User, int>
    {
        User Login(string UserName, string Password, HttpResponseBase response);
    }
}
