using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MessageChat
    {
        public MessageChat()
        {
            MessageChatDetails = new HashSet<MessageChatDetail>();
        }

        public int MessageChatId { get; set; }
        public int? StaffId { get; set; }
        public int? MemberId { get; set; }
        public int? RateStar { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? ContentStart { get; set; }

        public virtual Member? Member { get; set; }
        public virtual staff? Staff { get; set; }
        public virtual ICollection<MessageChatDetail> MessageChatDetails { get; set; }
    }
}
