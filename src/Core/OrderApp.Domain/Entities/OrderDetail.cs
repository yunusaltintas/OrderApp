using OrderApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.Entities
{
    public class OrderDetail : CommonEntity
    {
        public int OrderId { get; set; }
        public virtual Order  Order { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
