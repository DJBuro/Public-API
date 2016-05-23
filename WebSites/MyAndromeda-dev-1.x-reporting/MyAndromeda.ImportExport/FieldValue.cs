using System;

namespace MyAndromeda.ImportExport
{
    internal class FieldValue<T> 
    {
        public FieldValue(string fieldName, Func<T, object> accessor)
        {
            this.Accessor = accessor;
            this.FieldName = fieldName;
        }

        public string FieldName { get; set; }

        private Func<T, object> Accessor { get; set; }

        public object GetValue(T item) 
        {
            return this.Accessor(item);
        }
    }
}