using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Dtos.Requests
{
    public class CreateOrderRequest
    {
        public CreateOrderRequest()
        {
            this.ProductDetails = new List<ProductDetail>();
        }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerGSM { get; set; }

        public List<ProductDetail> ProductDetails { get; set; }
    }

    public class ProductDetail
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
    }
}
