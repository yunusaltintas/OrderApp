using OrderApp.Application.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Interfaces.IService
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderRequest request);
    }
}
