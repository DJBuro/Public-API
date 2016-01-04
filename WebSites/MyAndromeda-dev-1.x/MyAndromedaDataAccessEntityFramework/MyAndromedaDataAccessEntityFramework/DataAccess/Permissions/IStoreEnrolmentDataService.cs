using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Permissions
{
    public interface IStoreEnrolmentDataService : IDependency
    {
        void UpdateStoreEnrolment(ISite site, IEnrolmentLevel enrolment);
    }
}