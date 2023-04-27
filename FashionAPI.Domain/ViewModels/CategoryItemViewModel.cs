using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FashionWebAPI.Domain.ViewModels
{
    public class CategoryItemViewModel
    {
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "CATEGORY NAME IS REQUIRED")]
        public string Name { get; set; }

        [Required(ErrorMessage = "DESCRIPTION IS REQUIRED")]
        public string Description { get; set; }

        [Required(ErrorMessage = "IMAGE UPLOAD IS REQUIRED")]
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; }
    }
}
