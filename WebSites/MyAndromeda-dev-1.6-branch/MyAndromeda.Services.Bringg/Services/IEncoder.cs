using MyAndromeda.Core;
using System;
using System.Collections.Generic;
namespace MyAndromeda.Services.Bringg.Services
{
    public interface IEncoder : ITransientDependency
    {
        string EncodeForRequest(string input, string secretKey);
        string CreateParams(IEnumerable<KeyValuePair<string, string>> keyPairs);
    }
}
