using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Users
{
    public class UserRoleDataService : IUserRoleDataService 
    {
        private readonly MyAndromedaDbContext dbContext;

        public UserRoleDataService(MyAndromedaDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
          
        public void CreateORUpdate(IUserRole userRole)
        {
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var table = this.dbContext.Roles;
                Role entity;
                entity = table.FirstOrDefault(e => e.Name == userRole.Name) ?? table.Create();
                  
                entity.Name = userRole.Name;

                table.Add(entity);
                userRole.Id = entity.Id;

                this.dbContext.SaveChanges();
            }
        }

        public IUserRole Get(int roleId)
        {
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var table = this.dbContext.Roles;
                Role entity = table.Single(e => e.Id == roleId);

                return entity;
            }
        }

        public IUserRole Get(string name)
        {
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var table = this.dbContext.Roles;
                Role entity = table.Single(e => e.Name == name);

                return entity;
            }
        }

        public IEnumerable<IUserRole> List()
        {
            IEnumerable<IUserRole> items;
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var table = this.dbContext.Roles;

                items = table.ToList();
            }

            return items;
        }

        public IEnumerable<IUserRole> ListRolesForUser(int userId)
        {
            IEnumerable<IUserRole> results;

            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var table = this.dbContext.Roles;
                var query = table.Where(e => e.UserRecords.Any(user => user.Id == userId));
                results = query.ToArray();
            }

            return results;
        }

        public void AddRolesToUser(int userId, IEnumerable<IUserRole> userRoles)
        {
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var table = this.dbContext.Roles;
                var allRoles = table.ToArray();
                var user = this.dbContext.UserRecords.Single(e => e.Id == userId);

                user.Roles.Clear();
                this.dbContext.SaveChanges();

                foreach (IUserRole userRole in userRoles) 
                {
                    var role = allRoles.Single(e => e.Id == userRole.Id);
                    role.UserRecords.Add(user);
                }

                this.dbContext.SaveChanges();
            }
        }

        public void Delete(IUserRole role)
        {
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var table = this.dbContext.Roles;

                var entity = table.SingleOrDefault(e => e.Name == role.Name);

                if (entity == null)
                {
                    return;
                }

                table.Remove(entity);
                this.dbContext.SaveChanges();
            }
        }
    }
}