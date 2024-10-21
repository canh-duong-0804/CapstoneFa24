using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Content { get; set; } = null!;
        public bool? Status { get; set; }

        public virtual Member CreateByNavigation { get; set; } = null!;
        public virtual CommunityPost Post { get; set; } = null!;
    }
}
