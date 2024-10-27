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

        // POST: api/communitycategory
        [HttpPost]
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

        // GET: api/communitycategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityCategoryDTO>>> GetAllCategories()
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync();
            var categoryDtos = categories.Select(c => new CommunityCategoryDTO
            {
                CommunityCategoryId = c.CommunityCategoryId,
                CommunityCategoryName = c.CommunityCategoryName
            }).ToList();

            return Ok(categoryDtos);
        }

        // GET: api/communitycategory/{id}
        [HttpGet("{id}")]
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

        // PUT: api/communitycategory/{id}
        [HttpPut("{id}")]
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

        // DELETE: api/communitycategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            await _categoryRepo.DeleteCategoryAsync(id);
            return NoContent(); // Successful deletion, no content to return
        }
    }
}
