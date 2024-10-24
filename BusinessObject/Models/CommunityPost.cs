using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class CommunityPost
    {
        public CommunityPost()
        {
            Comments = new HashSet<Comment>();
        }

        public int CommunityPostId { get; set; }
        public int CommunityCategoryId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool? Status { get; set; }

        public virtual CommunityPostCategory CommunityCategory { get; set; } = null!;
        public virtual Member CreateByNavigation { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
