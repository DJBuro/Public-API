using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using MyAndromeda.Data.Model.MyAndromeda;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MyAndromeda.SendGridService.MarketingApi.Models.Contact;
using MyAndromeda.SendGridService.MarketingApi.Models.Template;
using System.Web;
using MyAndromeda.SendGridService.Attributes;
using System.Collections;

namespace MyAndromeda.SendGridService.MarketingApi
{
    public static class Extensions 
    {
        public static AddOrEditSenderAddressModel Convert(this MarketingContact entity)
        {
            return new AddOrEditSenderAddressModel()
            {
                Identity = entity.GetIdentityOfContactTemplate(), //entity.From,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                Email = entity.From,
                Name = entity.Name,
                ReplyTo = entity.ReplyTo,
                State = entity.County,
                Zip = entity.PostCode
            };
        }

        public static string GetIdentityOfContactTemplate(this MarketingContact entity) 
        {
            string name = string.Format("{0}-Marketing-Contact-{1}", entity.AndromedaSiteId, entity.From);
            return name;
        }

        public static string GetNameOfTemplate(this MarketingEventCampaign entity) 
        {
            var timestamp = System.DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss:fff");
            string name = string.Format("{0}-Marketing-Event-{1}-{2}", entity.AndromedaSiteId, entity.TemplateName, timestamp);
            return name;
        }

        public static string GetNameOfRecipientList(this MarketingEventCampaign entity, int count = 0) 
        {
            string name = count > 0 
                ? string.Format("{0}-Marketing-Event-{1}_{2}", entity.AndromedaSiteId, entity.TemplateName)
                : string.Format("{0}-Marketing-Event-{1}", entity.AndromedaSiteId, entity.TemplateName);

            return name;
        }

        public static GetRequestTemplateModel ConvertToGetTemplateModel(this MarketingEventCampaign entity) 
        {
            return new GetRequestTemplateModel()
            {
                Name = entity.GetNameOfTemplate()
            };
        }

        public static AddTemplateModel ConvertToAddModel(
            this MarketingEventCampaign entity, 
            MarketingContact contactEntity, 
            string templateName,
            string htmlTemplate) 
        {
            var model = new AddTemplateModel() 
            {
                //Html = entity.EmailTemplate,
                Identity = contactEntity.GetIdentityOfContactTemplate(),
                Name = templateName,
                Subject = entity.Subject,
                Text = string.Empty
            };

            model.Html = htmlTemplate;

            return model;
        }

        public static EditTemplateModel ConvertToEditModel(this MarketingEventCampaign entity, MarketingContact contactEntity, string templateName, string completeTemplate) 
        {
            var model = new EditTemplateModel()
            {
                //Html = entity.EmailTemplate,
                Identity = contactEntity.GetIdentityOfContactTemplate(),
                Name = templateName,
                Subject = entity.Subject,
                Text = string.Empty
            };

            model.Html = completeTemplate;

            return model;
        }


        public static async Task<HttpResponseMessage> PostAsFormPostAsync(this HttpClient client, string address, object postObject) 
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            postObject.GetType().GetProperties()
                .ToList()
                .ForEach(pi =>
                {
                    foreach (var pair in pi.AddPair(postObject))
                    { 
                        values.Add(pair);
                    }
                });

            var k = new FormUrlEncodedContent(values);

            return await client.PostAsync(address, k);
        }

        public static string CreateUrlEncodedQueryString(this HttpClient client, object postObject)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            postObject.GetType().GetProperties()
                .ToList()
                .ForEach(pi =>
                {
                    foreach (var pair in pi.AddPair(postObject)) { 
                        values.Add(pair);
                    }
                });


            var array = values
                .Select(e => string.Format("{0}={1}", HttpUtility.UrlEncode(e.Key), HttpUtility.UrlEncode(e.Value)))
                .ToArray();

            return "?" + string.Join("&", array);
        }
 

        public static IEnumerable<KeyValuePair<string, string>> AddPair(this PropertyInfo propertyInfo, object postObject) 
        {
            var arrayProperty = propertyInfo.GetCustomAttribute<SendGridArrayAttribute>();

            if(arrayProperty != null)
            {
                var o = propertyInfo.GetValue(postObject, null);
                if (o == null) { yield break; }

                if (o is IEnumerable) 
                {
                    var items = o as IEnumerable;
                    foreach (var item in items) 
                    {
                        var arrayValue = arrayProperty.Value(item);
                        yield return new KeyValuePair<string, string>(arrayProperty.FieldName, arrayValue);
                    }
                }
                yield break;
            }

            var jsonProperty = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
            var value = propertyInfo.GetValue(postObject, null);

            if (value == null) { yield break; }

            if (jsonProperty == null)
            {
                yield return new KeyValuePair<string, string>(propertyInfo.Name, value.ToString());
                yield break;
            }

            yield return new KeyValuePair<string,string>(jsonProperty.PropertyName, value.ToString());
            yield break;
        }
    }
}