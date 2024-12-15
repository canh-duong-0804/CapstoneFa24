using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MessageChatDetail
{
    public class GetMessageChatDetailDTO
    {
        public int MessageChatDetailsId { get; set; }
       // public int? MessageChatId { get; set; }
        public string? SenderType { get; set; }
        public string? MessageContent { get; set; }
        public DateTime? SentAt { get; set; }


    }
}
