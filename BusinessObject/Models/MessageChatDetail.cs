using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MessageChatDetail
    {
        public int MessageChatDetailsId { get; set; }
        public int? MessageChatId { get; set; }
        public string? SenderType { get; set; }
        public string? MessageContent { get; set; }
        public DateTime? SentAt { get; set; }

        public virtual MessageChat? MessageChat { get; set; }
    }
}
