using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andromeda.WebOrderingDataAccess.DataAccess;

namespace Andromeda.WebOrderingDataAccess
{
    public interface IDataAccessFactory
    {
        IPasswordResetRequest PasswordResetRequestDataAccess { get; set; }
    }
}
