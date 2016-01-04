using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.DataAccess.Permissions;

namespace MyAndromeda.Data.DataAccess.Permissions
{
    public class PermissionDataAccessService : IPermissionDataAccessService
    {
        private readonly MyAndromedaDbContext dbContext;

        public PermissionDataAccessService(MyAndromedaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddOrUpdatePermissions(IEnumerable<IPermission> permissions)
        {
            var table = this.dbContext.Permissions;
            var all = table.ToList();

            foreach (IPermission permission in permissions)
            {
                var entity = all.FirstOrDefault(e => e.Name == permission.Name && e.Category == permission.Category) ??
                             new Data.Model.MyAndromeda.Permission()
                             {
                                 Category = permission.Category,
                                 Name = permission.Name,
                                 Description = permission.Description
                             };

                if (entity.Id == 0)
                {
                    table.Add(entity);
                }
            }

            this.dbContext.SaveChanges();
        }

        public void AddOrUpdatePermission(IPermission permission)
        {
            var table = this.dbContext.Permissions;

            var entity = table.FirstOrDefault(e => e.Name == permission.Name && e.Category == permission.Category) ??
                         new Data.Model.MyAndromeda.Permission()
                         {
                             Category = permission.Category,
                             Name = permission.Name,
                             Description = permission.Description
                         };

            if (entity.Id == 0)
            {
                table.Add(entity);
            }

            this.dbContext.SaveChanges();
        }

        public void UpdateRole(IUserRole role, IEnumerable<IPermission> permissions)
        {
            var roleTable = this.dbContext.Roles;
            var roleEntity = roleTable.SingleOrDefault(e => e.Id == role.Id);
            roleEntity.RolePermissions.Clear();

            this.AddPermissionsToCollection(roleEntity.RolePermissions, permissions);

            this.dbContext.SaveChanges();
        }

        public void UpdateEnrolmentPermissions(IEnrolmentLevel enrolementLevel, IEnumerable<IPermission> permissions)
        {
            var enrolementTable = this.dbContext.EnrolmentLevels;
            var enrolmentEntity = enrolementLevel.Id == 0 ? enrolementTable.SingleOrDefault(e => e.Name == enrolementLevel.Name) :
                                  enrolementTable.SingleOrDefault(e => e.Id == enrolementLevel.Id);

            this.AddPermissionsToCollection(enrolmentEntity.Permissions, permissions);

            this.dbContext.SaveChanges();
        }

        public IEnumerable<IPermission> GetAllPermissions()
        {
            IEnumerable<IPermission> results = Enumerable.Empty<IPermission>();

            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var table = this.dbContext.Permissions;
                var data = table.ToArray();
                results = data.Select(e => e as IPermission);
            }

            return results;
        }

        public IEnumerable<IPermission> GetEffectiveUserPermissions(int userId)
        {
            IEnumerable<IPermission> permissions;
            var permissionsTable = this.dbContext.Permissions;
            var query = permissionsTable
                                        .Where(e => e.RolePermissions.Any(userRole => userRole.Role.UserRecords.Any(userRecord => userRecord.Id == userId)));

            var result = query.ToArray();
            permissions = result.Select(e => e as IPermission).ToArray();

            return permissions;
        }

        public IEnumerable<IPermission> GetEffectiveUserPermissions(IUserRole userRole)
        {
            IEnumerable<IPermission> permissions;
        
            var permissionsTable = this.dbContext.Permissions;
            var query = userRole.Id == 0 ? 
                        permissionsTable.Where(e => e.RolePermissions.Any(role => role.Role.Name == userRole.Name)) :
                        permissionsTable.Where(e => e.RolePermissions.Any(role => role.Role.Id == userRole.Id));

            //query = query.Where(e => e.Category == "");
            var result = query.ToArray();
            permissions = result.Select(e => e as IPermission).ToArray();

            return permissions;
        }

        public IEnumerable<IPermission> GetEffectiveEnrolementPermissions(IEnrolmentLevel role)
        {
            IEnumerable<IPermission> permissions;
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var permissionsTable = this.dbContext.Permissions;
                var query = role.Id == 0 ? permissionsTable.Where(e => e.EnrolmentLevels.Any(level => level.Name == role.Name)) :
                            permissionsTable.Where(e => e.EnrolmentLevels.Any(level => level.Id == role.Id));

                var results = query.ToArray();
                permissions = results.Select(e => e as IPermission).ToArray();
            }

            return permissions;
        }

        public IEnumerable<IPermission> GetEffectiveEntolementPermissions(ISite site)
        {
            IEnumerable<IPermission> permissions;
            var permissionsTable = this.dbContext.Permissions;

            var query = permissionsTable
                                        .Where(e => e.EnrolmentLevels.Any(level => level.StoreEnrolments.Any(enrolement => enrolement.StoreId == site.Id)));

            var defaultPermissions = permissionsTable.Where(e => e.EnrolmentLevels.Any(enrolment => enrolment.Name == ExpectedStoreEnrollments.DefaultStoreEnrollment));

            var results = query.Union(defaultPermissions).ToArray();

            permissions = results.Select(e => e as IPermission).ToArray();

            return permissions;
        }

        public IEnumerable<IUserRole> GetUserRolePermissions(int userId)
        {
            var userRoles = new List<IUserRole>();

            //grab user roles 
            var appliedRoles = this.dbContext
            .Roles.Where(e => e.UserRecords.Any(r => r.Id == userId) || e.Name == ExpectedUserRoles.User)
                                   .ToArray();

            //grab user role permissions.
            var rolePermissionsTable = this.dbContext.RolePermissions
                                           .Include(e => e.Role)
                                           .Include(e => e.Permission)
                //find all roles for the user level and the 'store user' role 
                                           .Where(e => e.Role.UserRecords.Any(r => r.Id == userId) || e.Role.Name == ExpectedUserRoles.User)
                                           .ToArray();

            var groups = rolePermissionsTable
                                             .GroupBy(e => e.RoleId)
                                             .ToDictionary(e => e.Key, e => e.ToList());

            foreach (var role in appliedRoles)
            {
                role.EffectivePermissions = Enumerable.Empty<IPermission>();
                userRoles.Add(role);
            }

            foreach (var key in groups.Keys)
            {
                var group = userRoles.Where(e => e.Id == key).Single();
                group.EffectivePermissions = groups[key].Select(e => new Permission()
                {
                    Id = e.PermissionId,
                    Name = e.Permission.Name,
                    Description = e.Permission.Description,
                    Category = e.Permission.Category
                }).ToList();
            }

            return userRoles;
        }

        private void AddPermissionsToCollection(ICollection<RolePermissions> rolePermissions, IEnumerable<IPermission> permissions)
        {
            var permissionTable = dbContext.Permissions.ToList();

            foreach (var permission in permissions)
            {
                var permissionEntity = permissionTable.SingleOrDefault(e =>
                    e.Name == permission.Name &&
                    e.Category == permission.Category);

                if (permissionEntity == null)
                {
                    permissionEntity = new Model.MyAndromeda.Permission()
                    {
                        Name = permission.Name,
                        Description = permission.Description,
                        Category = permission.Category
                    };
                }

                rolePermissions.Add(new RolePermissions() { Permission = permissionEntity });
            }
        }

        private void AddPermissionsToCollection(ICollection<Permission> collection, IEnumerable<IPermission> permissions)
        {
            var permissionTable = dbContext.Permissions.ToList();

            foreach (var permission in permissions)
            {
                var permissionEntity = permissionTable.SingleOrDefault(e => e.Name == permission.Name && e.Category == permission.Category);
                if (permissionEntity == null)
                {
                    permissionEntity = new Model.MyAndromeda.Permission()
                    {
                        Name = permission.Name,
                        Description = permission.Description,
                        Category = permission.Category
                    };
                }

                collection.Add(permissionEntity);
            }
        }
    }
}