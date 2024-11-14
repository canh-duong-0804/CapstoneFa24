using Newtonsoft.Json;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace HealthTrackingManageAPI.NewFolder.EsmsHelper
{
    public class SpeedSMSService
    {
        private readonly string _accessToken;
        private readonly string _rootURL;
        private readonly int _type;
        private readonly string _deviceId;
        private const int TYPE_BRANDNAME = 5;

        public SpeedSMSService(IConfiguration configuration)
        {
            _accessToken = configuration["SpeedSMS:AccessToken"];
            _rootURL = configuration["SpeedSMS:RootURL"];
            _type = int.Parse(configuration["SpeedSMS:Type"]);
            _deviceId = configuration["SpeedSMS:DeviceId"];
        }

        public string SendSMS(string phones, string content)
        {
            string url = _rootURL + "/sms/send";

            if (string.IsNullOrEmpty(phones))
                return "Invalid phone numbers";

            if (string.IsNullOrEmpty(content))
                return "Content is required";

            if (_type == TYPE_BRANDNAME && string.IsNullOrEmpty(_deviceId))
                return "Device ID is required for brandname SMS";

            string[] phoneNumbers = phones.Split(',');

            using (WebClient client = new WebClient())
            {
                NetworkCredential myCreds = new NetworkCredential(_accessToken, ":x");
                client.Credentials = myCreds;
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                // Build JSON payload
                StringBuilder builder = new StringBuilder();
                builder.Append("{\"to\":[");

                for (int i = 0; i < phoneNumbers.Length; i++)
                {
                    string formattedPhone = phoneNumbers[i].Trim().StartsWith("0")
                        ? "84" + phoneNumbers[i].Trim().Substring(1)
                        : phoneNumbers[i].Trim();
                    builder.Append("\"").Append(formattedPhone).Append("\"");
                    if (i < phoneNumbers.Length - 1)
                    {
                        builder.Append(",");
                    }
                }

                builder.Append("], \"content\": \"").Append(Uri.EscapeDataString(content))
                       .Append("\", \"type\":").Append(_type)
                       .Append(", \"sender\": \"").Append(_deviceId).Append("\"}");

                string json = builder.ToString();

                // Send POST request to SpeedSMS
                return client.UploadString(url, json);
            }
        }
    }
}
