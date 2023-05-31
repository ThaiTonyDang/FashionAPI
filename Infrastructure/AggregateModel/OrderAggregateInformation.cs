namespace Infrastructure.AggregateModel
{
    public class OrderAggregateInformation 
    {
        public BaseInformation BaseInformation { get; set; }
        public int OrderProductsQuantity { get; set; }
        public bool IsPaid { get; set; }
        public double Discount { get; set; }
    }
}
