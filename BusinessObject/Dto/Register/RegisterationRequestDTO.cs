namespace BusinessObject.Dto.Register
{
    public class RegisterationRequestDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string Gender { get; set; }

        public int ExecriseLevel { get; set; }
        public string Goal { get; set; }
        public string UnderLyingDisease { get; set; }
    }
}
