using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using MyAndromeda.Data.Model.MyAndromeda;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;

namespace MyAndromeda.Identity
{
    public class MyAndromedaIdentityRoleStore : IQueryableRoleStore<MyAndromedaIdentityRole, int>
    {
        //to-do: fill me up
        private readonly MyAndromedaDbContext dbContext;

        public MyAndromedaIdentityRoleStore(MyAndromedaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public static MyAndromedaIdentityRoleStore Create(
            IdentityFactoryOptions<MyAndromedaIdentityRoleStore> options,
            IOwinContext context)
        {
            var manager = new MyAndromedaIdentityRoleStore(context.Get<MyAndromedaDbContext>());

            return manager;
        }

        public IQueryable<MyAndromedaIdentityRole> Roles
        {
            get
            {
                IQueryable<MyAndromedaIdentityRole> roles = this.dbContext.Roles.Select(e=> new MyAndromedaIdentityRole() {
                    Id = e.Id,
                    Name = e.Name
                });

                return roles;
            }
        }

        public Task CreateAsync(MyAndromedaIdentityRole role)
        {
            Role entity = this.dbContext.Roles.Create();

            entity.Id = role.Id;
            entity.Name = role.Name;

            this.dbContext.Roles.Add(entity);

            return this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(MyAndromedaIdentityRole role)
        {
            Role entity = await this.dbContext.Roles.FirstOrDefaultAsync(e => e.Id == role.Id);

            this.dbContext.Roles.Remove(entity);

            await this.dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        public async Task<MyAndromedaIdentityRole> FindByIdAsync(int roleId)
        {
            MyAndromedaIdentityRole entity = await this.dbContext.Roles
                .Where(e=> e.Id == roleId)
                .Select(e=> new MyAndromedaIdentityRole() { Id = e.Id, Name = e.Name })
                .FirstOrDefaultAsync();

            return entity;
        }

        public Task<MyAndromedaIdentityRole> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(MyAndromedaIdentityRole role)
        {
            throw new NotImplementedException();
        }
    }

}