using Infrastructure.Models;
using System;

namespace API.ViewModel
{
    public class CartItemViewModel
    {
        public string UserId { get; set; }
        public int Quantity { set; get; }
        public decimal Price { get => this.Quantity * this.Product.Price; }
        public Guid ProductId { get; set; }
        public Product Product { set; get; }
    }
}
