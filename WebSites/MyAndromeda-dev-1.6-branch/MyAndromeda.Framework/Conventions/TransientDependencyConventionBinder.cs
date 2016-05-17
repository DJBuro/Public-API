using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using Ninject.Extensions.Conventions.BindingGenerators;
using Ninject.Syntax;

namespace MyAndromeda.Framework.Conventions
{
    /// <summary>
    /// Transient dependency builder
    /// </summary>
    public class TransientDependencyConventionBinder : IBindingGenerator
    {
        private static readonly Type rootDependency = typeof(ITransientDependency);
        private static readonly Type lazyType = typeof(Func<>);

        public IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindings(Type type, IBindingRoot bindingRoot)
        {
            if (type != null && !type.IsAbstract && type.IsClass && rootDependency.IsAssignableFrom(type))
            {
                var interfaces = type.GetInterfaces().Where(e => e != rootDependency).ToArray();
                if (interfaces.Length > 0)
                {
                    //bindingRoot.Bind(interfaces).To(type).Named(type.Name) as IBindingWhenInNamedWithOrOnSyntax<object>;
                    foreach (var interfaceType in interfaces)
                    {
                        var binding = bindingRoot.Bind(interfaceType).To(type).Named(type.Name);

                        yield return binding as IBindingWhenInNamedWithOrOnSyntax<object>;
                    }
                }
            }
        }
    }
}