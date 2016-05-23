using System;
using System.Linq;

namespace MyAndromeda.Core
{
    public interface IDelay : IDependency
    {
        int DelayInMilliseconds { get; }
        int DelayInSeconds { get; }
    }
}
