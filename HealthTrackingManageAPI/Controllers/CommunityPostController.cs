/*using AutoMapper;
using BusinessObject;
using BusinessObject.Dto.CommunityPost;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HealthTrackingManageAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommunityPostController : ControllerBase
	{
		private readonly HealthTrackingDBContext _context;
		private readonly ICommunityPostRepository _postRepo;

		public CommunityPostController(HealthTrackingDBContext context, ICommunityPostRepository postRepo)
		{
			_postRepo = postRepo;
			_context = context;
		}

		// GET: api/communitypost/user/list - User: Get all community posts with pagination
		[HttpGet("user/list")]
		public async Task<IActionResult> GetAllPostUser([FromQuery] int? page)
		{
			int currentPage = page ?? 1;
			if (currentPage < 1) currentPage = 1;

			int currentPageSize = 5; // Page size can be adjusted as needed
			int totalPosts = await _postRepo.GetTotalPostCountAsync();
			int totalPages = (int)Math.Ceiling(totalPosts / (double)currentPageSize);

			if (totalPosts < currentPageSize) currentPageSize = totalPosts;
			if (currentPage > totalPages && totalPages > 0) currentPage = totalPages;

			var posts = await _postRepo.GetAllPostsAsync(currentPage, currentPageSize);
			var mapper = MapperConfig.InitializeAutomapper();
			var postDtos = mapper.Map<List<CommunityPostDto>>(posts);

			return Ok(new
			{
				Posts = postDtos,
				TotalPages = totalPages,
				CurrentPage = currentPage,
				PageSize = currentPageSize
			});
		}

		// POST: api/communitypost/user/create - User: Create a new community post
		[Authorize]
		[HttpPost("user/create")]
		public async Task<IActionResult> AddPost([FromBody] NewCommunityPostDto postDto)
		{
			if (postDto == null)
			{
				return BadRequest("Post data is null.");
			}

			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
			{
				return Unauthorized("Member ID not found in claims.");
			}

			if (!int.TryParse(memberIdClaim, out int memberId))
			{
				return BadRequest("Invalid member ID.");
			}

			var mapper = MapperConfig.InitializeAutomapper();
			var communityPost = mapper.Map<CommunityPost>(postDto);
			communityPost.CreateBy = memberId; // Set the creator of the post

			await _postRepo.CreatePost(communityPost);

			return Ok(communityPost);
		}

		// GET: api/communitypost/user/{id} - User: Get a specific community post by ID
		[HttpGet("user/{id:int}")]
		public async Task<IActionResult> GetPostById(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var post = await _postRepo.GetPostById(id);
			if (post == null)
			{
				return NotFound();
			}
			var mapper = MapperConfig.InitializeAutomapper();
			var postDto = mapper.Map<CommunityPostDto>(post);

			return Ok(postDto);
		}

		// GET: api/communitypost/user/title/{title} - User: Get posts by title
		[HttpGet("user/title/{title}")]
		public async Task<IActionResult> GetPostsByTitle(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				return BadRequest("Title cannot be empty.");
			}

			var posts = await _postRepo.GetPostsByTitle(title);

			if (posts == null || !posts.Any())
			{
				return NotFound("No posts found with the specified title.");
			}

			var mapper = MapperConfig.InitializeAutomapper();
			var postDtos = mapper.Map<List<CommunityPostDto>>(posts);

			return Ok(postDtos);
		}

		// PUT: api/communitypost/user/update/{id} - User: Update a community post
		[Authorize]
		[HttpPut("user/update/{id:int}")]
		public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDTO updatePostDto)
		{
			if (updatePostDto == null)
			{
				return BadRequest("Invalid post data.");
			}

			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
			{
				return Unauthorized("Member ID not found in claims.");
			}

			if (!int.TryParse(memberIdClaim, out int memberId))
			{
				return BadRequest("Invalid member ID.");
			}

			var existingPost = await _postRepo.GetPostById(id);
			if (existingPost == null)
			{
				return NotFound("Post not found.");
			}

			existingPost.Title = updatePostDto.Title;
			existingPost.Content = updatePostDto.Content;
			existingPost.ChangeBy = memberId;
			existingPost.ChangeDate = DateTime.Now;

			await _postRepo.UpdatePost(existingPost);

			return NoContent();
		}

		// DELETE: api/communitypost/user/delete/{id} - User: Soft delete a community post
		[Authorize]
		[HttpDelete("user/delete/{id:int}")]
		public async Task<IActionResult> DeletePost(int id)
		{
			if (id <= 0)
			{
				return BadRequest("Invalid post ID.");
			}

			var post = await _postRepo.GetPostById(id);
			if (post == null)
			{
				return NotFound("Post not found.");
			}

			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
			{
				return Unauthorized("Member ID not found in claims.");
			}

			if (!int.TryParse(memberIdClaim, out int memberId))
			{
				return BadRequest("Invalid member ID.");
			}

			if (post.CreateBy != memberId)
			{
				return Forbid("You are not authorized to delete this post.");
			}

			await _postRepo.SoftDeletePost(id);

			return NoContent();
		}
	}
}
*/