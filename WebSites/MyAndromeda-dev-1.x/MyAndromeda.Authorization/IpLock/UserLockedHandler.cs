using MyAndromeda.Authorization.Management;
using MyAndromeda.Core.User.Events;
using MyAndromeda.Framework.Contexts;
using System;
using System.Linq;

namespace MyAndromeda.Authorization.IpLock
{
    public class UserLockedHandler : IUserEventHandler
    {
        private readonly IIpRangeBlockingService blockingService;
        private readonly ICurrentRequest currentRequest;

        public UserLockedHandler(IIpRangeBlockingService blockingService, ICurrentRequest currentRequest) 
        {
            this.blockingService = blockingService;
            this.currentRequest = currentRequest;
        }

        /// <summary>
        /// Check the login for the specified user.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        public void Login(UserEventContext userContext)
        {
            if (userContext.Cancel)
                return; // no point in checking something that has been declined elsewhere

            //get the IP 
            var hostAddress = currentRequest.Request.UserHostAddress; 

            //check if the ip is acceptable
            //true if the user does not have any ip rules
            if(this.blockingService.IsTheIpValidForTheUser(userContext.User, hostAddress))
            {
                return;
            }

            userContext.Cancel = true;
        }

        public void Creating(UserEventContext userContext)
        {

        }

        public void Created(UserEventContext userContext)
        {

        }

        public void ChangingPassword(UserEventContext userContext)
        {

        }
    }
}
