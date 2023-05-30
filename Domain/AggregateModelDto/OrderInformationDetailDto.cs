using Domain.Dtos;

namespace Domain.AggregateModelDto
{
    public class OrderInformationDetailDto
    {
        public BaseInformationDto BaseInformationDto { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}
