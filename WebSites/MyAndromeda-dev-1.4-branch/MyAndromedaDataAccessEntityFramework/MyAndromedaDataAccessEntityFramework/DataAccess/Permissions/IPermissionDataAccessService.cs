using MyAndromeda.Core;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Permissions
{
    public interface IPermissionDataAccessService : IDependency
    {
        /// <summary>
        /// Adds the or update permissions.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        void AddOrUpdatePermissions(IEnumerable<IPermission> permissions);

        /// <summary>
        /// Adds the or update permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        void AddOrUpdatePermission(IPermission permission); 
        
        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="permissions">The permissions.</param>
        void UpdateRole(IUserRole role, IEnumerable<IPermission> permissions);
        
        /// <summary>
        /// Updates the enrolment permissions.
        /// </summary>
        /// <param name="enrolementLevel">The enrolment level.</param>
        /// <param name="permissions">The permissions.</param>
        void UpdateEnrolmentPermissions(MyAndromeda.Core.Site.IEnrolmentLevel enrolementLevel, IEnumerable<IPermission> permissions);

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPermission> GetAllPermissions();

        /// <summary>
        /// Gets the effective user permissions.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveUserPermissions(int userId);

        /// <summary>
        /// Gets the effective user permissions.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveUserPermissions(IUserRole role);

        /// <summary>
        /// Gets the effective enrolement permissions.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveEnrolementPermissions(IEnrolmentLevel role);

        /// <summary>
        /// Gets the effective entolement permissions.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        IEnumerable<IPermission> GetEffectiveEntolementPermissions(ISite site);

        /// <summary>
        /// Gets the user role permissions.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<IUserRole> GetUserRolePermissions(int userId);
    }

    public class PermissionDataAccessService : IPermissionDataAccessService 
    {
        private readonly MyAndromedaDbContext dbContext;

        public PermissionDataAccessService(MyAndromedaDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public void AddOrUpdatePermissions(IEnumerable<IPermission> permissions)
        {
            var table = dbContext.Permissions;
            var all = table.ToList();

            foreach (var permission in permissions) 
            {
                var entity = all.FirstOrDefault(e => e.Name == permission.Name && e.Category == permission.Category) ?? 
                    new Model.MyAndromeda.Permission(){
                        Category = permission.Category,
                        Name = permission.Name,
                        Description = permission.Description
                    };

                if (entity.Id == 0) 
                {
                    table.Add(entity);
                }
            }

            dbContext.SaveChanges();
        }

        public void AddOrUpdatePermission(IPermission permission)
        {
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var table = dbContext.Permissions;

                var entity = table.FirstOrDefault(e => e.Name == permission.Name && e.Category == permission.Category) ??
                        new Model.MyAndromeda.Permission()
                        {
                            Category = permission.Category,
                            Name = permission.Name,
                            Description = permission.Description
                        };

                if (entity.Id == 0)
                {
                    table.Add(entity);
                }

                dbContext.SaveChanges();
            }
        }

        public void UpdateRole(IUserRole role, IEnumerable<IPermission> permissions)
        {
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var roleTable = dbContext.Roles;
                var roleEntity = roleTable.SingleOrDefault(e => e.Id == role.Id);
                roleEntity.RolePermissions.Clear();

                this.AddPermissionsToCollection(dbContext, roleEntity.RolePermissions, permissions);
                
                dbContext.SaveChanges();
            }
        }
  
        public void UpdateEnrolmentPermissions(IEnrolmentLevel enrolementLevel, IEnumerable<IPermission> permissions)
        {
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var enrolementTable = dbContext.EnrolmentLevels;
                var enrolmentEntity = 
                    enrolementLevel.Id == 0 ? 
                        enrolementTable.SingleOrDefault(e=> e.Name == enrolementLevel.Name) : 
                        enrolementTable.SingleOrDefault(e=> e.Id == enrolementLevel.Id);

                this.AddPermissionsToCollection(dbContext, enrolmentEntity.Permissions, permissions);

                dbContext.SaveChanges();
            }
        }

        public IEnumerable<IPermission> GetAllPermissions() 
        {
            var results = Enumerable.Empty<IPermission>();

            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var table = dbContext.Permissions;
                var data = table.ToArray();
                results = data.Select(e => e as IPermission);
            }

            return results;
        }

        public IEnumerable<IPermission> GetEffectiveUserPermissions(int userId)
        {
            IEnumerable<IPermission> permissions;
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var permissionsTable = dbContext.Permissions;
                var query = permissionsTable
                        .Where(e => e.RolePermissions.Any(userRole => userRole.Role.UserRecords.Any(userRecord => userRecord.Id == userId))
                        );

                var result = query.ToArray();
                permissions = result.Select(e => e as IPermission).ToArray();
            }

            return permissions;
        }

        public IEnumerable<IPermission> GetEffectiveUserPermissions(IUserRole userRole)
        {
            IEnumerable<IPermission> permissions;
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var permissionsTable = dbContext.Permissions;
                var query = userRole.Id == 0 ?
                    permissionsTable.Where(e => e.RolePermissions.Any(role => role.Role.Name == userRole.Name)) :
                    permissionsTable.Where(e => e.RolePermissions.Any(role => role.Role.Id == userRole.Id));

                var result = query.ToArray();
                permissions = result.Select(e => e as IPermission).ToArray();
            }

            return permissions;
        }

        public IEnumerable<IPermission> GetEffectiveEnrolementPermissions(IEnrolmentLevel role)
        {
            IEnumerable<IPermission> permissions;
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var permissionsTable = dbContext.Permissions;
                var query = role.Id == 0 ?
                    permissionsTable.Where(e => e.EnrolmentLevels.Any(level => level.Name == role.Name)) :
                    permissionsTable.Where(e => e.EnrolmentLevels.Any(level => level.Id == role.Id));

                var results = query.ToArray();
                permissions = results.Select(e => e as IPermission).ToArray();
            }

            return permissions;
        }

        public IEnumerable<IPermission> GetEffectiveEntolementPermissions(ISite site)
        {
            IEnumerable<IPermission> permissions;
            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext())
            {
                var permissionsTable = dbContext.Permissions;

                var query = permissionsTable
                    .Where(e => e.EnrolmentLevels.Any(level => level.StoreEnrolments.Any(enrolement => enrolement.StoreId == site.Id)));
                
                var defaultPermissions =
                    permissionsTable.Where(e => e.EnrolmentLevels.Any(enrolment => enrolment.Name == ExpectedStoreEnrollments.DefaultStoreEnrollment));

                var results = //two queries in one
                    query.Union(defaultPermissions).ToArray();
                
                permissions = results.Select(e => e as IPermission).ToArray();
            }

            return permissions;
        }

        public IEnumerable<IUserRole> GetUserRolePermissions(int userId)
        {
            List<IUserRole> userRoles = new List<IUserRole>();

            //using (var dbContext = new Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                //grab user roles 
                var appliedRoles = dbContext
                    .Roles.Where(e=> e.UserRecords.Any(r=> r.Id == userId) || e.Name == ExpectedUserRoles.User)
                    .ToArray();

                //grab user role permissions.
                var rolePermissionsTable = dbContext.RolePermissions
                    .Include(e=> e.Role)
                    .Include(e=> e.Permission)
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
                    group.EffectivePermissions = groups[key].Select(e=> new Permission(){
                        Id = e.PermissionId,
                        Name = e.Permission.Name,
                        Description = e.Permission.Description,
                        Category = e.Permission.Category
                    }).ToList();
                }
            }

            return userRoles;
        }

        private void AddPermissionsToCollection(
            MyAndromedaDbContext dbContext,
            ICollection<RolePermissions> rolePermissions,
            IEnumerable<IPermission> permissions)
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

        private void AddPermissionsToCollection(
            MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.MyAndromedaDbContext dbContext,
            ICollection<MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.Permission> collection,
            IEnumerable<IPermission> permissions)
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
