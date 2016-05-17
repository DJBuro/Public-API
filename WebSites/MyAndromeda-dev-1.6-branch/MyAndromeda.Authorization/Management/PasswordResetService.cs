using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Monads;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using MyAndromeda.Data.DataAccess.Users;

namespace MyAndromeda.Authorization.Management
{

    public class PasswordResetService : IPasswordResetService 
    {
        private const string DefaultKey = "'3tara23t";
        private const string EncryptionFormat = "{0}&{1}"; 
        private readonly IUserDataService userDataService; 

        private readonly IMyAndromedaLogger logger;

        public PasswordResetService(IUserDataService userDataService, IMyAndromedaLogger logger)
        {
            this.userDataService = userDataService;
            this.logger = logger;
        }

        private string Key 
        {
            get 
            {
                return ConfigurationManager.AppSettings["MyAndromeda.Authorization.EncryptionKey"] ?? DefaultKey; 
            }
        }

        private int ValidForHours
        {
            get 
            {
                var r = ConfigurationManager.AppSettings["MyAndromeda.Authorization.EncryptionCodeValidForHours"];

                return string.IsNullOrWhiteSpace(r) ? 12 : Convert.ToInt32(r);
            }
        }

        public object GetParms(string code)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public string GetResetCode(string username)
        {
            var user = this.userDataService.GetByUserName(username);
            var encryptionCodeKey = this.Key;
            
            if (user == null)
                return string.Empty;

            encryptionCodeKey.CheckNull("encryptionCode");

            var encyptedText = Encrypt(string.Format(EncryptionFormat, user.Username, DateTime.UtcNow.Ticks), encryptionCodeKey);

            return encyptedText;
        }

        public bool VerifyCode(string text)
        {
            string decrpyted = string.Empty;
            try
            {
                decrpyted = Decrypt(text, this.Key);
                var parts = decrpyted.Split('&');
                if (parts.Length != 2)
                    return false;

                long ticks = 0;
                if (!long.TryParse(parts[1], out ticks))
                    return false;

                var timeStamp = new DateTime(ticks, DateTimeKind.Utc);

                if (timeStamp > DateTime.UtcNow.AddHours(-this.ValidForHours))
                {
                    return true;
                }

                return false;
            }
            catch (Exception e) 
            {
                this.logger.Error(e);
            }

            return false;
        }

        public string GetUserNameFromText(string text)
        {
            string decrypted = string.Empty;

            try
            {
                decrypted = Decrypt(text, this.Key);
                var parts = decrypted.Split('&');
                var userName = parts[0];

                return userName;
            }
            catch (Exception e) 
            {
                this.logger.Error(e);
            }

            return string.Empty;
        }

        private static string Encrypt(string toEncrypt, string key)
        {
            var des = new DESCryptoServiceProvider();
            var ms = new MemoryStream();

            VerifyKey(ref key);

            des.Key = HashKey(key, des.KeySize / 8);
            des.IV = HashKey(key, des.KeySize / 8);
            byte[] inputBytes = Encoding.UTF8.GetBytes(toEncrypt);

            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputBytes, 0, inputBytes.Length);
            cs.FlushFinalBlock();

            return HttpServerUtility.UrlTokenEncode(ms.ToArray());
        }

        private static string Decrypt(string toDecrypt, string key)
        {
            var des = new DESCryptoServiceProvider();
            var ms = new MemoryStream();

            VerifyKey(ref key);

            des.Key = HashKey(key, des.KeySize / 8);
            des.IV = HashKey(key, des.KeySize / 8);
            byte[] inputBytes = HttpServerUtility.UrlTokenDecode(toDecrypt);

            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputBytes, 0, inputBytes.Length);
            cs.FlushFinalBlock();

            var encoding = Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        /// <summary>
        /// Make sure key is exactly 8 characters
        /// </summary>
        /// <param name="key"></param>
        private static void VerifyKey(ref string key)
        {
            if (string.IsNullOrEmpty(key))
                key = DefaultKey;

            key = key.Length > 8 ? key.Substring(0, 8) : key;

            if (key.Length < 8)
            {
                for (int i = key.Length; i < 8; i++)
                {
                    key += DefaultKey[i];
                }
            }
        }

        private static byte[] HashKey(string key, int length)
        {
            var sha = new SHA1CryptoServiceProvider();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hash = sha.ComputeHash(keyBytes);
            byte[] truncateHash = new byte[length];
            Array.Copy(hash, 0, truncateHash, 0, length);

            return truncateHash;
        }
    }

}