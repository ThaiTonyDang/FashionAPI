using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.JoinItem
{
    public class OrderInformation : BaseInformation
    {
        public int OrderProductsQuantity { get; set; }
        public bool IsPaid { get; set; }
        public double Discount { get; set; }
    }
}
