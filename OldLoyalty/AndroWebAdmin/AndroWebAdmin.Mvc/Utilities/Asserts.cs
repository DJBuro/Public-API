using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Spring.Core.TypeConversion;

namespace AndroWebAdmin.Mvc.Utilities
{
    public class Asserts
    {
        //public static void ArgumentDerivedFrom(Page page, Type type, string message)
        //{
        //    if (!IsDerivedFrom(page, type)) throw new Exception(message);
        //}

        //public static void ArgumentDerivedFrom(Page page, Type type)
        //{
        //    ArgumentDerivedFrom(page, type, "");
        //}

        public static void ArgumentDerivedFrom(object data, Type type)
        {
            ArgumentExists(data, "data");
            ArgumentExists(type, "type");

            if (!IsDerivedFrom(data, type))
                throw new ArgumentException(String.Format("Assertion failed object class {0} is not a subclass of {1}", data.GetType(), type));
        }


        public static bool IsDerivedFrom(object data, Type type)
        {
            return IsDerivedFrom(data.GetType(), type);
        }

        public static bool IsDerivedFrom(Type datatype, Type derivedtype)
        {
            return (
                datatype.IsSubclassOf(derivedtype)
                || datatype.Equals(derivedtype)
                || derivedtype.IsAssignableFrom(datatype));
        }


        public static Object ArgumentExists<T>(T obj, string parametername)
        {
            if (IsEmpty(obj)) throw new ArgumentException(String.Format("{0} argument cannot be null", parametername));

            return obj;
        }

        public static void ArgumentMatchesPattern(string value, string pattern)
        {
            Match prefix = Regex.Match(value, pattern);

            if (!prefix.Success)
                throw new ArgumentException(
                    String.Format("Assertion failed argument \"{0}\" does not match the pattern \"{1}\"", value, pattern));
        }

        public static void ArgumentNumerical(string asnumber, string name)
        {
            if (!IsNumerical(asnumber)) throw new ArgumentException(
                      String.Format("Argument {0} expects a numerical value \"{1}\" is an invalid value for this object", name,
                                    asnumber));
        }

        public static bool IsNumerical(string asnumber)
        {
            try
            {
                long.Parse(asnumber);
                return true;
            }
            catch (FormatException nfe)
            {

            }
            return false;
        }

        public static bool IsEmpty<T>(T param)
        {
            return param == null || "".Equals(param) || (default(T) != null && param.Equals(default(T)));
        }

        public static bool IsEmptyList<T>(IList<T> items)
        {
            return items == null || items.Count == 0 || IsEnumerableFullOfNulls(items);
        }


        public static bool IsEmptyEnumerable<T>(IEnumerable<T> details)
        {
            return (details == null || !details.GetEnumerator().MoveNext() || IsEnumerableFullOfNulls(details));
        }

        private static bool IsEnumerableFullOfNulls<T>(IEnumerable<T> enumerable)
        {
            foreach (T t in enumerable)
            {
                if (!IsEmpty(t)) return false;
            }
            return true;
        }

        public static void ArgumentIsGreaterThan(IComparable first, IComparable second)
        {
            if (first.CompareTo(second) < 0) throw new ArgumentException(String.Format("Assertion IsGreaterThan failed  {0} is not less than {1}", first, second));
        }

        public static void ArgumentIsGreaterThanZero(IComparable arg, String name)
        {
            ArgumentIsGreaterThan(arg, (IComparable)TypeConversionUtils.ConvertValueIfNecessary(arg.GetType(), 0, "Zero"));
        }

        public static void ArgumentHasLength(IList details, string argumentname)
        {
            if (details == null || details.Count == 0) throw new ArgumentException(String.Format("Assertion failed List Argument \"{0}\" must have length", argumentname));
        }

        public static void ArgumentEnumHasLength<T>(IEnumerable<T> details, string argumentname)
        {
            if (IsEmptyEnumerable(details)) throw new ArgumentException(String.Format("Assertion failed List Argument \"{0}\" must have length", argumentname));
        }


    }
}