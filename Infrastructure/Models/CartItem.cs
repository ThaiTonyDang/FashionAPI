using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class CartItem
    {
        public Guid CartId { get; set; }
        public int Quantity { set; get; }
        public decimal Price { get => this.Quantity * this.Product.Price; }
        public Guid ProductId { get; set; }
        public Product Product { set; get; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
