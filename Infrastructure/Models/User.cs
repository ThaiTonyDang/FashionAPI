using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Cart> CartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
