using AutoMapper.Execution;
using BusinessObject;
using BusinessObject.Dto.Blog;
using BusinessObject.Dto.SearchFilter;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using Repository.Repo;
using System.Reflection.Metadata;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [Authorize(Roles = "2")]
        [HttpGet("get-all-blog-for-staff")]
        public async Task<IActionResult> GetAllBlogsForStaff()
        {
            var blogs = await _blogRepository.GetAllBlogsAsync();


            if (blogs == null || !blogs.Any())
            {
                return NotFound("No blogs found.");
            }


            return Ok(blogs);
        }
        // GET: api/blogs/get-blog/{id}
        [HttpGet("get-blog/{id}")]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog = await _blogRepository.GetBlogByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        // POST: api/blogs/create-blog
        [Authorize(Roles = "2")]
        [HttpPost("create-blog")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequestDTO blog)
        {
            
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            
            int userId = int.Parse(userIdClaim.Value);

            
            var mapper = MapperConfig.InitializeAutomapper();
            var blogModel = mapper.Map<BusinessObject.Models.Blog>(blog);

            
            blogModel.CreateBy = userId;
            if (blogModel == null)
            {
                return BadRequest("Blog model cannot be null.");
            }

            await _blogRepository.CreateBlogAsync(blogModel);
            return CreatedAtAction(nameof(GetBlog), new { id = blogModel.BlogId }, blogModel);
        }

        // PUT: api/blogs/update-blog/{id}
        [HttpPut("update-blog/{id}")]
      
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] Blog blog)
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }


            int userId = int.Parse(userIdClaim.Value);


            var mapper = MapperConfig.InitializeAutomapper();
            var blogModel = mapper.Map<BusinessObject.Models.Blog>(blog);


            blogModel.CreateBy = userId;

            var existingBlog = await _blogRepository.GetBlogByIdAsync(id);
            if (existingBlog == null)
            {
                return NotFound();
            }

            await _blogRepository.UpdateBlogAsync(blogModel);
            return NoContent();
        }

        // DELETE: api/blogs/delete-blog/{id}
        [HttpDelete("delete-blog/{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var existingBlog = await _blogRepository.GetBlogByIdAsync(id);
            if (existingBlog == null)
            {
                return NotFound();
            }

            await _blogRepository.DeleteBlogAsync(id);
            return NoContent(); 
        }

        [HttpPost("search-and-filter")]
        public async Task<IActionResult> SearchAndFilterExercise([FromBody] SearchFilterObjectDTO search)
        {
            try
            {
                var exercises = await _blogRepository.SearchAndFilterExerciseByIdAsync(search);
                if (exercises == null)
                {
                    return NotFound("No exercises found matching the criteria.");
                }

                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

