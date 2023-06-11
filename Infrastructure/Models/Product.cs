namespace Infrastructure.Models
{
    public class Product
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string Provider { set; get; }
        public decimal Price { set; get; }
        public string Description { get; set; }
        public int QuantityInStock { get; set; }
        public string MainImageName { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<SubImage> SubImages { get; set; }
        public ICollection<Cart> CartItems { get; set; }
    }
}
