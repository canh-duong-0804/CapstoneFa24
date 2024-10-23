using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BusinessObject.Models;

namespace HealthTrackingManageAPI.NewFolder
{
    public class RefreshToken
    {
       
        public Guid Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public Member mem { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
