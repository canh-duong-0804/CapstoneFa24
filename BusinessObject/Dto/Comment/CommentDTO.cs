using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Comment
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string ?Content { get; set; }
        public int CreatedBy { get; set; }

        public int? ChangeBy { get; set; }

        public DateTime ?CreatedDate { get; set; }
        // You can add other properties as needed, e.g., PostId, etc.
        public DateTime ?ChangeDate { get; set; }
    }
}
