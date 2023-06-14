using Domain.Dtos;
using Infrastructure.Models;
using System;

namespace Domain.ViewModel
{
    public class CartItemViewModel : CartItemDto
    {
        public Guid UserId { get; set; }
        public decimal Price { get => Quantity * Product.Price; }
        public ProductDto Product { set; get; }
    }
}
