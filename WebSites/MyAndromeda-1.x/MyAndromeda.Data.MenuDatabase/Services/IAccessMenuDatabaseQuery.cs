using System;
using System.Collections.Generic;
using MyAndromeda.Core;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessMenuDatabaseQuery<TRowType> : IDependency, IDisposable
        where TRowType : System.Data.DataRow
    {
        IEnumerable<TRowType> List(Func<TRowType, bool> query = null);

        void SaveChanges();
    }
}