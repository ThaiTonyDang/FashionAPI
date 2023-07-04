using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class SubImagesDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public string ImageName { get; set; }
    }
}
