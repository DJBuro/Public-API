using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Extensions
{
    public static class JustEatErrorExtensions
    {
        public static Models.JustEat.ErrorResponse CreateErrorResponse(this Models.Services.ErrorResponse error, Models.JustEat.JustEatOrder order) 
        {
            var model = new Models.JustEat.ErrorResponse()
            {
                OrderId = order.Id,
                TimeStamp = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                Message = error.Message,
                Details = "ErrorCode: " + error.ErrorCode.ToString()
            };

            return model;
        }
    }
}