using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Models.Services
{
    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}