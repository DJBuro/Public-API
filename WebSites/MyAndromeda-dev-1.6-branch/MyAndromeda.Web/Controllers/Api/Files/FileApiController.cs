using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Storage;
using MyAndromeda.Storage.Models;

namespace MyAndromeda.Web.Controllers.Api.Files
{
    public class FileApiController : ApiController
    {
        private readonly IStorageService storageService;

        public FileApiController(IStorageService storageService)
        { 
            this.storageService = storageService;
        }
    }
}