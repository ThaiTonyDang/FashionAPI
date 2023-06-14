using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class CartItemDto
    {
        public Guid UserId { get; set; }
        public int Quantity { set; get; }
        public Guid ProductId { get; set; }
    }
}
