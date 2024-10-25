using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface ICommunityPostRepository
    {
        // Create a new post
        Task<CommunityPost> CreatePost(CommunityPost communityPost);

        // Get post by ID
        Task<CommunityPost?> GetPostById(int postId);

        // Get posts by title
        Task<IEnumerable<CommunityPost>> GetPostsByTitle(string title);

        // Update an existing post
        Task<CommunityPost> UpdatePost(CommunityPost updatedPost);

        // Soft delete a post (set Status to false)
        Task<bool> SoftDeletePost(int postId);

        Task<IEnumerable<CommunityPost>> GetAllPosts();


    }
}