using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;

namespace MyAndromeda.Data.DataAccess.Users
{
    public class UserLockingDataService : IUserLockingDataService 
    {
        public UserLockingDataService() 
        {
        }

        public void CreateOrUpdateRestrictionsByUserId(int id, string ipRanges)
        {
            using (var dbContext = new MyAndromedaDbContext()) 
            {
                var user = dbContext.UserRecords
                                    .Include(e => e.UserIpLock)
                                    .Single(e => e.Id == id);

                if (user.UserIpLock == null)
                {
                    user.UserIpLock = new UserIpLock();
                }
                
                user.UserIpLock.Enabled = true;
                user.UserIpLock.ValidIpV4Ranges = ipRanges;

                dbContext.SaveChanges();
            }
        }

        public UserIpLock GetLockByUserName(string userName)
        {
            return this.List(e => e.UserRecord.Username == userName).SingleOrDefault();
        }

        public UserIpLock GetLockByUserId(int id)
        {
            var result = this.List(e => e.UserId == id).SingleOrDefault();

            if (result != null)
            {
                return result;
            }

            this.CreateOrUpdateRestrictionsByUserId(id, string.Empty);

            return new UserIpLock()
            {
                UserId = id,
                Enabled = true,
                ValidIpV4Ranges = string.Empty
            };
        }

        public IEnumerable<UserIpLock> List(Expression<Func<UserIpLock, bool>> query)
        {
            IEnumerable<UserIpLock> results = Enumerable.Empty<UserIpLock>();

            using (var dbContext = new MyAndromedaDbContext()) 
            {
                var table = dbContext.UserIpLocks;
                var tableQuery = table.Where(query);

                results = tableQuery.ToArray();
            }

            return results;
        }
    }
}