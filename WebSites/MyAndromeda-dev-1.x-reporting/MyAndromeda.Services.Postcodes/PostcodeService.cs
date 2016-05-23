using MyAndromeda.Core;
using MyAndromeda.Services.Postcodes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Postcodes
{
    public interface IPostcodeService : IDependency
    {
        Task<Models.PostcodeDataResult> GetDataFromPostcodeAsync(string postcode);
        Task<Models.GeoLocationResult> GetLatLongFromOutwardCodeAsync(string outcode);
    }

    public class PostcodeService : IPostcodeService
    {
        //http://postcodes.io/docs
        
        public const string GetAllData = "https://api.postcodes.io/postcodes/:postcode";
        public const string PostBulkPostcodeLookup = "https://api.postcodes.io/postcodes?q=[postcode]";
        public const string GetNearestPostcodeFromLocation = "https://api.postcodes.io/postcodes?lon=:longitude&lat=:latitude";
        public const string GetFindPostcode = "https://api.postcodes.io/postcodes?q=[query]";
        public const string GetValidatePostcode = "https://api.postcodes.io/postcodes/:postcode/validate";
        public const string GetOtherPostcodesNearThisPostcode = "https://api.postcodes.io/postcodes/:postcode/nearest";
        public const string GetPostcodeAutoComplete = "https://api.postcodes.io/postcodes/:postcode/autocomplete";

        //outward code
        //Geolocation data for the centroid of the outward code specified. The outward code represents the first half of any postcode (separated by a space).
        public const string GetGeolocationForOutwardCode = "https://api.postcodes.io/outcodes/:outcode";

        public PostcodeService() 
        { 
            
        }


        public async Task<PostcodeDataResult> GetDataFromPostcodeAsync(string postcode)
        {
            var route = GetAllData.Replace(":postcode", postcode); 
            var data = await this.GetResult<PostcodeDataResult>(route);

            return data;
        }

        public async Task<GeoLocationResult> GetLatLongFromOutwardCodeAsync(string outcode)
        {
            var route = GetGeolocationForOutwardCode.Replace(":outcode", outcode);
            var data = await this.GetResult<GeoLocationResult>(route);

            return data;
        }

        private async Task<TResult> GetResult<TResult>(string route)
            where TResult : class
        {
            TResult result = null;

            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync(route);

                if (!response.IsSuccessStatusCode)
                {
                    string message = string.Format("Notify - Could not call : {0}", route);
                    string errorResponseMessage = await response.Content.ReadAsStringAsync();

                    throw new WebException(message, new Exception(errorResponseMessage));
                }

                string responseMessage = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<TResult>(responseMessage);
            }

            return result;
        }

        private async Task<TResult> GetResult<TModel, TResult>(string route, TModel model)
            where TResult : class
        {
            TResult result = null;

            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.PutAsJsonAsync(route, model);

                if (!response.IsSuccessStatusCode)
                {
                    string message = string.Format("Notify - Could not call : {0}", route);
                    string errorResponseMessage = await response.Content.ReadAsStringAsync();

                    throw new WebException(message, new Exception(errorResponseMessage));
                }

                string responseMessage = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<TResult>(responseMessage);
            }

            return result;
        }
    }
}
