using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MessageChat
    {
        public int MessageChatId { get; set; }
        public int? StaffId { get; set; }
        public int? MemberId { get; set; }
        public string? SenderType { get; set; }
        public string? MessageContent { get; set; }
        public DateTime? SentAt { get; set; }

        public virtual Member? Member { get; set; }
        public virtual staff? Staff { get; set; }
    }
}
