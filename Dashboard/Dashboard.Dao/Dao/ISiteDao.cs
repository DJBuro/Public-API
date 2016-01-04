using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Dashboard.Dao.Dao;
using Dashboard.Dao.Domain;

namespace Dashboard.Dao
{
    public interface ISiteDao : IGenericDao<Site, int>
    {
        Site FindByIP(string IPAddress);
        void Login(Site site, HttpResponseBase response);
    }
}
