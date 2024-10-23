using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ThumbnailBlog { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public bool? Status { get; set; }

        public virtual staff CreateByNavigation { get; set; } = null!;
    }
}
