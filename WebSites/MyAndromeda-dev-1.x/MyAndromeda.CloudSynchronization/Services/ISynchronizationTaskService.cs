using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.CloudSynchronization.Services
{
    public interface ISynchronizationTaskService : IDependency
    {
        /// <summary>
        /// Gets the tasks to run.
        /// </summary>
        /// <param name="utcNow">The UTC now.</param>
        /// <returns></returns>
        IEnumerable<CloudSynchronizationTask> GetTasksToRun(DateTime utcNow);

        /// <summary>
        /// Completes the tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        void CompleteTasks(IEnumerable<CloudSynchronizationTask> tasks);

        /// <summary>
        /// Creates the task.
        /// </summary>
        /// <param name="task">The task.</param>
        void CreateTask(CloudSynchronizationTask task);

        void CauseStoreToUpdate(int andromedaSiteId);
    }
}
