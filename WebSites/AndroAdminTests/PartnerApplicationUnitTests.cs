using System;
using AndroAdminDataAccess.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AndroAdminTests
{
    [TestClass]
    public class PartnerApplicationUnitTests
    {
        [TestMethod]
        public void AddApplicationSite()
        {
            EntityFrameworkDataAccessFactory entityFrameworkDataAccessFactory = new EntityFrameworkDataAccessFactory();
            entityFrameworkDataAccessFactory.ACSApplicationDAO.AddStore(430, 2);
        }
    }
}
