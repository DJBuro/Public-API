using AndroAdmin.ViewModels.ApiCredentials;
using AndroAdminDataAccess.EntityFramework;
using AndroAdminDataAccess.EntityFramework.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace AndroAdmin.Services.Api
{
    public class ExternalApiService
    {
        readonly IExternalApiDataService externalApiDataService = new ExternalApiDataService();

        public ExternalApiService() { }

        public IEnumerable<ApiCredentialKeyPairViewModel> ReadSchema(ExternalApi dbItem) 
        {
            var data = string.IsNullOrWhiteSpace(dbItem.DefinitionParameters) ?
                Enumerable.Empty<ApiCredentialKeyPairViewModel>() :
                Newtonsoft.Json.JsonConvert.DeserializeObject<ApiParameters>(dbItem.DefinitionParameters);

            return data;
        }

        public void CreateSchema(ExternalApi dbItem, IEnumerable<ApiCredentialKeyPairViewModel> models)
        {
            var data = string.IsNullOrWhiteSpace(dbItem.DefinitionParameters) ?
                new ApiParameters() :
                Newtonsoft.Json.JsonConvert.DeserializeObject<ApiParameters>(dbItem.DefinitionParameters);

            foreach (var newParam in models)
            {
                data.Add(newParam);
            }

            dbItem.DefinitionParameters = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            dbItem.GenerateDefaultParameters();

            this.externalApiDataService.Update(dbItem);
        }


        public void UpdateSchema(ExternalApi dbItem, IEnumerable<ApiCredentialKeyPairViewModel> models)
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiParameters>(dbItem.DefinitionParameters);

            foreach (var param in data)
            {
                var keyPair = models.FirstOrDefault(e => e.Key == param.Key);
                if (keyPair == null) { continue; }

                param.Value = keyPair.Value;
            }

            dbItem.DefinitionParameters = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            dbItem.GenerateDefaultParameters();

            this.externalApiDataService.Update(dbItem);
        }

        public void DeleteSchema(ExternalApi dbItem, IEnumerable<ApiCredentialKeyPairViewModel> models) 
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiParameters>(dbItem.DefinitionParameters);

            foreach (var param in models)
            {
                var propertyPair = data.FirstOrDefault(e => e.Key == param.Key);
                if (propertyPair != null)
                {
                    data.Remove(propertyPair);
                }
            }

            dbItem.DefinitionParameters = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            dbItem.GenerateDefaultParameters();

            this.externalApiDataService.Update(dbItem);
        }

        public IEnumerable<ApiCredentialKeyPairViewModel> ReadStoreDeviceSchema(string deviceDefinitionField, string storeDeviceParametersField) 
        {
            //api parameters - may contain default parameters - credentials 
            dynamic apiParameters = JsonConvert.DeserializeObject(deviceDefinitionField);
            //store specific parameters - printer id, currency type etc 
            dynamic storeDeviceParameters = JsonConvert.DeserializeObject(storeDeviceParametersField ?? @"{ ""nothing"" : ""true"" }");
            JObject storeDeviceParametersList = storeDeviceParameters;
            if (apiParameters != null)
            {
                foreach (var parameterKeyPair in apiParameters)
                {
                    string key = parameterKeyPair.Key;
                    string value = parameterKeyPair.Value;

                    dynamic storeDeviceProperty = storeDeviceParametersList.Property(key);

                    yield return new ApiCredentialKeyPairViewModel()
                    {
                        Key = key,
                        Placeholder = value,
                        Value = storeDeviceProperty == null ? null : storeDeviceProperty.Value
                    };
                }
            }          

            yield break;
        }
    }

    public static class Extensions 
    {
        public static void GenerateDefaultParameters(this ExternalApi dbItem)
        {
            var data = JsonConvert.DeserializeObject<ApiParameters>(dbItem.DefinitionParameters);
            dynamic parameterObject = new ExpandoObject();
            IDictionary<string, object> allDictionary = parameterObject;

            foreach (var param in data)
            {
                if (string.IsNullOrWhiteSpace(param.Value)) 
                {
                    continue;
                }

                allDictionary.Add(param.Key, param.Value);
            }

            var parameters = JsonConvert.SerializeObject(parameterObject);
            dbItem.Parameters = parameters;
        }

        public static void GenerateParameters(this StoreDevice model, IEnumerable<ApiCredentialKeyPairViewModel> models) 
        {
            dynamic parameterObject = new ExpandoObject();
            IDictionary<string, object> allDictionary = parameterObject;
                        
            if (model.Parameters != null)
            {
                dynamic modelParameters = JsonConvert.DeserializeObject(model.Parameters);
                
                foreach (JProperty param in modelParameters)
                {
                    allDictionary.Add(param.Name, param.Value);
                }
            }           
            

            foreach (var pair in models.Where(e=> !string.IsNullOrWhiteSpace(e.Value ))) 
            {               
                if (allDictionary.ContainsKey(pair.Key))
                {
                    allDictionary[pair.Key] = pair.Value;
                    continue;
                }
                allDictionary.Add(pair.Key, pair.Value);       
            }

            var parameters = JsonConvert.SerializeObject(parameterObject);
            model.Parameters = parameters;
        }
    }
}