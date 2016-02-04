using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.Model;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.CloudSynchronization.Services
{
    public class SynchronizationTaskService : ISynchronizationTaskService 
    {
        public SynchronizationTaskService()
        {
        }

        public void CauseStoreToUpdate(int andromedaSiteId) 
        {
            using (var dbContext = new MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext())
            {
                var store = dbContext.Stores.FirstOrDefault(e => e.AndromedaSiteId == andromedaSiteId);

                store.DataVersion = dbContext.GetNextDataVersionForEntity();

                dbContext.SaveChanges();
            }
        } 

        public IEnumerable<CloudSynchronizationTask> GetTasksToRun(DateTime utcNow)
        {
            IEnumerable<CloudSynchronizationTask> tasks;
            using (var dbContext = new MyAndromedaDbContext()) 
            {
                var table = dbContext.CloudSynchronizationTasks;
                var query = table.Where(e => !e.Completed);
                var result = query.ToArray();
                tasks = result;
            }

            return tasks;
        }

        public void CompleteTasks(IEnumerable<CloudSynchronizationTask> tasks)
        {
            using (var dbContext = new MyAndromedaDbContext())
            {
                var table = dbContext.CloudSynchronizationTasks;
                foreach (var task in tasks)
                {
                    var model = table.Single(e => e.Id == task.Id);
                    model.Completed = true;
                }

                dbContext.SaveChanges();
            }
        }

        public void CreateTask(CloudSynchronizationTask task)
        {
            using (var dbContext = new MyAndromedaDbContext()) 
            {
                var table = dbContext.CloudSynchronizationTasks;
                
                table.Add(task);

                dbContext.SaveChanges();
            }
        }

    }
}