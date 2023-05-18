using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models
{
    public class Role : IdentityRole<Guid>
    {
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
