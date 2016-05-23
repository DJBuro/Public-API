using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Entity;
using MyAndromeda.Core.Site;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Data.DataAccess.Permissions;

namespace MyAndromeda.Data.DataAccess.Permissions
{
    public class EnrolmentDataService : IEnrolmentDataService 
    {
        private readonly MyAndromedaDbContext dbContext;

        public EnrolmentDataService(MyAndromedaDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<IEnrolmentLevel> List()
        {
            IEnumerable<IEnrolmentLevel> items; 
            
            var table = this.dbContext.EnrolmentLevels;
            items = table.ToArray().Select(e => e as IEnrolmentLevel).ToList();

            return items;
        }

        public IEnumerable<IEnrolmentLevel> Query(Expression<Func<EnrolmentLevel, bool>> query)
        {
            IEnumerable<IEnrolmentLevel> items;
            
            var table = this.dbContext.EnrolmentLevels;
            items = table.Where(query).ToArray().Select(e => e as IEnrolmentLevel).ToList();
            
            return items;
        }

        public IEnrolmentLevel Create(string name, string description) 
        {
            IEnrolmentLevel model;
            
            var table = this.dbContext.EnrolmentLevels;

            var entity = new Model.MyAndromeda.EnrolmentLevel() { Name = name, Description = description };

            model = entity;
            table.Add(entity);
            this.dbContext.SaveChanges();
            
            return model;
        }

        public void Update(IEnrolmentLevel model) 
        {
            var table = this.dbContext.EnrolmentLevels;
            var item = table.Where(e => e.Id == model.Id).Single();

            item.Name = model.Name;
            item.Description = model.Description;

            this.dbContext.SaveChanges();
        }

        public void Delete(IEnrolmentLevel enrollment)
        {
            var table = this.dbContext.EnrolmentLevels;
            var item = table.Where(e => e.Name == enrollment.Name).Single();

            table.Remove(item);

            this.dbContext.SaveChanges();
        }
    }
}