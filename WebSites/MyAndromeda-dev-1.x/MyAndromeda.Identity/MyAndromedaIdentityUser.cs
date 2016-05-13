using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MyAndromeda.Identity
{
    /// <summary>
    /// implement lots of interface on Microsoft.AspNet.Identity.I.....
    /// </summary>
    public class MyAndromedaIdentityUser : IUser<int>
    {
        public MyAndromedaIdentityUser(int id, string userName) 
        {
            this.Id = id;
            this.UserName = userName;
        }

        public int Id { get; private set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this,
                    DefaultAuthenticationTypes.ApplicationCookie);
            
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}