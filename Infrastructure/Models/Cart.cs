using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Cart
    {
        public int Quantity { set; get; }
        public Guid ProductId { get; set; }
        public Product Product { set; get; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
