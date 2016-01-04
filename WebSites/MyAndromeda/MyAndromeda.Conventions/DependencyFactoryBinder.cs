using MyAndromeda.Framework;
using Ninject.Extensions.Conventions.BindingGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Syntax;

namespace MyAndromeda.Conventions
{
    //public class DependencyConventionBinder : IBindingGenerator
    //{
    //    private static Type rootDependency = typeof(IDependency);
    //    public IEnumerable<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindings(Type type, IBindingRoot bindingRoot)
    //    {
    //        if (type != null && !type.IsAbstract && type.IsClass && typeof(IDependency).IsAssignableFrom(type))
    //        {
    //            var interfaces = type.GetInterfaces().Where(e => e != rootDependency).ToArray();
    //            if (interfaces.Length > 0)
    //            {
    //                //bindingRoot.Bind(interfaces).To(type).Named(type.Name) as IBindingWhenInNamedWithOrOnSyntax<object>;
    //                foreach (var interfaceType in interfaces)
    //                {
    //                    var binding = bindingRoot.Bind(interfaceType).To(type).Named(type.Name);

    //                    yield return binding as IBindingWhenInNamedWithOrOnSyntax<object>;
    //                }
    //            }
    //        }
    //    }
    //}
    //public class BaseTypeBindingGenerator<IDependency> : IBindableTypeSelector 
    //{
    //    private static Type rootDependency = typeof(IDependency);

    //    public IEnumerable<Type> GetBindableInterfaces(Type type)
    //    {
    //        if (type == rootDependency) 
    //        {
    //            return E;
    //        }


    //    }

    //    public IEnumerable<Type> GetBindableBaseTypes(Type type)
    //    {

    //    }
    //}
}
