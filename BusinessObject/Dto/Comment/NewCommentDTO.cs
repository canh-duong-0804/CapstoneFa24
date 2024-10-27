using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Comment
{
    public class NewCommentDTO
    {
       /* public int CommentId { get; set; }*/
        public int PostId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
       /* public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }*/
        public string Content { get; set; } = null!;
     /*   public bool? Status { get; set; }*/

       
    }
}
