using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Files
{
    public class KendoFileModel 
    {
        public string Name	{get;set;}
        public string Path {get;set;}
        public long Size {get;set;}
        public string Type {get;set;}
    }
}