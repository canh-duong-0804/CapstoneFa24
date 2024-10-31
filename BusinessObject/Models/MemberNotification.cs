using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class MemberNotification
    {
        public int MemberNotificationId { get; set; }
        public int MemberId { get; set; }
        public int NotificationId { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Notification Notification { get; set; } = null!;
    }
}
