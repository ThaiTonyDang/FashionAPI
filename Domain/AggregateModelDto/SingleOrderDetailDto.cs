using Domain.Dtos;

namespace Domain.AggregateModelDto
{
    public class SingleOrderDetailDto
    {
        public BaseInformationDto BaseInformationDto { get; set; }
        public List<ProductDto> ProductDtos { get; set; }
    }
}