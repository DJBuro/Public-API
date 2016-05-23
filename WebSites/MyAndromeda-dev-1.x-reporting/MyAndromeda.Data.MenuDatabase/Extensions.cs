using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace MyAndromeda.Data.MenuDatabase
{
    public static class Extensions
    {
        public static TValue Value<TValue,TModel>(this 
            TModel row, 
            Expression<Func<TModel, TValue>> predicate, 
            TValue defaultValue)
            where TModel : DataRow
        {
            MemberExpression mx = (MemberExpression)predicate.Body;

            var columnName = mx.Member.Name;

            if (row.IsNull(columnName)) 
            {
                return defaultValue;//default(TValue);
            }

            if (columnName.IndexOf("_") == 0) 
            {
                columnName.Substring(1, columnName.Length - 1);
            }

            if (row[columnName] != null) 
            {
                return row.Field<TValue>(columnName);
            }

            columnName= columnName.Replace("_", "");

            if (row[columnName] != null)
            {
                return row.Field<TValue>(columnName);
            }

            throw new MissingMemberException("Matt cant find a column name:" + columnName);
        }

        public static TValue Value<TValue, TModel>(this 
            TModel row,
            string columnName,
            TValue defaultValue)
            where TModel : DataRow
        {
            if (row.IsNull(columnName))
            {
                return defaultValue;//default(TValue);
            }

            if (columnName.IndexOf("_") == 0)
            {
                columnName.Substring(1, columnName.Length - 1);
            }

            if (row[columnName] != null)
            {
                return row.Field<TValue>(columnName);
            }

            columnName = columnName.Replace("_", "");

            if (row[columnName] != null)
            {
                return row.Field<TValue>(columnName);
            }

            throw new MissingMemberException("Matt cant find a column name:" + columnName);
        }

    }
}
