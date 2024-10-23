using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class RefreshTokensMember
    {
        public int RefreshTokensMemberId { get; set; }
        public int MemberId { get; set; }
        public string Token { get; set; } = null!;
        public string JwtId { get; set; } = null!;
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
