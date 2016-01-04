using MyAndromeda.Core;

namespace MyAndromeda.Data.AcsServices.Events
{
    public interface IMenuLoadingEvent : ITransientDependency 
    {
        /// <summary>
        /// Gets the order. Lower the value the sooner the action
        /// </summary>
        /// <value>The order.</value>
        int Order { get; }

        /// <summary>
        /// Waht feature the 
        /// </summary>
        /// <value>The feature.</value>
        string HandlerName { get; }

        /// <summary>
        /// Loaded the specified menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        void Loaded(IMenuLoadingContext context);
    }
}