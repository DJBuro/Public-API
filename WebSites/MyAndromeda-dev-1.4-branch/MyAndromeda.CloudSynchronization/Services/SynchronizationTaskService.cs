using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromeda.CloudSynchronization.Services
{
    public class SynchronizationTaskService : ISynchronizationTaskService 
    {
        public SynchronizationTaskService()
        {
        }

        public IEnumerable<CloudSynchronizationTask> GetTasksToRun(DateTime utcNow)
        {
            IEnumerable<CloudSynchronizationTask> tasks;
            using (var dbContext = new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.MyAndromedaDbContext()) 
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
            using (var dbContext = new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.MyAndromedaDbContext())
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
            using (var dbContext = new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.MyAndromedaDbContext()) 
            {
                var table = dbContext.CloudSynchronizationTasks;
                
                table.Add(task);

                dbContext.SaveChanges();
            }
        }

    }
}