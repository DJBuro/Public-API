using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderTracking.Models
{
    public enum ResponseErrorCodes
    {
        NoError = 0,
        InternalError = 1,
        InvalidApplicationId = 10,
        CustomerHasNoOrders = 15,
        InvalidSessionId = 20,
        UnknownCustomer = 25
    }
    public class Response
    {
        private System.Net.HttpStatusCode httpStatusCode = System.Net.HttpStatusCode.OK;
        public System.Net.HttpStatusCode HttpStatusCode { get { return this.httpStatusCode; } }
        public string ResponseJSON { get; set; }

        private ResponseErrorCodes errorCode = ResponseErrorCodes.NoError;
        public ResponseErrorCodes ErrorCode { get { return this.errorCode; } }

        public void SetError(ResponseErrorCodes errorCode)
        {
            this.errorCode = errorCode;
            this.ResponseJSON = "{\"error\":{\"errorCode\":" + ((int)errorCode).ToString() + ",\"errorMessage\":\"";

            switch (errorCode)
            {
                case ResponseErrorCodes.InternalError:
                    this.ResponseJSON += "Internal error";
                    this.httpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                    break;
                case ResponseErrorCodes.InvalidApplicationId:
                    this.ResponseJSON += "Invalid application id";
                    this.httpStatusCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                case ResponseErrorCodes.CustomerHasNoOrders:
                    this.ResponseJSON += "Customer has no orders";
                    this.httpStatusCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                case ResponseErrorCodes.InvalidSessionId:
                    this.ResponseJSON += "Invalid session Idode.BadRequest";
                    this.httpStatusCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                case ResponseErrorCodes.UnknownCustomer:
                    this.ResponseJSON += "Unknown customer";
                    this.httpStatusCode = System.Net.HttpStatusCode.BadRequest;
                    break;
            }

            this.ResponseJSON += "\"}}";
        }
    }
}