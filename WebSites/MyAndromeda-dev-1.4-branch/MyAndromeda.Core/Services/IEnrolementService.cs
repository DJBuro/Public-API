using MyAndromeda.Core.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Core.Services
{
    public interface IEnrolmentService : IDependency
    {
        void Delete(IEnrolmentLevel enrollment);

        /// <summary>
        /// Gets the enrolment levels.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IEnrolmentLevel> ListEnrolmentLevels();

        /// <summary>
        /// Gets the enrolment level.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IEnrolmentLevel GetEnrolmentLevel(string name);

        /// <summary>
        /// Gets the enrolment levels.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        IEnumerable<IEnrolmentLevel> GetEnrolmentLevels(ISite site);

        /// <summary>
        /// Updates the sites enrolment.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="enrolmentLevel">The enrolment level.</param>
        void UpdateSitesEnrolment(ISite site, IEnrolmentLevel enrolmentLevel);

        /// <summary>
        /// Creates or updates a enrolment.
        /// </summary>
        /// <param name="enrolmentLevel">The enrolment level.</param>
        void CreateORUpdate(IEnrolmentLevel enrolmentLevel);
    }
}
