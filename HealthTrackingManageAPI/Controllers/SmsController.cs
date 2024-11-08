using BusinessObject.Dto.Message;
using HealthTrackingManageAPI.NewFolder.EsmsHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.IRepo;
using System.Net.Http.Headers;
using System.Text;
using Twilio.AspNet.Common;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {

        private readonly SpeedSMSService _smsService;

        public SmsController(SpeedSMSService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost("send")]
        public IActionResult SendSMS([FromBody] SmsRequest request)
        {
            try
            {
                var response = _smsService.SendSMS(
                    request.Phones,
                    request.Content,
                    request.Type,
                    request.DeviceId
                );

                if (string.IsNullOrEmpty(response))
                {
                    return BadRequest("Gửi SMS thất bại");
                }

                return Ok(new { message = "Gửi SMS thành công", response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }


    public class SmsRequest
    {
        public string[] Phones { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public string DeviceId { get; set; }
    }


}