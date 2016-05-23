using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService.Attributes
{
    //public class SendGridArrayAttribute : Attribut
    //{
    //}
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class SendGridArrayAttribute : Attribute
    {
        private readonly string fieldName;

        public SendGridArrayAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public string FieldName { get { return fieldName; } }

        public string Value<TModel>(TModel model) 
        {
            if (model is string)
            {
                return model.ToString();
            }

            var o = JsonConvert.SerializeObject(model);

            return o;
        }

    }
}
