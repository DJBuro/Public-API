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
            var table = this.dbContext.UserRecords;
            var tableQuery = table.Where(query);

            return tableQuery.ToArray();
        }

        public UserRecord New()
        {
            return new UserRecord();
        }

        public MyAndromedaUser GetByUserName(string userName)
        {
            MyAndromedaUser model = null;

            var result = this.dbContext.UserRecords
            //.Include(e=> e.UserIpLocking)
                             .Where(e => e.Username.Equals(userName))
                             .SingleOrDefault();

            if (result == null)
            {
                return null;
            }

            model = result.ToDomain();

            return model;
        }

        public UserRecord GetByUserId(int userId)
        {
            UserRecord model = null;
            var result = this.dbContext.UserRecords
            //.Include(e=> e.UserIpLocking)
                             .Where(e => e.Id.Equals(userId))
                             .SingleOrDefault();

            if (result == null)
            {
                return null;
            }

            model = result;

            return model;
        }

        public IEnumerable<MyAndromedaUser> QueryForUsers(Expression<Func<UserRecord, bool>> query)
        {
            IEnumerable<MyAndromedaUser> results;
            
            var table = this.dbContext.UserRecords;
            var userQuery = table.Where(query);
            var result = userQuery.ToArray();

            results = result.Select(e => e.ToDomain());

            return results;
        }

        public void Add(UserRecord user)
        {
            var table = this.dbContext.UserRecords;
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

            var result = await this.dbContext.UserRecords
                             .Where(e => e.Username.Equals(userName))
                             .SingleOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            model = result.ToDomain();

            return model;
        }

        public async Task<UserRecord> GetByUserIdAsync(int userId)
        {
            UserRecord model = null;
            var result = await this.dbContext.UserRecords
            //.Include(e=> e.UserIpLocking)
                             .Where(e => e.Id.Equals(userId))
                             .SingleOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            model = result;

            return model;
        }
    }
}