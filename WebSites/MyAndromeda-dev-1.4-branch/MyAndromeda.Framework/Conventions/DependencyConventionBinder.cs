using System.Web.Mvc;
using MyAndromeda.Core;
using Ninject.Extensions.Conventions.BindingGenerators;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Framework.Conventions
{
    public class DependencyConventionBinder : IBindingGenerator
    {
        private static readonly Type rootDependency = typeof(IDependency);

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

    public class SingletonDependencyConventionBinder : IBindingGenerator
    {
        private static readonly Type rootDependency = typeof(ISingletonDependency);

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

    //public class ControllerConventionBinder : IBindingGenerator 
    //{
    //    private static Type rootDependency = typeof(IController);

    //    public IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindings(Type type, IBindingRoot bindingRoot)
    //    {
    //        if (type != null && !type.IsAbstract && type.IsClass)
    //        {
    //            var binding = bindingRoot.Bind(type, 
    //        }
    //    }
    //}
}
