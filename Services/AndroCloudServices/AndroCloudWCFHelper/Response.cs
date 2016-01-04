using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroCloudHelper
{
    public class Response
    {
        public string ResponseText { get; set; }
        public ResultEnum Result { get; set; }
        public Error Error { get; set; }

        public Response()
        {
            this.ResponseText = "";
            this.Result = ResultEnum.NoError;
            this.Error = new Error("OK", 0, ResultEnum.NoError);
        }

        public Response (string responseText)
        {
            this.ResponseText = responseText;
            this.Result = ResultEnum.NoError;
            this.Error = new Error("OK", 0, ResultEnum.NoError);
        }

        public Response(Error error, DataTypeEnum dataType)
        {
            this.ResponseText = SerializeHelper.Serialize<Error>(error, dataType);
            this.Result = error.Result;
            this.Error = error;
        }
    }
}
