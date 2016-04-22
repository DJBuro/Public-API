using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyAndromeda.Web.Controllers.Api
{
    
    public class StatusController : ApiController
    {
        [Route("status/ping")]
        public string Ping() {
            return "";
        }
    }
}
