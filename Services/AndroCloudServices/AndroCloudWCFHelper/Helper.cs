using System;
using System.Web;
using System.ServiceModel.Web;
using System.IO;
using System.Net;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Configuration;

namespace AndroCloudHelper
{
    public class Helper
    {
        internal static log4net.ILog log = null;
        public static log4net.ILog Log
        {
            get
            {
                if (log == null)
                {
                    log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }

                return log;
            }
        }

        public static DataTypes GetDataTypes()
        {
            var dataTypes = new DataTypes();

            // Figure out what type of data was submitted to us
            if (WebOperationContext.Current.IncomingRequest.ContentType == null)
            {
                // Default to XML
                dataTypes.SubmittedDataType = DataTypeEnum.XML;
            }
            else
            {
                string contentTypeUpper = WebOperationContext.Current.IncomingRequest.ContentType.ToUpper();

                if (contentTypeUpper.Contains(value: "APPLICATION/XML"))
                {
                    dataTypes.SubmittedDataType = DataTypeEnum.XML;
                }
                else if (contentTypeUpper.Contains(value: "APPLICATION/JSON"))
                {
                    dataTypes.SubmittedDataType = DataTypeEnum.JSON;
                }
                else 
                {
                    dataTypes.SubmittedDataType = DataTypeEnum.XML;
                }
            }

            // Figure out what type of data was they want us to return
            if (WebOperationContext.Current.IncomingRequest.Accept == null)
            {
                dataTypes.WantsDataType = DataTypeEnum.XML;
            }
            else
            {
                string acceptsTypeUpper = WebOperationContext.Current.IncomingRequest.Accept.ToUpper();

                if (acceptsTypeUpper.Contains(value: "APPLICATION/XML"))
                {
                    dataTypes.WantsDataType = DataTypeEnum.XML;
                }
                else if (acceptsTypeUpper.Contains(value: "APPLICATION/JSON"))
                {
                    dataTypes.WantsDataType = DataTypeEnum.JSON;
                }
                else
                {
                    dataTypes.WantsDataType = DataTypeEnum.XML;
                }
            }

            return dataTypes;
        }

        public static DataTypes GetDataTypes(HttpContextBase httpContext)
        {
            var dataTypes = new DataTypes();

            // Figure out what type of data was submitted to us
            if (httpContext.Request.ContentType == null || httpContext.Request.ContentType == "")
            {
                // Default to XML
                dataTypes.SubmittedDataType = DataTypeEnum.XML;
            }
            else
            {
                string contentTypeUpper = httpContext.Request.ContentType.ToUpper();

                if (contentTypeUpper.Contains(value: "APPLICATION/XML"))
                {
                    dataTypes.SubmittedDataType = DataTypeEnum.XML;
                }
                else if (contentTypeUpper.Contains(value: "APPLICATION/JSON"))
                {
                    dataTypes.SubmittedDataType = DataTypeEnum.JSON;
                }
            }

            // Figure out what type of data was they want us to return
            if (httpContext.Request.AcceptTypes == null)
            {
                dataTypes.WantsDataType = DataTypeEnum.XML;
            }
            else
            {
                foreach (string acceptType in httpContext.Request.AcceptTypes)
                {
                    string acceptsTypeUpper = acceptType.ToUpper();

                    if (acceptsTypeUpper == "APPLICATION/XML")
                    {
                        dataTypes.WantsDataType = DataTypeEnum.XML;
                    }
                    else if (acceptsTypeUpper == "APPLICATION/JSON")
                    {
                        dataTypes.WantsDataType = DataTypeEnum.JSON;
                    }
                }
           }

            return dataTypes;
        }

        public static void FinishWebCall(DataTypeEnum wantsDataType, Response response)
        {
            if (WebOperationContext.Current != null)
            {
                switch (response.Result)
                {
                    case ResultEnum.NoError:
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                        break;

                    case ResultEnum.InternalServerError:
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                        break;

                    case ResultEnum.BadRequest:
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        break;

                    case ResultEnum.Unauthorized:
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                        break;
                }

                // Set the data type that we are returning
                if (wantsDataType == DataTypeEnum.XML)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
                }
                else if (wantsDataType == DataTypeEnum.JSON)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
                }
            }
        }

        public static HttpStatusCode GetHttpStatusCode(ResultEnum resultEnum)
        {
            HttpStatusCode httpStatusCode = System.Net.HttpStatusCode.InternalServerError;

            switch (resultEnum)
            {
                case ResultEnum.NoError:
                    httpStatusCode = HttpStatusCode.OK;
                    break;

                case ResultEnum.InternalServerError:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;

                case ResultEnum.BadRequest:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    break;

                case ResultEnum.Unauthorized:
                    httpStatusCode = HttpStatusCode.Unauthorized;
                    break;
            }

            return httpStatusCode;
        }

        public static void FinishWebCall(HttpContextBase httpContext, DataTypeEnum wantsDataType, Response response)
        {
            if (httpContext != null)
            {
                switch (response.Result)
                {
                    case ResultEnum.NoError:
                        httpContext.Response.StatusCode = 200;
                        break;

                    case ResultEnum.InternalServerError:
                        httpContext.Response.StatusCode = 500;
                        break;

                    case ResultEnum.BadRequest:
                        httpContext.Response.StatusCode = 400;
                        break;

                    case ResultEnum.Unauthorized:
                        httpContext.Response.StatusCode = 401;
                        break;
                }

                // Set the data type that we are returning
                if (wantsDataType == DataTypeEnum.XML)
                {
                    httpContext.Response.ContentType = "application/xml";
                }
                else if (wantsDataType == DataTypeEnum.JSON)
                {
                    httpContext.Response.ContentType = "application/json";
                }
            }
        }

        public static string StreamToString(Stream input)
        {
            string output = "";

            using (StreamReader streamReader = new StreamReader(input, Encoding.UTF8))
            {
                if (streamReader != null)
                {
                    output = streamReader.ReadToEnd();
                }
            }

            return output;
        }

        public static string GetClientIPAddressPortString()
        {
            string result = "";
            if (OperationContext.Current != null)
            {
                MessageProperties messageProperties = OperationContext.Current.IncomingMessageProperties;
                var endpointProperty = (RemoteEndpointMessageProperty)messageProperties[RemoteEndpointMessageProperty.Name];

                result = endpointProperty.Address;// +":" + endpointProperty.Port.ToString();
            }

            return result;
        }

        public static string GetClientIPAddressPortString(HttpContextBase httpContext)
        {
            string result = "";
            if (httpContext != null)
            {
                result = httpContext.Request.UserHostAddress;
            }

            return result;
        }

        public static MemoryStream StringToStream(string text)
        {
            Helper.Log.Debug(text);
            byte[] returnBytes = Encoding.UTF8.GetBytes(text);
            return new MemoryStream(returnBytes, 0, returnBytes.Length);
        }


        public static Response ProcessUnhandledException(string source, Exception exception, DataTypeEnum wantsDataType)
        {
            try
            {
                Helper.Log.Error(source, exception);
            }
            catch { }

            return new Response(Errors.InternalError, wantsDataType);
        }

        public static string ProcessError(Error error)
        {
            // See if we can get the wanted type.  If not default to XML
            DataTypes dataTypes = null;
            try
            {
                dataTypes = Helper.GetDataTypes();
            }
            catch
            { 
                dataTypes = new DataTypes()
                {
                    WantsDataType = DataTypeEnum.XML
                };
            }

            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;

            // Generate the error XML/JSON
            var errorResponse = new Response(error, dataTypes.WantsDataType);
            return errorResponse.ResponseText;
        }

        public static string ProcessCatastrophicException(Exception exception)
        {
            string responseText = "";

            // See if we can get the wanted type.  If not default to XML
            DataTypes dataTypes = null;
            try
            {
                dataTypes = Helper.GetDataTypes();
            }
            catch
            {
                dataTypes = new DataTypes()
                {
                    WantsDataType = DataTypeEnum.XML
                };
            }

            // Are we in debug mode?
            string debugMode = ConfigurationManager.AppSettings["DebugMode"];
            if (debugMode != null && debugMode.Equals(value: "true", comparisonType: StringComparison.InvariantCultureIgnoreCase))
            {
                // Return the full error details
                responseText =
                    "<html>" +
                    "<body>" +
                    exception.ToString() +
                    "</body>" +
                    "</html>";
            }
            else
            {
                // Return an unhelpful "there was an error" error :)
                var errorResponse = new Response(Errors.InternalError, dataTypes.WantsDataType);
                responseText = errorResponse.ResponseText;
            }

            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

            return responseText;
        }

        public static string GetPassword()
        {
            string authorization = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

            if (authorization == null) return null;

            string[] chunks = authorization.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);

            string password = "";
            if (chunks.Length == 2)
            {
                string encodedPassword = chunks[1];
                password = DecodeFrom64(encodedPassword);
            }

            return password;
        }

        /// <summary>
        /// The method to Decode your Base64 strings
        /// </summary>
        /// <param name="encodedData">The String containing the characters to decode.</param>
        /// <returns>A String containing the results of decoding the specified sequence of bytes.</returns>
        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);

            string returnValue = Encoding.UTF8.GetString(encodedDataAsBytes);

            return returnValue;
        }
    }
}