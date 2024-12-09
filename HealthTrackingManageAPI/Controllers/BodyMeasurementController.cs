using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using BusinessObject.Dto.BodyMeasurement;
using BusinessObject.Models;
using Repository.IRepo;
using System.Threading.Tasks;
using BusinessObject;
using Repository.Repo;

namespace HealthTrackingManageAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BodyMeasurementController : ControllerBase
	{
		private readonly HealthTrackingDBContext _context;
		private readonly IBodyMesurementRepository _bodyMeasurementRepo;

		public BodyMeasurementController(HealthTrackingDBContext context, IBodyMesurementRepository bodyMeasurementRepo)
		{
			_context = context;
			_bodyMeasurementRepo = bodyMeasurementRepo;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddBodyMeasurement([FromBody] BodyMeasurementDTO bodyMeasurementDto)
		{
			if (bodyMeasurementDto == null)
			{
				return BadRequest("Body measurement data is null.");
			}

			// Retrieve the member ID from claims
			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
			{
				return Unauthorized("Member ID not found in claims.");
			}

			if (!int.TryParse(memberIdClaim, out int memberId))
			{
				return BadRequest("Invalid member ID.");
			}

			// Initialize AutoMapper if needed
			var mapper = MapperConfig.InitializeAutomapper();

			// Map DTO to BodyMeasureChange model
			var bodyMeasurement = mapper.Map<BodyMeasureChange>(bodyMeasurementDto);
			bodyMeasurement.MemberId = memberId; // Set the MemberId from claims

			// Call repository to add the body measurement
			await _bodyMeasurementRepo.CreateMeasurementAsync(bodyMeasurement);

			return Ok(bodyMeasurement);
		}

		/*[Authorize]
		[HttpGet("member/{memberId}")]
		public async Task<IActionResult> GetBodyMeasurementsByMemberId(int memberId)
		{
			var measurements = await _bodyMeasurementRepo.GetMeasurementsByMemberIdAsync(memberId);
			if (measurements == null || !measurements.Any())
			{
				return NotFound("No body measurements found for this member.");
			}

			return Ok(measurements);
		}

		[Authorize]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBodyMeasurement(int id, [FromBody] BodyMeasurementDTO bodyMeasurementDto)
		{
			if (bodyMeasurementDto == null)
			{
				return BadRequest("Body measurement data is null.");
			}

			// Retrieve the member ID from claims
			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
			{
				return Unauthorized("Member ID not found in claims.");
			}

			if (!int.TryParse(memberIdClaim, out int memberId))
			{
				return BadRequest("Invalid member ID.");
			}

			// Fetch the existing measurement
			var existingMeasurement = await _bodyMeasurementRepo.GetMeasurementByIdAsync(id);
			if (existingMeasurement == null || existingMeasurement.MemberId != memberId)
			{
				return NotFound("Body measurement not found or does not belong to the member.");
			}

			// Initialize AutoMapper if needed
			var mapper = MapperConfig.InitializeAutomapper();

			// Map DTO to BodyMeasureChange model
			var bodyMeasurement = mapper.Map(bodyMeasurementDto, existingMeasurement); // Update existing measurement

			// Call repository to update the body measurement
			await _bodyMeasurementRepo.UpdateMeasurementAsync(bodyMeasurement);

			return Ok(bodyMeasurement);
		}*/

	}
}
