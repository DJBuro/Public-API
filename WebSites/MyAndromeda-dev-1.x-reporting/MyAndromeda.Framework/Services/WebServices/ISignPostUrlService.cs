using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Configuration;
using MyAndromeda.Core;

namespace MyAndromeda.Framework.Services.WebServices
{
    public interface ISignPostUrlService : IDependency
    {
        IEnumerable<string> SignPostUrls { get; }
    }

    public class SignPostUrlService : ISignPostUrlService
    {
        private IEnumerable<string> signPostUrl;
        public IEnumerable<string> SignPostUrls
        {
            get
            {
                if (signPostUrl != null)
                    return signPostUrl;

                var appSettings = new System.Configuration.AppSettingsReader();
                var setting = appSettings.GetValue(AppSettingsDefinitions.SignPostUrl, typeof(string));
                var result = setting.ToString();

                var servers = result.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries).ToArray();

                signPostUrl = servers;

                return servers;
            }
        }
    }
}
