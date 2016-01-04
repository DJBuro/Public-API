using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andromeda.WebOrderingDataAccess.Domain;

namespace Andromeda.WebOrderingDataAccess.DataAccess
{
    public interface IPasswordResetRequest
    {
        string ConnectionStringOverride { get; set; }
        string Add(PasswordResetRequest passwordResetRequest);
        string Get(string token, out PasswordResetRequest passwordResetRequest);
    }
}
