namespace Domain.Dtos
{
    public class UserRegistrationDto : UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ConfirmPassword { get; init; }
    }
}