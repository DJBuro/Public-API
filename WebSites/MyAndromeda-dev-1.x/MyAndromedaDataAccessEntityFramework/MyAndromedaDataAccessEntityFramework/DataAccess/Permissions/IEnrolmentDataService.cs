using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Permissions
{
    public interface IEnrolmentDataService : IDependency 
    {
        IEnumerable<IEnrolmentLevel> List();

        IEnumerable<IEnrolmentLevel> Query(Expression<Func<MyAndromeda.Data.Model.MyAndromeda.EnrolmentLevel, bool>> query);

        IEnrolmentLevel Create(string name, string description);

        void Update(IEnrolmentLevel model);

        void Delete(IEnrolmentLevel enrollment);
    }
}