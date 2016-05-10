using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Monads;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using MyAndromeda.Data.DataAccess.Users;

namespace MyAndromeda.Authorization.Management
{
    public interface IPasswordResetService : IDependency
    {
        object GetParms(string code);

        /// <summary>
        /// Gets the reset (encrypted) code.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        string GetResetCode(string username);

        /// <summary>
        /// Verifies the encrypted code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        bool VerifyCode(string code);

        string GetUserNameFromText(string text); 
    }

}
