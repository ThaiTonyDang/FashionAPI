using Infrastructure.Models;

namespace Infrastructure.AggregateModel
{
    public class OrderAggregateDetailInformation 
    {
       public BaseInformation BaseInformation { get; set; }
       public Product Product { get; set; }
       public int Quantity { get; set; }
    }
}
