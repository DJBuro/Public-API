using MyAndromeda.Core;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Framework.Authorization
{
    public interface ICoreStoreEnrollments : IDependency
    {
        IEnumerable<IEnrolmentLevel> GetEnrollments();
    }

    public class CoreStoreEnrollments : ICoreStoreEnrollments
    {
        public IEnumerable<IEnrolmentLevel> GetEnrollments()
        {
            yield return new MyAndromeda.Data.Model.MyAndromeda.EnrolmentLevel()
            {
                Name = ExpectedStoreEnrollments.DefaultStoreEnrollment
            };

            yield return new MyAndromeda.Data.Model.MyAndromeda.EnrolmentLevel()
            {
                Name = ExpectedStoreEnrollments.FullEnrollment
            };

            yield return new MyAndromeda.Data.Model.MyAndromeda.EnrolmentLevel()
            {
                Name = ExpectedStoreEnrollments.RamesesStoreEnrollment
            };

            yield return new MyAndromeda.Data.Model.MyAndromeda.EnrolmentLevel()
            {
                Name = ExpectedStoreEnrollments.GprsStore
            };

        }
    }

    public interface ICoreUserRoles : IDependency 
    {
        IEnumerable<string> GetRoles();
    }

    public class CoreUserRoles : ICoreUserRoles 
    { 
        public IEnumerable<string> GetRoles()
        {
            yield return ExpectedUserRoles.Administrator;
            yield return ExpectedUserRoles.ChainAdministrator;
            yield return ExpectedUserRoles.DebugUser;
            yield return ExpectedUserRoles.Experimental;
            yield return ExpectedUserRoles.ReportingUser;
            yield return ExpectedUserRoles.StoreAdministrator;
            yield return ExpectedUserRoles.SuperAdministrator;
            yield return ExpectedUserRoles.User;
        }
    }
}
