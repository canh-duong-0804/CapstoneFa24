using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace HealthTrackingManageAPI.NewFolder.EsmsHelper
{
    public class SpeedSMSService
    {
        private readonly string _accessToken;
        private readonly string _rootURL;
        private const int TYPE_BRANDNAME = 5;

        public SpeedSMSService(IConfiguration configuration)
        {
            _accessToken = configuration["SpeedSMS:AccessToken"];
            _rootURL = configuration["SpeedSMS:RootURL"];
        }

        public string SendSMS(string[] phones, string content, int type, string deviceId)
        {
            string url = _rootURL + "/sms/send";

            if (phones.Length <= 0)
                return "Invalid phone numbers";

            if (string.IsNullOrEmpty(content))
                return "Content is required";

            if (type == TYPE_BRANDNAME && string.IsNullOrEmpty(deviceId))
                return "Device ID is required for brandname SMS";

            using (WebClient client = new WebClient())
            {
                NetworkCredential myCreds = new NetworkCredential(_accessToken, ":x");
                client.Credentials = myCreds;
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                // Xây dựng nội dung JSON
                StringBuilder builder = new StringBuilder();
                builder.Append("{\"to\":[");

                for (int i = 0; i < phones.Length; i++)
                {
                    string formattedPhone = phones[i].StartsWith("0") ? "84" + phones[i].Substring(1) : phones[i];
                    builder.Append("\"").Append(formattedPhone).Append("\"");
                    if (i < phones.Length - 1)
                    {
                        builder.Append(",");
                    }
                }

                builder.Append("], \"content\": \"").Append(Uri.EscapeDataString(content))
                       .Append("\", \"type\":").Append(type)
                       .Append(", \"sender\": \"").Append(deviceId).Append("\"}");

                string json = builder.ToString();

                // Gửi yêu cầu POST tới SpeedSMS
                return client.UploadString(url, json);
            }
        }

    }
    public class SpeedSMSConfig
    {
        public string AccessToken { get; set; }
        public string RootURL { get; set; }
    }
}