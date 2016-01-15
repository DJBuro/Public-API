using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Bringg
{
    internal static class BringgHelper
    {
        internal static JToken GetJSONAttribute(string json, string name)
        {
            JObject jsonObject = JObject.Parse(json);

            return GetJSONAttribute(jsonObject, name);
        }

        internal static JToken GetJSONAttribute(JObject jsonObject, string name)
        {
            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Name == name)
                {
                    // There was an error
                    return property.Value;
                }
            }

            return null;
        }

        internal static string GetTimestamp()
        {
            return ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
        }

        /// <summary>
        /// Note that the data should only be url encoded if it's an HTTP GET as we do that through the query string
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="secretKey"></param>
        /// <param name="isHTTPGET"></param>
        /// <returns></returns>
        internal static string GenerateMessage(Object requestObject, string secretKey)
        {
            // Get the message signature
            string signature = BringgHelper.GetSignature(requestObject, secretKey);

            // We need to inject the signature into the message
            // We can do this using JSON.Net
            JsonSerializer jsonSerializer = new JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore // Don't create attributes for null values
            };
            JObject requestJson = JObject.FromObject(requestObject, jsonSerializer);
            requestJson.Add("signature", signature);
            return requestJson.ToString(Formatting.None);
        }

        /// <summary>
        /// Note that the data should only be url encoded if it's an HTTP GET as we do that through the query string
        /// </summary>
        /// <param name="requestObject"></param>
        /// <param name="secretKey"></param>
        /// <param name="isHTTPGET"></param>
        /// <returns></returns>
        internal static string GetSignature(Object requestObject, string secretKey)
        {
            string signature = "";

            // We need to generate a querystring from the message to get the signature
            // Note that the message MUST not include the signature attribute (even empty)!
            string queryString = BringgHelper.ConvertToQueryString(requestObject);

            // Generate a signature using the querystring and secret key
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            HMACSHA1 hasher = new HMACSHA1(encoding.GetBytes(secretKey));
            byte[] signatureBytes = hasher.ComputeHash(encoding.GetBytes(queryString));

            // Convert the signature bytes to a string
            StringBuilder signatureStringBuilder = new StringBuilder();
            foreach (byte signatureByte in signatureBytes)
            {
                signatureStringBuilder.Append(String.Format("{0:x2}", signatureByte));
            }
            signature = signatureStringBuilder.ToString();

            return signature;
        }

        internal static string ConvertToQueryString(object targetObject)
        {
            Type targetObjectType = targetObject.GetType();

            int index = 0;
            StringBuilder queryString = new StringBuilder();

            foreach (var property in targetObjectType.GetProperties())
            {
                object propertyValue = property.GetValue(targetObject, null);

                if (propertyValue != null)
                {
                    string propertyValueText = Uri.EscapeDataString(propertyValue.ToString());

                    if (index != 0)
                    {
                        queryString.Append("&");
                    }
                    queryString.Append(property.Name);
                    queryString.Append("=");

                    if (property.PropertyType == typeof(bool))
                    {
                        queryString.Append(propertyValueText.ToLower()); // Small fix for booleans to match JSON
                    }
                    else if (property.PropertyType == typeof(double))
                    {
                        queryString.Append(((double)propertyValue).ToString("0.0"));
                    }
                    else
                    {
                        queryString.Append(propertyValueText);
                    }

                    index++;
                }
            }

            return queryString.ToString();
        }

        internal static string CheckResponse(HttpStatusCode httpStatusCode, string responseData)
        {
            string responseText = null;
            string reason = "";

            if (!String.IsNullOrEmpty(responseData))
            {
                JObject responseJsonObject = JObject.Parse(responseData);               

                foreach (JProperty property in responseJsonObject.Properties())
                {
                    if (property.Name == "error")
                    {
                        // There was an error
                        responseText = "HTTP STATUS " + httpStatusCode.ToString() + " " + property.Value.ToString();
                    }
                    else if (property.Name == "success" && String.IsNullOrEmpty(responseText))
                    {
                        if (property.Value.ToString() == "False")
                        {
                            // There was an error
                            responseText = "Call failed with " + responseData;
                        }
                    }
                    else if (property.Name == "reason")
                    {
                        reason = property.Value.ToString();
                    }
                }
            }

            // Fudgetastic!!
            if (reason == "User already has an active shift")
            {
                responseText = null;
            }
            else if (httpStatusCode != HttpStatusCode.OK)
            {
                responseText = "HTTP STATUS " + httpStatusCode.ToString();
            }

            return responseText;
        }
    }
}
