using System;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Web;
using System.Text;

namespace AndroCloudMVCServices.ActionResults
{
    public class ServiceResult : ActionResult 
    {
        public string Data { private get; set; }
        
        public ServiceResult(string data)
        {
            this.Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpContextBase httpContextBase = context.HttpContext;
            httpContextBase.Response.Buffer = true;
            httpContextBase.Response.Clear();            
//            httpContextBase.Response.ContentType = "application/xml";

            httpContextBase.Response.Write(this.Data);
        }
    }
}