using BusinessObject.Dto.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.IRepo;
using System.Net.Http.Headers;
using System.Text;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {

       /* private readonly ISMSService _SMSService;

        public SmsController(ISMSService SMSService)
        {
            _SMSService = SMSService;
        }
        [HttpPost]
        [ActionName("Send")]
        public async Task<IActionResult> SendAsync(MessageRessourceDto model)
        {
            try
            {
                var result = await _SMSService.SendAsync(model.Message, model.To);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
*/












        /*private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _apiSid;
        private readonly string _apiSecret;
        public SmsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.stringee.com/v1/user/sendcustommessage");
            _apiSid = "SK.0.TWlSlIcR4qK5Qm26eKJCg4HoEPNuk9Fk";
            _apiSecret = "UEQxT1NiTHYxZ1ZxTkJ2TzVYQzR4bFpTUFB2NHBVaEc=";
        }

        [HttpPost("send-sms")]
        public async Task<IActionResult> SendSms([FromBody] SmsMessage model)
        {
           
            var smsContent = new
            {
                from = model.From,
                to = model.To,
                text = model.Message
            };

            
            var content = new StringContent(JsonConvert.SerializeObject(smsContent), Encoding.UTF8, "application/json");
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiSid}:{_apiSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            try
            {
                // Gửi yêu cầu POST tới Stringee
                var response = await _httpClient.PostAsync("/send", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return Ok("Message sent successfully: " + responseContent);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Failed to send SMS: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending SMS: {ex.Message}");
            }
        }*/





    }

}
   