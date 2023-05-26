using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.JoinItem
{
    public class OrderDetailInformation : BaseInformation
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Provider { get; set; }
        public decimal Price { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; }
    }
}
