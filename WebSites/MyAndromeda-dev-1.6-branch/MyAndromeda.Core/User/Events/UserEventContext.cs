namespace MyAndromeda.Core.User.Events
{
    public class UserEventContext
    {
        public UserEventContext() 
        {
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public MyAndromedaUser User { get; set; }

        /// <summary>
        /// Cancel the event.
        /// </summary>
        /// <value>The cancel.</value>
        public bool Cancel { get; set; }
    }
}