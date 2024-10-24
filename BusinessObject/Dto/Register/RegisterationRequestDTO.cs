namespace BusinessObject.Dto.Register
{
    public class RegisterationRequestDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
     
        public bool Gender { get; set; }

   
    }
}
