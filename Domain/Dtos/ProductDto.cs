using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos
{
    public class ProductDto
    {
        public Guid Id { set; get; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Provider is required")]
        public string Provider { set; get; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, long.MaxValue, ErrorMessage = "Price must be positive numbers")]
        public decimal Price { set; get; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, long.MaxValue, ErrorMessage = "Quantity must be positive numbers")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        public string MainImageName { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int QuantityInOrder { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}