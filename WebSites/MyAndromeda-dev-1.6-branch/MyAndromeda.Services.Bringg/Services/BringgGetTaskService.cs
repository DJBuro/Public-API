using MyAndromeda.Logging;
using MyAndromeda.Services.Bringg.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Bringg.Services
{
    public class BringgGetTaskService : MyAndromeda.Services.Bringg.Services.IBringgGetTaskService
    {
        private readonly IEncoder encoder;
        private readonly IMyAndromedaLogger logger;
        
        public BringgGetTaskService(IEncoder encoder, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.encoder = encoder;
        }

        public async Task<BringgTaskModel> Get(BringgAuth auth, int taskId) 
        {
            BringgTaskModel result; 

            string route = "https://developer-api.bringg.com/partner_api/tasks/:id";
            route = route.Replace(":id", taskId.ToString());

            var keyParams = new List<KeyValuePair<string, string>>();
            keyParams.Add(new KeyValuePair<string,string>("id", taskId.ToString()));
            keyParams.Add(new KeyValuePair<string, string>("company_id", auth.CompanyId.ToString()));

            using (var client = new HttpClient()) 
            {
                var p = encoder.CreateParams(keyParams);
                var encodedP = encoder.EncodeForRequest(p, auth.SecretKey);
                
                Uri requestUrl = new Uri(route + "?" + encodedP);
                var resultMessage = await client.GetAsync(requestUrl);
                var content = await resultMessage.Content.ReadAsStringAsync();

                if (!resultMessage.IsSuccessStatusCode) 
                {
                    string error = "Getting task id failed.";
                    this.logger.Error(error);
                    this.logger.Error(route);
                    this.logger.Error(content);

                    throw new Exception(error);
                }

                result = JsonConvert.DeserializeObject<BringgTaskModel>(content);
            }

            return result;
        }

    }

}
