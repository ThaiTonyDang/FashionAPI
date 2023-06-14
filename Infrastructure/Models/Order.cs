using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string ShipAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
        public bool IsPaid { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
