using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Bringg.Services.Implementation
{
    public class Encoder : MyAndromeda.Services.Bringg.Services.IEncoder 
    {
        public string EncodeForRequest(string input, string secretKey) 
        {
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            HMACSHA1 myhmacsha1 = new HMACSHA1(key);
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);

            return myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
        }

        public string CreateParams(IEnumerable<KeyValuePair<string, string>> keyPairs) 
        {
            string queryParams = "";

            foreach (var pair in keyPairs) 
            {
                if (queryParams.Length > 0)
                {
                    queryParams += '&';
                }
                queryParams += pair.Key + '=' + Uri.EscapeUriString(pair.Value);
            }

            return queryParams;
        }
    }
}
