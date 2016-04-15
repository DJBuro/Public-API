using Microsoft.AspNet.Identity;
using MyAndromeda.Data.Model.MyAndromeda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace MyAndromeda.Identity
{
    public class MyAndromedaIdentityRole : IRole<int>
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;set;
        }
    }

    public class MyAndromedaIdentityRoleStore : IQueryableRoleStore<MyAndromedaIdentityRole, int>
    {
        private readonly MyAndromedaDbContext dbContext;
        public IQueryable<MyAndromedaIdentityRole> Roles
        {
            get
            {
                var roles = this.dbContext.Roles.Select(e=> new MyAndromedaIdentityRole() {
                    Id = e.Id,
                    Name = e.Name
                });

                return roles;
            }
        }

        public Task CreateAsync(MyAndromedaIdentityRole role)
        {
            var entity = this.dbContext.Roles.Create();

            entity.Id = role.Id;
            entity.Name = role.Name;

            this.dbContext.Roles.Add(entity);

            return this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(MyAndromedaIdentityRole role)
        {
            var entity = await this.dbContext.Roles.FirstOrDefaultAsync(e => e.Id == role.Id);

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
