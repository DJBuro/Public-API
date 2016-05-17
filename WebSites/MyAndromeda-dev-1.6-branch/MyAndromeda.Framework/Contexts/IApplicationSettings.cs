using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Framework.Contexts
{
    public interface IApplicationSettings : ITransientDependency
    {
        bool DisplayDebugViews { get; }
        bool ProfileApplication { get;  }
        bool SignalrAsALogger { get; }
    }
}
