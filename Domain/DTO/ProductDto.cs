using System.ComponentModel.DataAnnotations;
using Utilities.GlobalHelpers;

namespace Domain.DTO
{
    public class ProductDto
    {
        public Guid Id { set; get; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Provider is required")]
        public string Provider { set; get; }

        [PriceAttribute]
        public string Price { set; get; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, long.MaxValue, ErrorMessage = "Quantity must be positive numbers")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        public string ImageName { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
    }

    public class PriceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var product = (ProductDto)validationContext.ObjectInstance;
            var price = product.Price;
            if (string.IsNullOrEmpty(price))
            {
                return new ValidationResult("The Price must be not empty");
            }
            if (!price.IsConvertToNumber())
            {
                return new ValidationResult("The Price entered is not correct! ");
            }

            if (decimal.Parse(product.Price) < 0)
            {
                return new ValidationResult("The Price must be more than zero! ");
            }

            return ValidationResult.Success;
        }
    }
}
