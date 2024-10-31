
using Microsoft.Extensions.Options;
using Repository.IRepo;
using Repository.NewFolder;

using Twilio;

using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Repository.Repo
{
    public class SMSService : ISMSService
    {
        private readonly SMSSetting _smsSetting;

        public SMSService(IOptions<SMSSetting> smsSetting)
        {
            this._smsSetting = smsSetting.Value;
        }
        public async Task<MessageResource> SendAsync(string message, string to)
        {
            TwilioClient.Init(_smsSetting.AccountSID, _smsSetting.AuthToken);

            var result = await MessageResource.CreateAsync(
                  body: message,
                  from: new PhoneNumber(_smsSetting.PhoneNumber),
                  to: new PhoneNumber(to)
              );
            return result;
        }
    }
}
        /*private readonly ITwilioRestClient _innerClient;


        public string AccountSid => _innerClient.AccountSid;

        public string Region => _innerClient.Region;

        public Twilio.Http.HttpClient HttpClient => _innerClient.HttpClient;

        public Response Request(Request request) => _innerClient.Request(request);

        public Task<Response> RequestAsync(Request request) => _innerClient.RequestAsync(request);
        public SMSService(IConfiguration config, System.Net.Http.HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("X-Customer-Header", "CustomerTwilloRestClient-Demo");
            _innerClient = new TwilioRestClient(
                "ACb501837d3725eb8cb338679a1ddb76c1",
               "bd0b708855d46c34e01a09fde2117fae",
                httpClient: new SystemNetHttpClient(httpClient)
                );
        }
    }*/

