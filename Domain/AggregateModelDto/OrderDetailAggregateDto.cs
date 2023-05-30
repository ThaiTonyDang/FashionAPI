using Domain.Dtos;

namespace Domain.AggregateModelDto
{
    public class OrderDetailAggregateDto : BaseInformationDto
    {
        public ProductDto ProductDto { get; set; }
    }
}
