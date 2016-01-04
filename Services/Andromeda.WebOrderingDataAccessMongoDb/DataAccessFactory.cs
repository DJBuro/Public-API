using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andromeda.WebOrderingDataAccess;
using Andromeda.WebOrderingDataAccess.DataAccess;
using Andromeda.WebOrderingDataAccessMongoDb.DataAccess;

namespace Andromeda.WebOrderingDataAccessMongoDb
{
    public class DataAccessFactory : IDataAccessFactory
    {
        public string ConnectionStringOverride { get; set; }

        public IPasswordResetRequest PasswordResetRequestDataAccess
        {
            get { return new PasswordResetRequestDataAccess() { ConnectionStringOverride = this.ConnectionStringOverride }; }
            set { throw new NotImplementedException(); }
        }
    }
}
