using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.CommunityPost
{
    public class UpdatePostDTO
    {
        public int CommunityPostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? ChangeBy { get; set; }
    }
}
