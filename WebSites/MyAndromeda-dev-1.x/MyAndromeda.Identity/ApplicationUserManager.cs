using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Threading.Tasks;
using MyAndromeda.Core.User;
using MyAndromeda.Data.DataAccess.Users;
using System;
using MyAndromeda.Framework.Services;

namespace MyAndromeda.Identity
{
    public class ApplicationUserManager : UserManager<MyAndromedaIdentityUser, int>
    {
        public ApplicationUserManager(IUserStore<MyAndromedaIdentityUser, int> store) : base(store)
        {
        }
    }

    public class UserStore : IUserStore<MyAndromedaIdentityUser, int>
    {
        private readonly IUserDataService userDataService;
        private readonly IMembershipService membershipService;

        public UserStore()
        {
            this.userDataService = DependencyResolver.Current.GetService<IUserDataService>();
            this.membershipService = DependencyResolver.Current.GetService<IMembershipService>();
        }

        //public UserStore(IUserDataService userDataService, IMembershipService membershipService)
        //{
        //    this.membershipService = membershipService;
        //    this.userDataService = userDataService;
        //}

        public async Task CreateAsync(MyAndromedaIdentityUser user)
        {
            var andromedaUser = new MyAndromedaUser();
            andromedaUser.Username = user.UserName;
            andromedaUser.Id = user.Id;
            
            membershipService.CreateUser(andromedaUser, user.Password);
        }

        public Task DeleteAsync(MyAndromedaIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public async Task<MyAndromedaIdentityUser> FindByIdAsync(int userId)
        {
            var user = await userDataService.GetByUserIdAsync(userId);
            var andromedaIdentityUser = new MyAndromedaIdentityUser(userId, user.Username);
             
            andromedaIdentityUser.Password = user.Password;

            return andromedaIdentityUser;
        }

        public async Task<MyAndromedaIdentityUser> FindByNameAsync(string userName)
        {
            var user = await userDataService.GetByUserNameAsync(userName);
            var andromedaIdentityUser = new MyAndromedaIdentityUser(user.Id, user.Username);
            
            return andromedaIdentityUser;
        }

        public async Task UpdateAsync(MyAndromedaIdentityUser user)
        {
            var andromedaUser = membershipService.GetUser(user.UserName);

            membershipService.UpdateUser(andromedaUser);
        }
    }
}