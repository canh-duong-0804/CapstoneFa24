using BusinessObject.Dto.SearchFilter;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BlogDAO
    {
        private static BlogDAO instance = null;
        private static readonly object instanceLock = new object();

        public static BlogDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BlogDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Blog> CreateBlogAsync(Blog blog)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    await context.Blogs.AddAsync(blog);

                    await context.SaveChangesAsync();
                    return blog;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteBlogAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    var existingBlog = await context.Blogs.FindAsync(id);
                    if (existingBlog == null)
                    {
                        throw new Exception("Blog not found.");
                    }

                    // Cập nhật trạng thái IsDeleted thành true
                    existingBlog.Status = false;
                    await context.SaveChangesAsync();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var blogs = await context.Blogs
                        .Where(b => b.Status == true)
                        .Select(b => new Blog
                        {
                            BlogId = b.BlogId,
                            Title = b.Title,
                            CreateDate = b.CreateDate,
                            CreateBy = b.CreateBy,
                            ThumbnailBlog = b.ThumbnailBlog,
                            Status = b.Status,
                            Likes = b.Likes,
                            Dislikes = b.Dislikes,
                        })
                        .ToListAsync();

                    return blogs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving blogs: {ex.Message}", ex);
            }
        }


        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {
                    return await context.Blogs.FirstOrDefaultAsync(b => b.Status == true && b.BlogId == id); ;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Blog> UpdateBlogAsync(Blog blog)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var existingBlog = await context.Blogs.FindAsync(blog.BlogId);
                    if (existingBlog == null)
                    {
                        throw new Exception("Blog not found.");
                    }


                    existingBlog.Title = blog.Title;
                    existingBlog.Content = blog.Content;
                    existingBlog.ThumbnailBlog = blog.ThumbnailBlog;
                    existingBlog.ChangeBy = 1;
                    existingBlog.ChangeDate = DateTime.Now;





                    await context.SaveChangesAsync();
                    return existingBlog;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating blog: {ex.Message}", ex);
            }
        }



        public async Task<IEnumerable<Blog>> SearchAndFilterExerciseByIdAsync(SearchFilterObjectDTO search)
        {
            try
            {
                using (var context = new HealthTrackingDBContext())
                {

                    var blogsQuery = context.Blogs
                        .Where(b => b.Status == true);


                    if (!string.IsNullOrWhiteSpace(search.SearchName) && !string.IsNullOrWhiteSpace(search.FilterName))
                    {

                        blogsQuery = blogsQuery.Where(b =>
                            b.Title.ToLower().Contains(search.SearchName.ToLower()) &&
                            b.Category.CategoryName.ToLower() == search.FilterName.ToLower());
                    }
                    else if (!string.IsNullOrWhiteSpace(search.SearchName))
                    {

                        blogsQuery = blogsQuery.Where(b => b.Title.ToLower().Contains(search.SearchName.ToLower()));
                    }
                    else if (!string.IsNullOrWhiteSpace(search.FilterName))
                    {
                        blogsQuery = blogsQuery.Where(b => b.Category.CategoryName.ToLower() == search.FilterName.ToLower());
                    }

                    var blogs = await blogsQuery
                        .Select(b => new Blog
                        {
                            BlogId = b.BlogId,
                            Title = b.Title,
                            CreateDate = b.CreateDate,
                            CreateBy = b.CreateBy,
                            ThumbnailBlog = b.ThumbnailBlog,
                            Status = b.Status,
                            Likes = b.Likes,
                            Dislikes = b.Dislikes,
                        })
                        .ToListAsync();


                    return blogs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving blogs: {ex.Message}", ex);
            }
        }
    }
}
