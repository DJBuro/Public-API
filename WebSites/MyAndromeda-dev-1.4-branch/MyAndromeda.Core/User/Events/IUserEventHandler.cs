using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Core.User.Events
{
    public interface IUserEventHandler : IDependency
    {
        void Login(UserEventContext userContext);

        void Creating(UserEventContext userContext);

        void Created(UserEventContext userContext);

        void ChangingPassword(UserEventContext userContext);
    }
}
