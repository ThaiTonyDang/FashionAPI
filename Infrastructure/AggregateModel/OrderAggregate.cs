namespace Infrastructure.AggregateModel
{
    public class OrderAggregate 
    {
        public BaseModel BaseInformation { get; set; }
        public int OrderProductsQuantity { get; set; }
    }
}
