using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.WebOrderingDataAccess.Domain
{
    public class PasswordResetRequest
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime RequestedDateTime { get; set; }
    }
}
