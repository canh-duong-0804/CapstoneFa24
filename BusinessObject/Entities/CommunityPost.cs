using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class CommunityPost
    {
        public CommunityPost()
        {
            Comments = new HashSet<Comment>();
        }

        public int PostId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int ChangeBy { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual Member CreateByNavigation { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
