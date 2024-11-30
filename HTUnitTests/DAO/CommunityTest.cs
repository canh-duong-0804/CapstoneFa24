using BusinessObject.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTUnitTests.DAO
{
	public class CommunityTest
	{
		private async Task<HealthTrackingDBContext> GetDatabaseContext()
		{
			var options = new DbContextOptionsBuilder<HealthTrackingDBContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			var context = new HealthTrackingDBContext(options);
			context.Database.EnsureCreated();
			
			if (await context.CommunityPosts.CountAsync() == 0)
			{
				context.CommunityPosts.Add(new CommunityPost
				{
					Title = "Test Post",
					Content = "This is a test post",
					Status = true,
					CreateBy = 1,
					CreateDate = DateTime.Now
				});
				await context.SaveChangesAsync();
			}

			return context;
		}

		/*[Fact]
		public async void CommunityPostDAO_CreatePost_ReturnsCreatedPost()
		{
			// Arrange
			var dbContext = await GetDatabaseContext();
			var dao = CommunityPostDAO.Instance;
			var newPost = new CommunityPost
			{
				Title = "New Post",
				Content = "Content of new post",
				Status = true,
				CreateBy = 1,
				CreateDate = DateTime.Now
			};

			// Act
			var result = await dao.CreatePost(newPost);

			// Assert
			Assert.NotNull(result);

		}*/
		
		[Fact]
		public async void CommunityPostDAO_CreatePost1_ReturnsCreatedPost()
		{
			// Arrange
			var dbContext = await GetDatabaseContext();
			var dao = CommunityPostDAO.Instance;

			dbContext.CommunityPosts.ToList().Count();

			// Act
			var result = await dao.GetPostById(1);

			// Assert
			Assert.NotNull(result);

		}
	}
}
