using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Core.Data
{
    public interface IDataProvider<T>
        where T : new ()
    {
        void ChangeIncludeScope<TPropertyModel>(Expression<Func<T, TPropertyModel>> predicate); 

        /// <summary>
        /// Creates a new model for use for use with insertion.
        /// </summary>
        /// <returns></returns>
        T New(); 

        /// <summary>
        /// Gets the specified query.
        /// </summary>
        /// <param name="predicate">The query.</param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> predicate); 

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> List();

        /// <summary>
        /// Lists by the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<T> List(Expression<Func<T, bool>> predicate);
    }


}
