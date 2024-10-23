using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Blog
{
    public class BlogRequestDTO
    {
       
        //public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ThumbnailBlog { get; set; }
        

    }
}
