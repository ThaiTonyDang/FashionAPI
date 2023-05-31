namespace Infrastructure.AggregateModel
{
    public class BaseInformation
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShipAddress { get; set; }
    }
}
