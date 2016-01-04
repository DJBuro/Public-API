using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Framework.Helpers
{
    public static class GenericCollectionExtensions
    {
        public static T Random<T>(this IEnumerable<T> collection) where T : class
        {
            var count = collection.Count();
            if (count == 0)
                return null;

            Random r = new Random();
            var index = r.Next(count - 1);

            return collection.Skip(index).Take(1).First();
        }


        public static decimal DevideBy(this int number, int value) 
        {
            var r = number;
            if (r == 0) { return r; }

            var a = Convert.ToDecimal(r);
            return a / value;
        }
        public static decimal DevideBy(this int? number, int value) 
        {
            var r = number.GetValueOrDefault(0);

            return r.DevideBy(value);
        } 
    }
}
