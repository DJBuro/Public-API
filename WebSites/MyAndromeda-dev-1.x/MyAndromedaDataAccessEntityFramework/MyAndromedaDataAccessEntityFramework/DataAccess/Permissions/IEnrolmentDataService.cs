using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Permissions
{
    public interface IEnrolmentDataService : IDependency 
    {
        IEnumerable<IEnrolmentLevel> List();

        IEnumerable<IEnrolmentLevel> Query(Expression<Func<EnrolmentLevel, bool>> query);

        IEnrolmentLevel Create(string name, string description);

        void Update(IEnrolmentLevel model);

        void Delete(IEnrolmentLevel enrollment);
    }
}