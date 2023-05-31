using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class OrderInforDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderProductsQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShipAddress { get; set; }
        public bool IsPaid { get; set; }
    }
}
