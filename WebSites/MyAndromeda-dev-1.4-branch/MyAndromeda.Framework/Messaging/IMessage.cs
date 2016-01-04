using MyAndromeda.Core;

namespace MyAndromeda.Framework.Messaging
{
    public interface IMessage : IDependency 
    {
        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>To.</value>
        string To { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        string Body { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }
    }
}