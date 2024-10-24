using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class CommunityPostCategory
    {
        public CommunityPostCategory()
        {
            CommunityPosts = new HashSet<CommunityPost>();
        }

        public int CommunityCategoryId { get; set; }
        public string? CommunityCategoryName { get; set; }

        public virtual ICollection<CommunityPost> CommunityPosts { get; set; }
    }
}
