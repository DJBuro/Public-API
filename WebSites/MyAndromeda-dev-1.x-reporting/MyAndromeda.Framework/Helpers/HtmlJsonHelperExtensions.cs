using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyAndromeda.Framework.Helpers
{
    public static class HtmlJsonHelperExtensions
    {
        public static HtmlString ToJson<TViewModel>(this HtmlHelper<TViewModel> html, object model,
            bool lowercaseName = true,
            bool indent = false)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var indentFormat = indent ? Formatting.Indented : Formatting.None;

            return new HtmlString(JsonConvert.SerializeObject(model, indentFormat, settings));
        }

        public static HtmlString ToJson<TViewModel, TOriginalModel, TJsonModel>(this HtmlHelper<TViewModel> html,
            TOriginalModel model,
            Func<TOriginalModel, TJsonModel> convertModel,
            bool lowercaseName = true,
            bool index = false)
        {
            var jsonModel = convertModel(model);

            return ToJson(html, jsonModel);
        }

        public static HtmlString ToJsonFromArray<TViewModel, TOriginalModel, TJsonModel>(this HtmlHelper<TViewModel> html,
            IEnumerable<TOriginalModel> model,
            Func<TOriginalModel, TJsonModel> convertModel,
            bool lowercaseName = true,
            bool indent = false)
        {
            List<TJsonModel> jsonList = new List<TJsonModel>();

            foreach (var item in model)
            {
                var tItem = convertModel(item);
                jsonList.Add(tItem);
            }

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var indentFormat = indent ? Formatting.Indented : Formatting.None;

            return new HtmlString(JsonConvert.SerializeObject(jsonList, indentFormat, settings));
        }

        public static HtmlString JsonNetToString(this object obj,
            bool lowercaseName = true,
            bool indent = false)
        {
            var settings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            };

            if (lowercaseName) { 
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            var indentFormat = indent ? Formatting.Indented : Formatting.None;

            return new HtmlString(JsonConvert.SerializeObject(obj, indentFormat, settings));
        }
    }
}