using MyAndromeda.Core;

namespace MyAndromeda.SendGridService
{
    public interface ISendGridEmailSettings : ISingletonDependency
    {
        string UserName { get; }

        string Password { get; }
    }
}