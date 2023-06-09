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
        public Guid CartId { get; set; }
        public int Quantity { set; get; }
        public decimal Price { get => this.Quantity * this.ProductDto.Price; }
        public ProductDto ProductDto { set; get; }
        public Guid UserId { get; set; }
        public UserDto UserDto { get; set; }
    }
}
