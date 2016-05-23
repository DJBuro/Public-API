using System;
using System.Linq;
using MyAndromeda.Core;

namespace MyAndromeda.CloudSynchronization.Events
{
    public interface ISynchronizationEvent : IDependency 
    {
        /// <summary>
        /// Skipping the specified context. ie it doesn't need to do anything at the moment 
        /// </summary>
        /// <param name="context">The context.</param>
        void Skipping(SyncronizationContext context);

        /// <summary>
        /// Started the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        void Started(SyncronizationContext context);

        /// <summary>
        /// Completed the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        void Completed(SyncronizationContext context);

        /// <summary>
        /// Errors the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="error">The error.</param>
        void Error(SyncronizationContext context, string error);
    }
}
