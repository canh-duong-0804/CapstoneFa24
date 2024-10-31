using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Notification
    {
        public Notification()
        {
            MemberNotifications = new HashSet<MemberNotification>();
        }

        public int NotificationId { get; set; }
        public string TitleNotification { get; set; } = null!;
        public string ContentNotification { get; set; } = null!;
        public int NotificationType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<MemberNotification> MemberNotifications { get; set; }
    }
}
