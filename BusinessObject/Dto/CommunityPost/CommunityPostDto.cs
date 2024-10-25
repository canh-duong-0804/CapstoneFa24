using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.CommunityPost
{
    public class CommunityPostDto
    {
        public int PostId { get; set; }
        public string Titles { get; set; }
        public string Content { get; set; }

        /*	public int ?MemberId { get; set; }*/
        public int? CommunityCategoryId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
