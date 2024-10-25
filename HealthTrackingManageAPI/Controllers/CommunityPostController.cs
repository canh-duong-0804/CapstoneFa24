using AutoMapper;
using BusinessObject;
using BusinessObject.Dto.CommunityPost;
using BusinessObject.Dto.Member;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Repository.IRepo;
using Repository.Repo;
using System.Security.Claims;

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

		[HttpGet]
		public async Task<IActionResult> GetAllPosts()
		{
			var posts = await _postRepo.GetAllPosts();
			var mapper = MapperConfig.InitializeAutomapper();
			var postDtos = mapper.Map<List<CommunityPostDto>>(posts);

			return Ok(postDtos);
		}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddPost([FromBody] NewCommunityPostDto postDto)
		{
			if (postDto == null)
			{
				return BadRequest("Post data is null.");
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

			// Map the DTO to the model
			var mapper = MapperConfig.InitializeAutomapper();
			var communityPost = mapper.Map<CommunityPost>(postDto);
			communityPost.CreateBy = memberId; // Set the creator of the post

			// Call repository to add the post
			await _postRepo.CreatePost(communityPost);

			return Ok(communityPost);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetQuestionById(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var post =  await _postRepo.GetPostById(id);
			if (post == null)
			{
				return NotFound();
			}
			var mapper = MapperConfig.InitializeAutomapper();
			var postDto = mapper.Map<CommunityPost>(post);
			
			return Ok(postDto);
		}

		[HttpGet("title/{title}")]
		public async Task<IActionResult> GetPostsByTitle(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				return BadRequest("Title cannot be empty.");
			}

			// Retrieve posts by title using repository
			var posts = await _postRepo.GetPostsByTitle(title);

			if (posts == null || !posts.Any())
			{
				return NotFound("No posts found with the specified title.");
			}

			// Map to DTOs
			var mapper = MapperConfig.InitializeAutomapper();
			var postDtos = mapper.Map<List<CommunityPostDto>>(posts);

			return Ok(postDtos);
		}

		[Authorize]
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDTO updatePostDto)
		{
			// Check if the provided data is valid
			if (updatePostDto == null)
			{
				return BadRequest("Invalid post data.");
			}

			// Retrieve the member ID from claims
			var memberIdClaim = User.FindFirstValue("Id");
			if (memberIdClaim == null)
			{
				return Unauthorized("Member ID not found in claims.");
			}

			// Parse the member ID
			if (!int.TryParse(memberIdClaim, out int memberId))
			{
				return BadRequest("Invalid member ID.");
			}

			// Retrieve the existing post by ID
			var existingPost = await _postRepo.GetPostById(id);
			if (existingPost == null)
			{
				return NotFound("Post not found.");
			}

			// Update the existing post with the new data
			existingPost.Title = updatePostDto.Title;
			existingPost.Content = updatePostDto.Content;
			existingPost.ChangeBy = memberId; // Set the user making the update
			existingPost.ChangeDate = DateTime.Now; // Set the change date

			// Call repository to update the post in the database
			var updatedPost = await _postRepo.UpdatePost(existingPost);

			return NoContent(); // Return 204 status on successful update
		}


		[Authorize]
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeletePost(int id)
		{
			if (id <= 0)
			{
				return BadRequest("Invalid post ID.");
			}

			// Check if the post exists
			var post = await _postRepo.GetPostById(id);
			if (post == null)
			{
				return NotFound("Post not found.");
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

			// Check if the user is authorized to delete (optional based on requirements)
			if (post.CreateBy != memberId)
			{
				return Forbid("You are not authorized to delete this post.");
			}

			// Delete the post using the repository
			await _postRepo.SoftDeletePost(id);

			return NoContent(); // Return 204 on successful deletion
		}
	}
}
