using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IBlogRepository
    { 
        Task<Blog> CreateBlogAsync(Blog blog);

       
        Task<IEnumerable<Blog>> GetAllBlogsAsync();

        
        Task<Blog> GetBlogByIdAsync(int id);

      
        Task<Blog> UpdateBlogAsync(Blog blog);

       
        Task<bool> DeleteBlogAsync(int id);
    }
}
