using System;

namespace MyAndromeda.CloudSynchronization.Events
{
    public class SyncronizationContext
    {
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the task count.
        /// </summary>
        /// <value>The task count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the store ids.
        /// </summary>
        /// <value>The store ids.</value>
        public int[] StoreIds { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public DateTime TimeStamp { get; set; }
    }
}