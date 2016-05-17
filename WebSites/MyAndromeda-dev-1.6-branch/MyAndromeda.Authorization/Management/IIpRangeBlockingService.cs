using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core.User;
using MyAndromeda.Core;

namespace MyAndromeda.Authorization.Management
{
    public interface IIpRangeBlockingService : IDependency
    {
        /// <summary>
        /// Removes the ip restriction.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="ipAddress">The ip address.</param>
        void RemoveIpRestriction(int userId, string ipAddress);

        /// <summary>
        /// Determines whether [is the ip valid for the user] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="hostAddress">The host address.</param>
        /// <returns></returns>
        bool IsTheIpValidForTheUser(MyAndromedaUser user, string hostAddress);

        /// <summary>
        /// Lists the valid addresses.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        IEnumerable<string> ListValidAddresses(MyAndromedaUser user);
        IEnumerable<string> ListValidAddresses(int userId);

        /// <summary>
        /// Adds the IP restrictions.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="ipAddresses">The ip addresses.</param>
        void AddIPRestrictions(MyAndromedaUser user, IEnumerable<string> ipAddresses);
        void AddIPRestrictions(int userId, IEnumerable<string> ipAddresses);
    }
}
