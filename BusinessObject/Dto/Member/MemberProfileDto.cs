using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.Member
{
	public class MemberProfileDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string ImageMember { get; set; }
		public DateTime Dob { get; set; }
		//public bool Gender { get; set; }
		public float Height { get; set; }
		public double Weight { get; set; }
      

    }

}