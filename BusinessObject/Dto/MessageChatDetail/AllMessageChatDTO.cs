using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MessageChatDetail
{
    public class AllMessageChatDTO
    {
        public int MessageChatId { get; set; }
        //public int? StaffId { get; set; }
        public int? MemberId { get; set; }
        public string? ContentStart { get; set; }
        public DateTime? CreateAt { get; set; }

    }
}
