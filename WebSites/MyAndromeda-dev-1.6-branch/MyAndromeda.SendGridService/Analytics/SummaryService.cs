using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService.Analytics.Models;
using Newtonsoft.Json.Linq;

namespace MyAndromeda.SendGridService.Analytics
{
    public interface IEmailSummaryService : IDependency 
    {
        Task<IEnumerable<Models.SummaryResult>> GetSummaryResultsAync();
    }

    public class SummaryService : IEmailSummaryService
    {
        private readonly IMyAndromedaLogger logger;

        private readonly ISendGridEmailSettings settings;

        public SummaryService(IMyAndromedaLogger logger, ISendGridEmailSettings settings)
        {
            this.logger = logger;
            this.settings = settings;
        }

        public async Task<IEnumerable<Models.SummaryResult>> GetSummaryResultsAync() 
        {
            var responseText = await this.MakeRequests();

            if(string.IsNullOrWhiteSpace(responseText))
            {
                return Enumerable.Empty<Models.SummaryResult>();
            }

            var jsonArray = JArray.Parse(responseText);

            List<SummaryResult> results = new List<SummaryResult>();
            foreach (dynamic item in jsonArray) 
            {
                var model = new Models.SummaryResult()
                {
                    Date = item.date,
                    Delivered = item.delivered,
                    Unsubscribes = item.unsubscribes,
                    InvalidEmail = item.invalid_email,
                    Bounces = item.bounces,
                    RepeatUnsubscribes = item.repeat_unsubscribes,
                    UniqueClicks = item.unique_clicks,
                    Blocked = item.blocked,
                    SpamDrop = item.spam_drop,
                    RepeatBounces = item.repeat_bounces,
                    RepeatSpamReports = item.repeat_spamreports,
                    Requests = item.requests,
                    Spamreports = item.spamreports,
                    Clicks = item.clicks,
                    Opens = item.opens,
                    UniqueOpens = item.unique_opens
                };

                results.Add(model);
            }

            return results;
        }

        private async Task<string> MakeRequests()
        {
            var  response = await RequestStatsSendgridCom();
            var responseText = string.Empty;

            if (response != null)
            {
                //Success, possibly use response.
                responseText = await Calls.ReadResponse(response);

                response.Close();
            }
            else
            {
                //Failure, cannot use response.
            }

            return responseText;
        }

        /// <summary>
        /// Tries to request the URL: https://sendgrid.com/api/stats.get.json
        /// </summary>
        /// <param name="response">After the function has finished, will possibly contain the response to the request.</param>
        /// <returns>True if the request was successful; false otherwise.</returns>
        private async Task<HttpWebResponse> RequestStatsSendgridCom()
        {
            HttpWebResponse response = null;

            try
            {
                //Create request to URL.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://sendgrid.com/api/stats.get.json");

                //Set request headers.
                request.UserAgent = "MyAndromeda";
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
                //request.Headers.Set(HttpRequestHeader.Cookie, @"sendgrid_frontend=191022830d9532e928443892d3fe3c39:4a13f1592244ac782017f591320cfb272afb588a; _ga=GA1.2.1731643200.1400236763; SnapABugHistory=1#; cvo_sid1=GEJ5E2T7VV2C; __ar_v4=DZZ73Q5VF5ABHIGNYPENCN%3A20140605%3A1%7CSBOYNCQOLNHFRAETDK23SX%3A20140604%3A2%7CUHHZRWYA6FDM7JTIFJ6VUQ%3A20140603%3A1%7C7EDD4RHZMNCFDLFJVYXFMR%3A20140603%3A3%7CJ53ALRDMEFFF7FNMCERDRA%3A20140603%3A1%7CZVEDT7DG35H77DAZW5W7GG%3A20140603%3A36%7C4KUNSVFPFNE73LJ2G56DGA%3A20140515%3A1%7CHWYX4RR7EJEBJIW3P4RQ6W%3A20140515%3A61%7CSYFMXPWDD5BYNDV7PHXGWZ%3A20140515%3A61%7CN27AJKSVQNGTFIUBGCE3YJ%3A20140515%3A2%7CL7VY5F545RFKRLXP6B6OFA%3A20140603%3A5%7C2MCFESXKERGMHGC6BFLXXL%3A20140603%3A1%7CHIRPMHJSV5GQJGPYENED3A%3A20140603%3A6%7CLQ62LOLKIBCYLEUQIZTCIJ%3A20140604%3A2; _mkto_trk=id:467-KXI-123&token:_mch-sendgrid.com-1400236763530-61668; cvo_tid1=BRQ4_2-o3Vw|1401791248|1401961583|0; sf_details=eyJtYyI6IlBhaWQgU2VhcmNoIiwibWNkIjoiQWRXb3JkcyIsImdhaWQiOiJTZW5kR3JpZC1VSy1CcmFuZCIsImt3IjoiU2VuZGdyaWQifQ%3D%3D; optimizelySegments=%7B%22174489618%22%3A%22false%22%2C%22174319933%22%3A%22ff%22%2C%22174357559%22%3A%22campaign%22%7D; optimizelyEndUserId=oeu1401789941225r0.9602604635544302; optimizelyBuckets=%7B%7D; __qca=P0-581241943-1401789941373; optimizelyCustomEvents=%7B%22oeu1401789941225r0.9602604635544302%22%3A%5B%22features_expand%22%2C%22dropdown%22%5D%7D; __cfduid=d864be8c39872c8bc2b2712c1a42eb6c61401790131552; _sendgrid_rails_session=BAh7B0kiD3Nlc3Npb25faWQGOgZFRkkiJTBlMzk4MmRkM2VlMDdiMTc2Y2JhNTE4MzRlYTMzMjUxBjsAVEkiEF9jc3JmX3Rva2VuBjsARkkiMWdBRmpBY3RQaXB3a1NZa2lYQ3BINUNRc3ZzcUV3eTRUdVkvcU1VSFhSdGs9BjsARg%3D%3D--0b8625c4effc29f8603bf703a448dcbf85f32ca9; cvo_sid1=GEJ5E2T7VV2C; cvo_tid1=BRQ4_2-o3Vw|1401791248|1401811621|1; __distillery=501986EAFE2E271D8218DCDD27C41D3A77028D6A; CW-TOKEN=iLLkcy96ZXsNm40mmGlmsTbXylt9YYc5yEE69%2FZ1do8%3D; _CodeWorkshop_session=QWhBN1RVckxoYzR2N3cydk5vV25sTE8zNzk0aU5yekF3VzhtQnZFYWFuYW1JVFZWWWRjSkx3RTFmNjE3aytDZ0hHbkZvak5IcjZlS1l0cUJ4QkJOVksvMCtaY0E4MnppVnZoOHZJdTM3c3NNditPYkhndFZDblAxamEwcDBDNmFPZkhFNTJlZng0LzZza09SV1p1aVk4NDhTbWwwcGpUTGEwd25UVVFsTGFaQlpKc29JajdjOU1Qb1lqaTJFeXFkLS1YVVd0YU9FcEkrbDJmcVlaYXQ5L2hBPT0%3D--097b3807f91d5562d2378f4d351f15a100889990; __csess=1401813121911.116YG0.; __cdrop=.BOX9EQ.; _gali=content");
                //request.Headers.Add("X-ClickOnceSupport", @"( .NET CLR 3.5.30729; .NET4.0E)");
                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.Pragma, "no-cache");
                request.Headers.Set(HttpRequestHeader.CacheControl, "no-cache");

                //Set request method
                request.Method = "POST";

                // Disable 'Expect: 100-continue' behavior. More info: http://haacked.com/archive/2004/05/15/http-web-request-expect-100-continue.aspx
                request.ServicePoint.Expect100Continue = false;

                //Set request body.
                var bodyContent = "api_user={0}&api_key={1}&days=30";
                string body =
                    //@"api_user=azure_7153bbe1d3fe12f7a885b265e563b036@azure.com&api_key=8gqjcxfr&days=30";
                    string.Format(bodyContent, settings.UserName, settings.Password);

                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                
                Stream stream = await request.GetRequestStreamAsync();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                //Get response to request.
                var webResponse = await request.GetResponseAsync();
                response = (HttpWebResponse)webResponse;
            }
            catch (WebException e)
            {
                //ProtocolError indicates a valid HTTP response, but with a non-200 status code (e.g. 304 Not Modified, 404 Not Found)
                if (e.Status == WebExceptionStatus.ProtocolError) 
                { 
                    response = (HttpWebResponse)e.Response;
                }
            }
            catch (Exception)
            {
                if (response != null)
                {
                    response.Close();
                }
                //return false;
            }

            return response;
        }
    }

    internal static class Calls 
    {
        

        //Returns the text contained in the response.  For example, the page HTML.  Only handles the most common HTTP encodings.
        internal static Task<string> ReadResponse(HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                Stream streamToRead = responseStream;
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
                }

                using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
                {
                    return streamReader.ReadToEndAsync();
                }
            }
        }

        
    }

}
