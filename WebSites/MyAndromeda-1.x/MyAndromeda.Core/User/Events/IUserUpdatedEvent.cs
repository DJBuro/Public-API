using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Core.User.Events
{
    public interface IUserUpdatedEvent : IDependency
    {
        void UserCreating(UserEditContext userContext);
        void UserCreated(UserEditContext userContext);
        void UserModifying(UserEditContext userContext);
        void UserModified(UserEditContext userContext);
        void UserPasswordChanging(UserEditContext userContext);
        void UserPasswordChanged(UserEditContext userContext);
        void UserChainAccessChanged(UserEditContext context);
        void UserStoreAccessChanged(UserEditContext context);
    }

    public class UserEditContext
    {
        public UserEditContext(MyAndromedaUser user, bool isCancel)
        {
            this.User = user;
            this.Cancel = isCancel;
        }
        public MyAndromedaUser User { get; set; }
        public bool Cancel { get; set; }

    }

    public static class Extensions
    {
        public static void Each<IUserUpdatedEvent>(this IEnumerable<IUserUpdatedEvent> items, 
            Action<IUserUpdatedEvent> Eventaction, 
            Action<Exception> ExceptionAction)
        {
            if (items == null) return;
            var cached = items;
            foreach (var item in cached)
                Eventaction(item);
        }
    }
}
