using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Login
{
    public class LoginResponseMemberDTO
    {
        public int MemberId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool Gender { get; set; }
        public int ExerciseLevel { get; set; }
        public string Goal { get; set; } = null!;
        public int? DietId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Status { get; set; }
    }
}
