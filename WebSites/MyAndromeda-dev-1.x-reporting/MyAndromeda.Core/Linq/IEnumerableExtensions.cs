using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Core.Linq
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TModel> Flatten<TModel>(
            this IEnumerable<TModel> e, 
            Func<TModel, IEnumerable<TModel>> children)
        {
            return e.SelectMany(c => children(c).Flatten(children)).Concat(e);
        }
    }
}
