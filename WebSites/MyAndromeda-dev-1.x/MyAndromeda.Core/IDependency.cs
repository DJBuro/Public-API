using System;
using System.Linq;

namespace MyAndromeda.Core
{
    /// <summary>
    /// Lives to the extent of the request. 
    /// </summary>
    public interface IDependency
    {
    }

    /// <summary>
    /// New version is created every time the item is requested
    /// </summary>
    public interface ITransientDependency 
    {
    }

    /// <summary>
    /// Only one version of the object exists in the application lifetime 
    /// </summary>
    public interface ISingletonDependency
    { 
    }

    /// <summary>
    /// For mvc attribute filters
    /// </summary>
    public interface IDependencyFilter 
    {
    
    }


    public interface IEventContext : IDependency 
    {

    }
}
