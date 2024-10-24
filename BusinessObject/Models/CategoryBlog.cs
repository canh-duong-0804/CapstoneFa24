using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class CategoryBlog
    {
        public CategoryBlog()
        {
            Blogs = new HashSet<Blog>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
