using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyAndromeda.Services.Ibs.Models
{
    public class TokenResult 
    {
        public DateTime ExpiresUtc { get; set; }
        public string Token { get; set; }
    }
}
