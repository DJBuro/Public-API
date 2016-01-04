using LukeSkywalker.IPNetwork;
using MyAndromeda.Core.User;
using MyAndromeda.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;

namespace MyAndromeda.Authorization.Management
{
    public class IpRangeBlockingService : IIpRangeBlockingService 
    {
        private readonly IUserLockingDataService lockService;

        private readonly IMyAndromedaLogger logger;

        public IpRangeBlockingService(IUserLockingDataService lockService, IMyAndromedaLogger logger) 
        {
            this.lockService = lockService;
            this.logger = logger;
        }

        public void RemoveIpRestriction(int userId, string ipAddress)
        {
            var locks = this.lockService.GetLockByUserId(userId);

            if (locks == null) return;

            var userIpDefintions = locks.ValidIpV4Ranges.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
 
            if(userIpDefintions.Any(e=> e.Equals(ipAddress)))
            {
                this.AddIPRestrictions(userId, userIpDefintions.Where(e => e != ipAddress));
            }
        }

        public bool IsTheIpValidForTheUser(MyAndromedaUser user, string hostAddress)
        {
            var locks = this.lockService.GetLockByUserId(user.Id);

            if ( locks == null)
                return true;

            if (!locks.Enabled)
                return true;

            if (string.IsNullOrWhiteSpace(locks.ValidIpV4Ranges)) 
                return true;
            
            var userIpDefintions = locks.ValidIpV4Ranges.Split(new []{ '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (userIpDefintions.Length == 0) 
                return true; 

            List<IPNetwork> networks = new List<IPNetwork>();
            foreach (var definition in userIpDefintions) 
            {
                IPNetwork ipnetwork = IPNetwork.Parse(definition);
                networks.Add(ipnetwork);
            }

            var checkWith = IPNetwork.Parse(hostAddress);
            var validAddress = networks.Any(e => IPNetwork.Contains(e, checkWith));

            if (!validAddress) 
            {
                this.logger.Debug("The network address ({0}) is not allowed for user: {1}", hostAddress, user.Username);
                this.logger.Debug("Valid IP's for the user {0} are: {1}", user.Username, locks.ValidIpV4Ranges);
            }
              
            return validAddress;
        }

        public IEnumerable<string> ListValidAddresses(MyAndromedaUser user)
        {
            return this.ListValidAddresses(user.Id);
        }

        public IEnumerable<string> ListValidAddresses(int userId)
        {
            return this.lockService.GetLockByUserId(userId).ValidIpV4Ranges.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void AddIPRestrictions(MyAndromedaUser user, IEnumerable<string> ipAddresses)
        {
            this.AddIPRestrictions(user.Id, ipAddresses);
        }

        public void AddIPRestrictions(int userId, IEnumerable<string> ipAddresses)
        {
            var ipRanges = string.Join("|", ipAddresses);
            this.lockService.CreateOrUpdateRestrictionsByUserId(userId, ipRanges); 
        }
    }
}