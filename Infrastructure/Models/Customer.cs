using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NumberDepartment { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Ward { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
