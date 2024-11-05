using BusinessObject.Dto.CommunityCategory;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthTrackingManageAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommunityCategoryController : ControllerBase
	{
		private readonly ICommunityCategoryRepo _categoryRepo;

		public CommunityCategoryController(ICommunityCategoryRepo categoryRepo)
		{
			_categoryRepo = categoryRepo;
		}

		// POST: api/communitycategory/admin/create - Admin: Create a new community category
		[HttpPost("admin/create")]
		public async Task<ActionResult<CommunityCategoryDTO>> CreateCategory([FromBody] CommunityCategoryDTO categoryDto)
		{
			if (categoryDto == null)
			{
				return BadRequest("Category data is null.");
			}

			var createdCategory = await _categoryRepo.CreateCategoryAsync(new CommunityPostCategory
			{
				CommunityCategoryName = categoryDto.CommunityCategoryName
			});

			var resultDto = new CommunityCategoryDTO
			{
				CommunityCategoryId = createdCategory.CommunityCategoryId,
				CommunityCategoryName = createdCategory.CommunityCategoryName
			};

			return CreatedAtAction(nameof(GetCategoryById), new { id = resultDto.CommunityCategoryId }, resultDto);
		}

		// GET: api/communitycategory/user/list - User: Get all community categories with pagination
		[HttpGet("user/list")]
		public async Task<ActionResult<IEnumerable<CommunityCategoryDTO>>> GetAllCategories(int pageNumber , int pageSize )
		{
			var categories = await _categoryRepo.GetAllCategoriesAsync(pageNumber, pageSize);
			var categoryDtos = categories.Select(c => new CommunityCategoryDTO
			{
				CommunityCategoryId = c.CommunityCategoryId,
				CommunityCategoryName = c.CommunityCategoryName
			}).ToList();

			return Ok(categoryDtos);
		}

		// GET: api/communitycategory/user/{id} - User: Get a specific community category by ID
		[HttpGet("user/{id}")]
		public async Task<ActionResult<CommunityCategoryDTO>> GetCategoryById(int id)
		{
			var category = await _categoryRepo.GetCategoryByIdAsync(id);

			if (category == null)
			{
				return NotFound("Category not found.");
			}

			var categoryDto = new CommunityCategoryDTO
			{
				CommunityCategoryId = category.CommunityCategoryId,
				CommunityCategoryName = category.CommunityCategoryName
			};

			return Ok(categoryDto);
		}

		// PUT: api/communitycategory/admin/update/{id} - Admin: Update a specific community category
		[HttpPut("admin/update/{id}")]
		public async Task<IActionResult> UpdateCategory(int id, [FromBody] CommunityCategoryDTO categoryDto)
		{
			if (id != categoryDto.CommunityCategoryId)
			{
				return BadRequest("Category ID mismatch.");
			}

			var existingCategory = await _categoryRepo.GetCategoryByIdAsync(id);
			if (existingCategory == null)
			{
				return NotFound("Category not found.");
			}

			existingCategory.CommunityCategoryName = categoryDto.CommunityCategoryName;
			await _categoryRepo.UpdateCategoryAsync(existingCategory);

			return NoContent(); // Successful update, no content to return
		}
	}
}
