using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Framework.Helpers
{
    public static class GenericCollectionExtensions
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body)
        {
            List<Exception> exceptions = null;
            foreach (var item in source)
            {
                try { await body(item); }
                catch (Exception exc)
                {
                    if (exceptions == null) exceptions = new List<Exception>();
                    exceptions.Add(exc);
                }
            }
            if (exceptions != null)
                throw new AggregateException(exceptions);
        }


        //public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body)
        //{
        //    return Task.WhenAll(
        //        from item in source
        //        select Task.Run(() => body(item)));
        //}

        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
        {
            return Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate
                {
                    using (partition)
                        while (partition.MoveNext())
                            await body(partition.Current);
                }));
        }

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
