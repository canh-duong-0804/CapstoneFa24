using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Blog.CreateBlog
{
    public class UpdateBlogRequestDTO
    {
        public int BlogId { get; set; }   
        public DateTime? ChangeDate { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ThumbnailBlog { get; set; }

    }
}
