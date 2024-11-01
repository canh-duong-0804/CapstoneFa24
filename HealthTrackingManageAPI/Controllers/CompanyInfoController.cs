using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repository.IRepo;

namespace HealthTrackingManageAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyInfoController : ControllerBase
	{
		private readonly ICompanyInfoRepository _companyInfoRepo;

		public CompanyInfoController(ICompanyInfoRepository companyInfoRepo)
		{
			_companyInfoRepo = companyInfoRepo;
		}

		// GET: api/CompanyInfo
		[HttpGet]
		public async Task<IActionResult> GetCompanyInfo()
		{
			var companyInfo = await _companyInfoRepo.GetCompanyInfoAsync();
			if (companyInfo == null)
			{
				return NotFound("Company information not found.");
			}
			return Ok(companyInfo);
		}

		// POST: api/CompanyInfo
		// This endpoint adds or updates the single company information record
		[HttpPost]
		public async Task<IActionResult> AddOrUpdateCompanyInfo([FromBody] CompanyInfo companyInfo)
		{
			if (companyInfo == null)
			{
				return BadRequest("Company information cannot be null.");
			}
			var updatedCompanyInfo = await _companyInfoRepo.AddOrUpdateCompanyInfoAsync(companyInfo);
			return Ok(updatedCompanyInfo);
		}

		// DELETE: api/CompanyInfo
		[HttpDelete]
		public async Task<IActionResult> DeleteCompanyInfo()
		{
			var isDeleted = await _companyInfoRepo.DeleteCompanyInfoAsync();
			if (!isDeleted)
			{
				return NotFound("Company information not found or already deleted.");
			}
			return Ok("Company information deleted successfully.");
		}
		
	}
}
