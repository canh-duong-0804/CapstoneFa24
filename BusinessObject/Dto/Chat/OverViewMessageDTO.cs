using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Chat
{
    public class OverViewMessageDTO
    {
        public int ChatId { get; set; }
       
        public int MemberId { get; set; }
        public string ImageMember { get; set; }
        public string ContentStart { get; set; }

        public string MessageContent { get; set; }
    }
}
