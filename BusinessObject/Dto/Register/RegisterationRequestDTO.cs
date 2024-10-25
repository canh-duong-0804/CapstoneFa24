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

        public double? Height { get; set; }
        public double? Weight { get; set; }
     
        public int? ExerciseLevel { get; set; }
        public string? Goal { get; set; }
        public int? DietId { get; set; }


    }
}
