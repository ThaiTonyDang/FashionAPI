using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "CATEGORY NAME IS REQUIRED")]
        public string Name { get; set; }

        [Required(ErrorMessage = "DESCRIPTION IS REQUIRED")]
        public string Description { get; set; }

        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Slug { get; set; }
        public ICollection<CategoryDto> CategoryChildren { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public CategoryDto ParentCategory { get; set; }
    }
}
