/*using BusinessObject.Dto.Register;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class CommunityPostDAO
	{
		private static CommunityPostDAO instance = null;
		private static readonly object instanceLock = new object();

		public static CommunityPostDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new CommunityPostDAO();
					}
					return instance;
				}
			}
		}



		public async Task<CommunityPost> CreatePost(CommunityPost communityPost)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					context.CommunityPosts.Add(communityPost);
					await context.SaveChangesAsync();

					return communityPost;
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

		}

		public async Task<CommunityPost?> GetPostById(int postId)
		{
			using (var context = new HealthTrackingDBContext())
			{
				return await context.CommunityPosts.FindAsync(postId);
			}
		}


		public async Task<IEnumerable<CommunityPost>> GetPostsByTitle(string title)
		{
			using (var context = new HealthTrackingDBContext())
			{
				return await context.CommunityPosts
					.Where(p => p.Title.Contains(title) && p.Status == true) // Assuming Status=true means active
					.ToListAsync();
			}
		}


		public async Task<CommunityPost> UpdatePost(CommunityPost updatedPost)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					var post = await context.CommunityPosts.FindAsync(updatedPost.CommunityPostId);
					if (post == null) return null;

					// Update post properties
					post.Title = updatedPost.Title;
					post.Content = updatedPost.Content;
					post.ChangeBy = updatedPost.ChangeBy;
					post.ChangeDate = DateTime.Now;

					await context.SaveChangesAsync();
					return post;
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}


		public async Task<bool> SoftDeletePost(int postId)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					var post = await context.CommunityPosts.FindAsync(postId);
					if (post == null) return false;

					// Set Status to false (soft delete)
					post.Status = false;

					await context.SaveChangesAsync();
					return true;
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
		public async Task<IEnumerable<CommunityPost>> GetAllPostsAsync(int page, int pageSize)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					var posts = await context.CommunityPosts
						.Where(post => post.Status == true) // Filter active posts
						.OrderByDescending(post => post.CreateDate) // Order by create date
						.Skip((page - 1) * pageSize) // Skip the records for previous pages
						.Take(pageSize) // Take the records for the current page
						.Include(post => post.CreateByNavigation) // Include related navigation property
						.ToListAsync(); // Execute the query and convert to a list

					return posts;
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error retrieving posts: {ex.Message}", ex);
			}
		}


		public async Task<int> GetTotalPostCountAsync()
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					// Count all active posts (where Status is true)
					return await context.CommunityPosts.CountAsync(post => post.Status == true);
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}*/