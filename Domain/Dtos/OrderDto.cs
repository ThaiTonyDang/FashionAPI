using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public string ShipAddress { get; set; }
        public bool Status { get; set; }
        public bool IsPaid { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public ICollection<OrderDetailDto> OrderDetails { get; set; }

    }
}
