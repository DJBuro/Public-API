using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MyAndromeda.Core;
using MyAndromeda.Core.User;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Framework.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAndromeda.Identity
{
    public class ApplicationUserManager : UserManager<MyAndromedaIdentityUser, int>
    {
        public ApplicationUserManager(IUserStore<MyAndromedaIdentityUser, int> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore());
                //new UserStore<MyAndromedaIdentityUser>(
                //    context.Get<MyAndromedaDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator =
                new UserValidator<MyAndromedaIdentityUser, int>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };


            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };


            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses 
            // Phone and Emails as a step of receiving a code for verifying 
            // the user You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode",
                new PhoneNumberTokenProvider<MyAndromedaIdentityUser, int>
                {
                    MessageFormat = "Your security code is: {0}"
                });

            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<MyAndromedaIdentityUser, int>
                {
                    Subject = "SecurityCode",
                    BodyFormat = "Your security code is {0}"
                });

            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<MyAndromedaIdentityUser, int>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
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