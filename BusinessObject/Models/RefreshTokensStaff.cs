using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class RefreshTokensStaff
    {
        public int RefreshTokensStaffId { get; set; }
        public int StaffId { get; set; }
        public string Token { get; set; } = null!;
        public string JwtId { get; set; } = null!;
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public virtual staff Staff { get; set; } = null!;
    }
}
