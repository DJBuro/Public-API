using Andromeda.GPSIntegration.Bringg.APIModel;
using Andromeda.GPSIntegration.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Bringg
{
    internal class BringgAPI
    {
        

        internal string ValidateCredentials(BringgConfig bringgConfig)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            BringgIdOnly bringgIdOnly = new BringgIdOnly()
            {
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            // Call the Bringg API
            string signature = BringgHelper.GetSignature(bringgIdOnly, bringgConfig.secretKey);

            BringgIdOnlySigned bringgIdOnlySigned = new BringgIdOnlySigned()
            {
                access_token = bringgConfig.accessToken,
                timestamp = bringgIdOnly.timestamp,
                signature = signature
            };

            string queryString = BringgHelper.ConvertToQueryString(bringgIdOnlySigned);

            string url = bringgConfig.apiUrl + "/companies/" + bringgConfig.companyId + "?" + queryString;
            HttpHelper.Call(
                "GET", url, "APPLICATION/JSON", "APPLICATION/JSON", null, null, false, out httpStatusCode, out responseData);

            // Check to see if an error was returned
            if (httpStatusCode != HttpStatusCode.OK)
            {
                return "HTTP STATUS " + httpStatusCode.ToString();
            }

            if (String.IsNullOrEmpty(responseData))
            {
                return "No data returned";
            }

            JObject responseJsonObject = JObject.Parse(responseData);
            JToken successToken = responseJsonObject["success"];
            if (successToken == null)
            {
                return "Missing success attribute";
            }

            if (successToken.ToString().ToLower() != "true")
            {
                return "Success attribute was not true";
            }

            JToken companyToken = BringgHelper.GetJSONAttribute(responseData, "company");

            if (companyToken == null)
            {
                return "Missing company attribute";
            }

            JToken companyIdToken = companyToken["id"];
            if (companyIdToken == null)
            {
                return "Company id attribute missing";
            }

            if (companyIdToken.ToString() != bringgConfig.companyId.Value.ToString())
            {
                return "Company ids do not match";
            }

            return "";
        }

        internal string AddCustomer(BringgConfig bringgConfig, Model.Customer newCustomer)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            // Call the Bringg API
            BringgCustomer bringgCustomer = new BringgCustomer()
            {
                allow_login = false,
                company_id = bringgConfig.companyId.Value,
                confirmation_code = "",
                email = String.IsNullOrEmpty(newCustomer.Email) ? null : newCustomer.Email,
                external_id = "",
                lat = newCustomer.Lat,
                lng = newCustomer.Lng,
                name = newCustomer.Name,
                phone = newCustomer.Phone,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            string message = BringgHelper.GenerateMessage(bringgCustomer, bringgConfig.secretKey);

            string url = bringgConfig.apiUrl + "/customers/";

            HttpHelper.Call(
                "POST", url, "APPLICATION/JSON", "APPLICATION/JSON", null, message, false, out httpStatusCode, out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);

            ErrorHelper.LogInfo("BringgAPI.AddCustomer", "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage), "");

            if (String.IsNullOrEmpty(errorMessage))
            {
                // Get the new customer id
                JToken customerToken = BringgHelper.GetJSONAttribute(responseData, "customer");

                if (customerToken == null)
                {
                    errorMessage = "Missing customer element " + responseData;
                }
                else
                {
                    JToken idToken = BringgHelper.GetJSONAttribute((JObject)customerToken, "id");

                    if (idToken == null)
                    {
                        errorMessage = "Missing id element " + responseData;
                    }
                    else
                    {
                        newCustomer.PartnerId = idToken.ToString();
                    }
                }
            }

            return errorMessage;
        }

        internal string UpdateCustomer(BringgConfig bringgConfig, Model.Customer updateCustomer)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            // Call the Bringg API
            BringgCustomer bringgCustomer = new BringgCustomer()
            {
                allow_login = false,
                company_id = bringgConfig.companyId.Value,
                confirmation_code = "",
                email = String.IsNullOrEmpty(updateCustomer.Email) ? null : updateCustomer.Email,
                lat = updateCustomer.Lat,
                lng = updateCustomer.Lng,
                name = updateCustomer.Name,
                phone = updateCustomer.Phone,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            string message = BringgHelper.GenerateMessage(bringgCustomer, bringgConfig.secretKey);

            string url = bringgConfig.apiUrl + "/customers/" + updateCustomer.PartnerId;
            HttpHelper.Call(
                "PATCH", url, "APPLICATION/JSON", "APPLICATION/JSON", null, message, false, out httpStatusCode, out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);

            ErrorHelper.LogInfo("BringgAPI.UpdateCustomer", "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage), "");

            if (String.IsNullOrEmpty(errorMessage))
            {
                // Get the new customer id
                JToken customerToken = BringgHelper.GetJSONAttribute(responseData, "customer");

                if (customerToken == null)
                {
                    errorMessage = "Missing customer element " + responseData;
                }
                else
                {
                    JToken idToken = BringgHelper.GetJSONAttribute((JObject)customerToken, "id");

                    if (idToken == null)
                    {
                        errorMessage = "Missing id element " + responseData;
                    }
                    else
                    {
                        updateCustomer.PartnerId = idToken.ToString();
                    }
                }
            }

            return errorMessage;
        }

        internal string AddTask(BringgConfig bringgConfig, Model.Customer customerGPS, Model.Order newOrder, bool addNote, Action<string, DebugLevel> log)
        {
            string errorMessage = "";

            if (String.IsNullOrEmpty(newOrder.BringgTaskId))
            {
                // Add a new task
                errorMessage = this.AddOrder(bringgConfig, customerGPS, newOrder, addNote);
            }
            else
            {
                // Update an existing task
                errorMessage = this.UpdateOrder(bringgConfig, customerGPS, newOrder, addNote, log);
            }

            return errorMessage;
        }

        private string AddOrder(BringgConfig bringgConfig, Model.Customer customerGps, Model.Order newOrder, bool addNote)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            // Call the Bringg API

            var bringgTask = new BringgTask()
            {
                address = newOrder.Address.ToString(),
                asap = false,
                company_id = bringgConfig.companyId.Value,
                customer_id = int.Parse(customerGps.PartnerId),
                delivery_price = null,//newOrder.DeliveryFee,
                external_id = newOrder.AndromedaOrderId,
                extras = "",
                lat = newOrder.Address.Lat,
                left_to_be_paid = null, // newOrder.HasBeenPaid ? 0 : newOrder.TotalPrice,
                lng = newOrder.Address.Long,
                note = addNote
                    ? newOrder.Note
                    : null,
                place_id = null,
                price_before_tax = null, //newOrder.TotalPrice,
                priority = 1,
                scheduled_at = newOrder.ScheduledAt.ToString(format: "yyyy-MM-ddTHH:mm:ss.fffZ"),
                silent = false,
                tax_price = null, //0,
                team_id = null,
                title = "Food Order",
                total_price = null, //newOrder.TotalPrice,
                user_id = null,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            string message = BringgHelper.GenerateMessage(bringgTask, bringgConfig.secretKey);

            string url = bringgConfig.apiUrl + "/tasks/";
            HttpHelper.Call(httpMethod: "POST", url: url, contentType: "APPLICATION/JSON", accepts: "APPLICATION/JSON", headers: null, requestData: message, logOutput: false, httpStatus: out httpStatusCode, responseData: out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);

            newOrder.BringgTaskId = "";

            ErrorHelper.LogInfo(method: "BringgAPI.AddOrder", info: "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage), storeId: "");

            if (string.IsNullOrEmpty(errorMessage))
            {
                // Get the new task id
                JToken taskToken = BringgHelper.GetJSONAttribute(responseData, "task");

                if (taskToken == null)
                {
                    errorMessage = "Missing task element " + responseData;
                }
                else
                {
                    JToken idToken = BringgHelper.GetJSONAttribute((JObject)taskToken, "id");

                    if (idToken == null)
                    {
                        errorMessage = "Missing id element " + responseData;
                    }
                    else
                    {
                        newOrder.BringgTaskId = idToken.ToString();
                    }
                }
            }

            return errorMessage;
        }

        private string UpdateOrder(BringgConfig bringgConfig, Customer customerGps, Order newOrder, bool addNote, Action<string, DebugLevel> log)
        {
            
            var bringgTask = new BringgTask();
            BringgTaskDetailModel bringgDetailTask = null;
            // Get the task from Bringg
            string errorMessage = this.GetTask(bringgConfig, newOrder.BringgTaskId, out bringgTask, out bringgDetailTask);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorHelper.LogError(method: "UpdateOrder unable to find Bringg task ", message: errorMessage, storeId: newOrder.BringgTaskId.ToString());
                return errorMessage;
            }

            string notifyMessage = string.Format("Update task: {0}; add note? {1}", newOrder.BringgTaskId, addNote);
            log(notifyMessage, DebugLevel.Notify);


            // Update the task details
            bringgTask.address = newOrder.Address.ToString();
            bringgTask.external_id = newOrder.AndromedaOrderId;
            bringgTask.lat = newOrder.Address.Lat;
            bringgTask.lng = newOrder.Address.Long;
            bringgTask.note = newOrder.Note;
            bringgTask.scheduled_at = newOrder.ScheduledAt.ToString(format: "yyyy-MM-ddTHH:mm:ss.fffZ");

            // Update the task in Bringg
            errorMessage = this.UpdateTask(bringgConfig, newOrder.BringgTaskId, bringgTask, log);

            //exit early if there is a error. 
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                return errorMessage;
            }

            string updateNoteMessage = string.Empty;

            if (addNote)
            {
                
                if (!string.IsNullOrWhiteSpace(bringgTask.note))
                {
                    string message = string.Format("Add note to task: {0}!", newOrder.BringgTaskId);
                    log(message, DebugLevel.Notify);

                    updateNoteMessage = this.PostUpdateNote(bringgConfig, newOrder.BringgTaskId, bringgDetailTask, newOrder.Note);
                }
                else
                {
                    string updateReason = string.Format(format: "No task note: {0}", arg0: newOrder.BringgTaskId);

                    log(updateReason, DebugLevel.Notify);
                }

                if (!string.IsNullOrWhiteSpace(updateNoteMessage))
                {
                    string logMessage = string.Format(format: "Add note failed for task: {0}", arg0: newOrder.BringgTaskId);

                    log(logMessage, DebugLevel.Error);
                }
            }

            return errorMessage;
        }

        private string PostUpdateNote(BringgConfig bringgConfig, string bringgTaskId, BringgTaskDetailModel bringgTask, string note)
        {
            HttpStatusCode httpStatusCode;
            string responseData = string.Empty;

            var bringgNote = new BringgNote()
            {
                company_id = bringgConfig.companyId.Value,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp(),
                //   task_id = int.Parse(bringgTaskId),
                //   way_point_id = int.Parse(bringgTask.active_way_point_id),
                note = note,
                type = 0
            };

            string waypointId = "";
            BringgWaypoint waypoint = bringgTask.way_points.FirstOrDefault();
            if (waypoint != null && !string.IsNullOrEmpty(waypoint.id))
            {
                //  bringgNote.way_point_id = int.Parse(waypoint.id);
                waypointId = waypoint.id;
            }

            // Call the Bringg API
            string message = BringgHelper.GenerateMessage(bringgNote, bringgConfig.secretKey);

            ///https://developer-api.bringg.com/partner_api/tasks/:task_id/way_points/:way_point_id/notes
            string url = bringgConfig.apiUrl + "/tasks/" + bringgTaskId + "/way_points/" + waypointId + "/notes";
            //string url = bringgConfig.apiUrl + "/tasks/" + bringgNote.task_id + "/way_points/" + bringgNote.way_point_id + "/notes";

            HttpHelper.Call(httpMethod: "POST",
                url: url,
                contentType: "APPLICATION/JSON",
                accepts: "APPLICATION/JSON",
                headers: null,
                requestData: message,
                logOutput: false,
                httpStatus: out httpStatusCode,
                responseData: out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);

            ErrorHelper.LogInfo(
                method: "BringgAPI.PostUpdateNote",
                info: "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage),
                storeId: "");

            return errorMessage;
        }

        internal string GetOrAddUser(BringgConfig bringgConfig, Driver driver, Action<string, DebugLevel> log)
        {
            HttpStatusCode httpStatusCode;

            // Try and get the user
            string errorMessage = this.GetUser(bringgConfig, driver, out httpStatusCode);

            if (httpStatusCode == HttpStatusCode.NotFound)
            {
                log(arg1: "GetOrAddUser - Creating user - does not exist", arg2: DebugLevel.Notify);
                // Driver doesn't exist - we need to create it
                errorMessage = this.AddUser(bringgConfig, driver);
            }
            else if (httpStatusCode != HttpStatusCode.OK)
            {
                errorMessage = "HTTP STATUS " + httpStatusCode.ToString();
            }

            return errorMessage;
        }

        private string GetUser(BringgConfig bringgConfig, Driver driver, out HttpStatusCode httpStatusCode)
        {
            string responseData = "";
            string errorMessage = null;
            httpStatusCode = HttpStatusCode.OK;

            var bringgGetDriver = new BringgGet()
            {
                company_id = bringgConfig.companyId.Value,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            // Call the Bringg API
            string signature = BringgHelper.GetSignature(bringgGetDriver, bringgConfig.secretKey);

            var bringgGetDriverSigned = new BringgGetSigned()
            {
                company_id = bringgConfig.companyId.Value,
                access_token = bringgConfig.accessToken,
                timestamp = bringgGetDriver.timestamp,
                signature = signature
            };

            string queryString = BringgHelper.ConvertToQueryString(bringgGetDriverSigned);

            string url = bringgConfig.apiUrl + "/users/external_id/" + Uri.EscapeDataString(driver.Phone) + "?" + queryString;
            HttpHelper.Call(httpMethod: "GET", url: url, contentType: "APPLICATION/JSON", accepts: "APPLICATION/JSON", headers: null, requestData: null, logOutput: false, httpStatus: out httpStatusCode, responseData: out responseData);

            if (httpStatusCode == HttpStatusCode.OK)
            {
                // Driver found (maybe) - extract the Bringg driver id
                int? id = null;
                errorMessage = this.ExtractId(responseData, out id);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    driver.ExternalId = id;
                }
            }

            ErrorHelper.LogInfo(method: "BringgAPI.GetUser", info: "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage), storeId: "");

            return errorMessage;
        }

        private string AddUser(BringgConfig bringgConfig, Driver driver)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            var bringgUser = new BringgUser()
            {
                company_id = bringgConfig.companyId.Value,
                name = driver.Name,
                email = null,
                password = driver.Phone,
                phone = driver.Phone,
                admin = false,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            // Call the Bringg API
            string message = BringgHelper.GenerateMessage(bringgUser, bringgConfig.secretKey);

            string url = bringgConfig.apiUrl + "/users";
            HttpHelper.Call(httpMethod: "POST",
                url: url,
                contentType: "APPLICATION/JSON",
                accepts: "APPLICATION/JSON",
                headers: null, requestData: message, logOutput: false, httpStatus: out httpStatusCode, responseData: out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);

            ErrorHelper.LogInfo(method: "BringgAPI.AddUser", info: "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage), storeId: "");

            if (String.IsNullOrEmpty(errorMessage))
            {
                // Extract the Bringg driver id
                int? id = null;
                errorMessage = this.ExtractId(responseData, out id);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    driver.ExternalId = id;
                }
            }

            return errorMessage;
        }

        private string ExtractId(string responseData, out int? id)
        {
            string errorMessage = "";
            id = null;
            JToken idToken = null;

            if (String.IsNullOrEmpty(responseData))
            {
                errorMessage = "Exmpty Bringg response";
            }
            else
            {
                idToken = BringgHelper.GetJSONAttribute(responseData, "id");

                JObject responseJsonObject = JObject.Parse(responseData);

                if (idToken == null)
                {
                    errorMessage = "No id found in Bringg response " + responseData;
                }
            }

            if (String.IsNullOrEmpty(errorMessage))
            {
                if (idToken.Type == JTokenType.Integer)
                {
                    id = (Int32)idToken;
                }
            }

            return errorMessage;
        }

        public string GetTask(BringgConfig bringgConfig, string bringTaskId, out BringgTask bringgTask, out BringgTaskDetailModel bringgTaskDetail)
        {
            string responseData = "";
            string errorMessage = null;
            bringgTask = null;
            bringgTaskDetail = null;

            var bringgGet = new BringgGet()
            {
                company_id = bringgConfig.companyId.Value,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            // Call the Bringg API
            string signature = BringgHelper.GetSignature(bringgGet, bringgConfig.secretKey);

            var bringgGetSigned = new BringgGetSigned()
            {
                company_id = bringgConfig.companyId.Value,
                access_token = bringgConfig.accessToken,
                timestamp = bringgGet.timestamp,
                signature = signature
            };

            string queryString = BringgHelper.ConvertToQueryString(bringgGetSigned);

            string url = bringgConfig.apiUrl + "/tasks/" + Uri.EscapeDataString(bringTaskId) + "?" + queryString;
            HttpStatusCode httpStatusCode;
            HttpHelper.Call(httpMethod: "GET",
                url: url,
                contentType: "APPLICATION/JSON",
                accepts: "APPLICATION/JSON",
                headers: null,
                requestData: null,
                logOutput: false,
                httpStatus: out httpStatusCode,
                responseData: out responseData);

            if (httpStatusCode == HttpStatusCode.OK && !String.IsNullOrEmpty(responseData))
            {
                bringgTask = JsonConvert.DeserializeObject<BringgTask>(responseData);
                bringgTaskDetail = JsonConvert.DeserializeObject<BringgTaskDetailModel>(responseData);
            }
            else
            {
                errorMessage = "HTTP STATUS " + httpStatusCode.ToString() + (responseData == null ? "" : responseData);
            }

            ErrorHelper.LogInfo(method: "BringgAPI.GetTask", info: "url:" + url + " result:" + (errorMessage == null ? responseData : errorMessage), storeId: responseData);

            return errorMessage;
        }

        internal string UpdateTask(BringgConfig bringgConfig, string bringTaskId, BringgTask bringgTask, Action<string, DebugLevel> log)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            bringgTask.access_token = bringgConfig.accessToken;
            bringgTask.timestamp = BringgHelper.GetTimestamp();

            // Bringgs update task doesn't like these being set in the JSON
            bringgTask.company_id = bringgConfig.companyId.Value;
            bringgTask.id = null;

            // Call the Bringg API
            string message = BringgHelper.GenerateMessage(bringgTask, bringgConfig.secretKey);

            string url = bringgConfig.apiUrl + "/tasks/" + bringTaskId + "?company_id=" + bringgConfig.companyId.Value;

            HttpHelper.Call(
                httpMethod: "PATCH",
                url: url,
                contentType: "APPLICATION/JSON",
                accepts: "APPLICATION/JSON",
                headers: null,
                requestData: message,
                logOutput: false,
                httpStatus: out httpStatusCode,
                responseData: out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);

            ErrorHelper.LogInfo(method: "BringgAPI.UpdateTask", info: "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage), storeId: "");

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                log("Update task: " + bringTaskId + " failed: " + message, DebugLevel.Error);
                log(errorMessage, DebugLevel.Error);
            }
            else
            {
                log("Update task: " + bringTaskId + " completed: " + message, DebugLevel.Notify);
            }

            return errorMessage;
        }

        internal string CancelTask(BringgConfig bringgConfig, string bringTaskId)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            var bringgGet = new BringgGet()
            {
                company_id = bringgConfig.companyId.Value,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            // Call the Bringg API
            string message = BringgHelper.GenerateMessage(bringgGet, bringgConfig.secretKey);

            string url = bringgConfig.apiUrl + "/tasks/" + bringTaskId + "/cancel";
            HttpHelper.Call(httpMethod: "POST",
                url: url,
                contentType: "APPLICATION/JSON", accepts: "APPLICATION/JSON", headers: null, requestData: message, logOutput: false, httpStatus: out httpStatusCode, responseData: out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);

            ErrorHelper.LogInfo(method: "BringgAPI.CancelTask", info: "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage), storeId: "");

            return errorMessage;
        }

        internal string StartTask(BringgConfig bringgConfig, string bringTaskId)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            var bringgGet = new BringgGet()
            {
                company_id = bringgConfig.companyId.Value,
                access_token = bringgConfig.accessToken,
                timestamp = BringgHelper.GetTimestamp()
            };

            // Call the Bringg API
            string message = BringgHelper.GenerateMessage(bringgGet, bringgConfig.secretKey);

            string url = bringgConfig.apiUrl + "/tasks/" + bringTaskId + "/start";
            HttpHelper.Call(
                "POST", url, "APPLICATION/JSON", "APPLICATION/JSON", null, message, false, out httpStatusCode, out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);

            ErrorHelper.LogInfo("BringgAPI.StartTask", "url:" + url + " result:" + (errorMessage == null ? "" : errorMessage), "");

            return errorMessage;
        }

        internal string ClockDriverIn(BringgConfig bringgConfig, int? bringgDriverId)
        {
            HttpStatusCode httpStatusCode;
            string responseData = "";

            if (bringgDriverId == null) return "No driver id returned by bringg";

            if (String.IsNullOrEmpty(bringgConfig.clockInAPIUrl)) return "Missing clock in URL";

            var bringgDriverCheckIn = new BringgDriverCheckIn()
            {
                user_id = bringgDriverId.Value
            };

            // Call the Bringg API
            JsonSerializer jsonSerializer = new JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore // Don't create attributes for null values
            };
            JObject requestJson = JObject.FromObject(bringgDriverCheckIn, jsonSerializer);
            string message = requestJson.ToString(Formatting.None);

            HttpHelper.Call(
                "POST", bringgConfig.clockInAPIUrl, "APPLICATION/JSON", "APPLICATION/JSON", null, message, false, out httpStatusCode, out responseData);

            // Check to see if an error was returned
            string errorMessage = BringgHelper.CheckResponse(httpStatusCode, responseData);


            ErrorHelper.LogInfo("BringgAPI.ClockDriverIn", "url:" + bringgConfig.clockInAPIUrl + " result:" + (errorMessage == null ? "" : errorMessage), "");

            return errorMessage;
        }
    }
}
