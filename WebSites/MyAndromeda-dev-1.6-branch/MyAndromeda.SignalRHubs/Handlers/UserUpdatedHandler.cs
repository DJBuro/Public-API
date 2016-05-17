using MyAndromeda.Core.User.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.SignalRHubs.Handlers
{
    public class UserUpdatedHandler : IUserUpdatedEvent
    {

        public void UserCreating(UserEditContext userContext)
        {
            throw new NotImplementedException();
        }

        public void UserCreated(UserEditContext userContext)
        {
            throw new NotImplementedException();
        }

        public void UserModifying(UserEditContext userContext)
        {
        }

        public void UserModified(UserEditContext userContext)
        {
        }

        public void UserPasswordChanging(UserEditContext userContext)
        {
            throw new NotImplementedException();
        }

        public void UserPasswordChanged(UserEditContext userContext)
        {
            throw new NotImplementedException();
        }

        public void UserChainAccessChanged(UserEditContext context)
        {
            throw new NotImplementedException();
        }

        public void UserStoreAccessChanged(UserEditContext context)
        {
            throw new NotImplementedException();
        }
    }
}
