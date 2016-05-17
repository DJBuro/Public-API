using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyAndromeda.Core.User;
using MyAndromeda.Data.Model.MyAndromeda;
using System.Data.Entity;

namespace MyAndromeda.Data.DataAccess.Users
{
    public class UserDataService : IUserDataService
    {
        private readonly MyAndromedaDbContext dbContext;

        public UserDataService(MyAndromedaDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<UserRecord> Query(Expression<Func<UserRecord, bool>> query)
        {
            DbSet<UserRecord> table = this.dbContext.UserRecords;
            IQueryable<UserRecord> tableQuery = table.Where(query);

            return tableQuery.ToArray();
        }

        public UserRecord New()
        {
            return new UserRecord();
        }

        public MyAndromedaUser GetByUserName(string userName)
        {
            MyAndromedaUser model = null;

            UserRecord result = this.dbContext.UserRecords
            //.Include(e=> e.UserIpLocking)
                             .Where(e => e.Username.Equals(userName))
                             .SingleOrDefault();

            if (result == null)
            {
                return null;
            }

            model = result.ToDomainModel();

            return model;
        }

        public UserRecord GetByUserId(int userId)
        {
            UserRecord result = this.dbContext.UserRecords
                            //.Include(e=> e.UserIpLocking)
                             .Where(e => e.Id.Equals(userId))
                             .SingleOrDefault();

            return result;
        }

        public IEnumerable<MyAndromedaUser> QueryForUsers(Expression<Func<UserRecord, bool>> query)
        {
            IEnumerable<MyAndromedaUser> results;
            
            DbSet<UserRecord> table = this.dbContext.UserRecords;
            IQueryable<UserRecord> userQuery = table.Where(query);
            UserRecord[] result = userQuery.ToArray();

            results = result.Select(e => e.ToDomainModel());

            return results;
        }

        public void Add(UserRecord user)
        {
            DbSet<UserRecord> table = this.dbContext.UserRecords;
            table.Add(user);

            this.dbContext.SaveChanges();
        }

        public void Update(UserRecord user)
        {
            //var table = dbContext.UserRecords;
            //var entity = table.Where(e => e.Id == user.Id).Single();
            //entity.FirstName = user.FirstName;
            //entity.LastName = user.LastName;
            //entity.IsEnabled = user.IsEnabled;
            //entity.Password = user.Password;
            //entity.PasswordSalt = user.PasswordSalt;
            //entity.PasswordFormat = user.PasswordFormat;
            //entity.HashAlgorithm = user.HashAlgorithm;
            this.dbContext.SaveChanges();
        }

        public async Task<MyAndromedaUser> GetByUserNameAsync(string userName)
        {
            MyAndromedaUser model = null;

            UserRecord result = await this.dbContext.UserRecords
                             .Where(e => e.Username.Equals(userName))
                             .SingleOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            model = result.ToDomainModel();

            return model;
        }

        public async Task<UserRecord> GetByUserIdAsync(int userId)
        {
            UserRecord result = await this.dbContext.UserRecords
                            //.Include(e=> e.UserIpLocking)
                             .Where(e => e.Id == userId)
                             .SingleOrDefaultAsync();

            return result;
        }
    }
}