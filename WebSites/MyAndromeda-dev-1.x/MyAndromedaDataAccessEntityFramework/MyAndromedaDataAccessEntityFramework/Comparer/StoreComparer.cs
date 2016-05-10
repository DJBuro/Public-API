using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.Comparer
{
    public class StoreComparer : IEqualityComparer<Store>
    {
        public bool Equals(
            Store x,
            Store y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Store obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}