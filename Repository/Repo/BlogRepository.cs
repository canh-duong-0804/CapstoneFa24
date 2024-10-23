using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class BlogRepository : IBlogRepository
    {
        public Task<Blog> CreateBlogAsync(Blog blog) => BlogDAO.Instance.CreateBlogAsync(blog);

        public Task<bool> DeleteBlogAsync(int id) => BlogDAO.Instance.DeleteBlogAsync(id);


        public Task<IEnumerable<Blog>> GetAllBlogsAsync() => BlogDAO.Instance.GetAllBlogsAsync();


        public Task<Blog> GetBlogByIdAsync(int id) => BlogDAO.Instance.GetBlogByIdAsync(id);


        public Task<Blog> UpdateBlogAsync(Blog blog) => BlogDAO.Instance.UpdateBlogAsync(blog);

    }
}
