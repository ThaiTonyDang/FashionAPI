using Infrastructure.Models;

namespace Infrastructure.AggregateModel
{
    public class OrderDetailAggregate 
    {
       public BaseInformation BaseInformation { get; set; }
       public Product Product { get; set; }
       public int QuantityInOrder { get; set; }
    }
}
