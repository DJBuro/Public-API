using MyAndromeda.Core;

namespace MyAndromeda.Framework.Authorization
{
    public interface IMyAndromedaRegistrationSettings : IDependency
    {
        int MinRequiredNonAlphanumericCharacters { get; }

        int MinRequiredPasswordLength { get; }

        bool RequiresUniqueEmail { get; }
    }
}