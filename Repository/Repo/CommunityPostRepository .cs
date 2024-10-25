using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class CommunityPostRepository : ICommunityPostRepository
    {
        public Task<CommunityPost> CreatePost(CommunityPost communityPost) => CommunityPostDAO.Instance.CreatePost(communityPost);

        public Task<bool> SoftDeletePost(int postId) => CommunityPostDAO.Instance.SoftDeletePost(postId);

        public Task<IEnumerable<CommunityPost>> GetAllPosts() => CommunityPostDAO.Instance.GetAllPosts();

        public Task<CommunityPost?> GetPostById(int postId) => CommunityPostDAO.Instance.GetPostById(postId);

        public Task<IEnumerable<CommunityPost>> GetPostsByTitle(string title) => CommunityPostDAO.Instance.GetPostsByTitle(title);

        public Task<CommunityPost> UpdatePost(CommunityPost updatedPost) => CommunityPostDAO.Instance.UpdatePost(updatedPost);


    }
}
