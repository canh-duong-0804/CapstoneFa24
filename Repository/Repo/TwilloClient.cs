using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Http;

namespace Repository.Repo
{
    public class TwilloClient : ITwilioRestClient
    {
        private readonly ITwilioRestClient _innerClient;


        public string AccountSid => _innerClient.AccountSid;

        public string Region => _innerClient.Region;

        public Twilio.Http.HttpClient HttpClient => _innerClient.HttpClient;

        public Response Request(Request request) => _innerClient.Request(request);

        public Task<Response> RequestAsync(Request request) => _innerClient.RequestAsync(request);
        public TwilloClient(IConfiguration config, System.Net.Http.HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("X-Customer-Header", "CustomerTwilloRestClient-Demo");
            _innerClient = new TwilioRestClient(
                "ACb501837d3725eb8cb338679a1ddb76c1",
               "bd0b708855d46c34e01a09fde2117fae",
                httpClient: new SystemNetHttpClient(httpClient)
                );
        }
    }
}
