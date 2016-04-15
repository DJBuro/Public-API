using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Identity
{
    public class ApplicationUserManager : UserManager<MyAndromedaIdentityUser>
    {
        public ApplicationUserManager(IUserStore<MyAndromedaIdentityUser> store) : base(store)
        {
        }
    }

    public class UserStore : IUserStore<MyAndromedaIdentityUser>
    {
        public Task CreateAsync(MyAndromedaIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(MyAndromedaIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<MyAndromedaIdentityUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<MyAndromedaIdentityUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(MyAndromedaIdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}
