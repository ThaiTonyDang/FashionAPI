using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggregateModelDto
{
    public class OrderAggregateDto :BaseInformationDto
    {
        public int OrderProductsQuantity { get; set; }
        public double Discount { get; set; }
    }
}
