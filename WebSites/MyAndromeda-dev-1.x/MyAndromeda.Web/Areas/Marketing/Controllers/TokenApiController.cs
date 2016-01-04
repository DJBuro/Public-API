using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MyAndromeda.Framework.Tokens;

namespace MyAndromeda.Web.Areas.Marketing.Controllers
{
    public class TokenApiController : ApiController
    {
        private readonly ITokenService tokenService;

        public TokenApiController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [Route("api/email/tokens")]
        [HttpGet]
        public async Task<IEnumerable<Token>> List() 
        {
            return tokenService.GetAllTokens();
        }
    }
}