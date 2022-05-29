using OrderApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.Entities
{
    public class Product : CommonEntity
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Status { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
