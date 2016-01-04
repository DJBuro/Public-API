using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromedaDataAccessEntityFramework.DataAccess
{
    public interface IAcsApplicationDataService : IDependency 
    {
        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <returns></returns>
        IQueryable<ACSApplication> Query();

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        ACSApplication Get(int id);

        /// <summary>
        /// Updates the specified acs application.
        /// </summary>
        /// <param name="acsApplication">The acs application.</param>
        void Update(ACSApplication acsApplication);
    }
}