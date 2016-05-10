using Ninject;
using Ninject.Planning.Bindings;
using System;

namespace MyAndromeda.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class ReadOnlyDataAttribute : ConstraintAttribute
    {
        public override bool Matches(IBindingMetadata metadata)
        {
            return metadata.Has(key: "Readonly") && metadata.Get<bool>(key: "Readonly");
        }
    }
}
