namespace Infrastructure.AggregateModel
{
    public class OrderAggregate 
    {
        public BaseInformation BaseInformation { get; set; }
        public int OrderProductsQuantity { get; set; }
        public double Discount { get; set; }
    }
}
