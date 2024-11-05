using BusinessObject.Dto.CommunityPost;
using BusinessObject;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepo;
using BusinessObject.Dto.Comment;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly HealthTrackingDBContext _context;
        private readonly ICommentRepository _commentRepo;
        public CommentController(HealthTrackingDBContext context, ICommentRepository commentRepo)
        {

            _commentRepo = commentRepo;
            _context = context;

        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] NewCommentDTO commentDto)
        {
            if (commentDto == null)
            {
                return BadRequest("Comment data is null.");
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

            // Map DTO to Comment model
            var comment = mapper.Map<Comment>(commentDto);
            comment.CreateBy = memberId;

            // Call repository to add the comment
            await _commentRepo.CreateComment(comment);

            return Ok(comment);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDTO updatedComment)
        {
            // Check if the provided data is valid
            if (updatedComment == null)
            {
                return BadRequest("Invalid comment data.");
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

            // Retrieve the existing comment by ID
            var existingComment = await _commentRepo.GetCommentById(id); // Assuming a method to get comments by ID
            if (existingComment == null)
            {
                return NotFound("Comment not found.");
            }
            if (existingComment.CreateBy != memberId)
            {
                return Forbid("You are not authorized to update this comment."); // Return 403 Forbidden
            }
            // Update the existing comment with the new data
            existingComment.Content = updatedComment.Content; // Update the content
            existingComment.ChangeBy = memberId; // Set the user making the update
            existingComment.ChangeDate = DateTime.Now; // Set the change date

            // Call repository to update the comment in the database
            await _commentRepo.UpdateComment(existingComment);

            return NoContent(); // Return 204 status on successful update
        }


		[HttpGet("post/{postId:int}/comments")]
		public async Task<IActionResult> GetCommentsByPostId(int postId, [FromQuery] int? page, int pageSize)
		{
			int currentPage = page ?? 1;
			if (currentPage < 1) currentPage = 1;

			try
			{
				var totalComments = await _commentRepo.GetTotalCommentsByPostIdAsync(postId);
				int totalPages = (int)Math.Ceiling(totalComments / (double)pageSize);

				if (totalComments < pageSize) pageSize = totalComments;
				if (currentPage > totalPages && totalPages > 0) currentPage = totalPages;

				var comments = await _commentRepo.GetCommentsByPostIdAsync(postId, currentPage, pageSize);

				if (comments == null || !comments.Any())
				{
					return NotFound("No comments found for this post.");
				}

				var commentDtos = comments.Select(comment => new CommentDTO
				{
					CommentId = comment.CommentId,
					Content = comment.Content,
					CreatedBy = comment.CreateBy,
					CreatedDate = comment.CreateDate,
					ChangeBy = comment.ChangeBy,
					ChangeDate = comment.ChangeDate
				}).ToList();

				return Ok(new
				{
					Comments = commentDtos,
					TotalPages = totalPages,
					CurrentPage = currentPage,
					PageSize = pageSize
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


		[HttpGet("{id:int}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _commentRepo.GetCommentById(id);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            var commentDto = new CommentDTO
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                CreatedBy = comment.CreateBy,
                CreatedDate = comment.CreateDate, 
                ChangeBy = comment.ChangeBy,
                ChangeDate = comment.ChangeDate
            };

            return Ok(commentDto);
        }

        // make delete comment method, check if the owner post the commnet then they can delete

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            // Retrieve the member ID from claims
            var memberIdClaim = User.FindFirstValue("Id");
            if (memberIdClaim == null || !int.TryParse(memberIdClaim, out int memberId))
            {
                return Unauthorized("Member ID not found in claims or invalid.");
            }

            // Retrieve the existing comment by ID
            var existingComment = await _commentRepo.GetCommentById(id); // Assuming a method to get comments by ID
            if (existingComment == null)
            {
                return NotFound("Comment not found.");
            }

            // Check if the current user is the creator of the comment
            if (existingComment.CreateBy != memberId)
            {
                return Forbid("You are not authorized to delete this comment."); // Return 403 Forbidden
            }

            // Call repository to delete the comment from the database
            await _commentRepo.DeleteComment(id);

            return NoContent(); // Return 204 status on successful deletion
        }

    }
}
