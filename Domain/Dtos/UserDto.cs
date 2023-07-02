namespace Domain.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarImage { get; set; }
        public DateTime Birthday { get; set; }
        public string Role { get; set; }
    }
}
