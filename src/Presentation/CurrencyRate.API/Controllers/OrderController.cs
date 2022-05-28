using Microsoft.AspNetCore.Mvc;
using OrderApp.Application.Dtos.Requests;
using OrderApp.Application.Interfaces.IService;

namespace OrderApp.API.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task CreateOrder(CreateOrderRequest request)
        {
           await _orderService.CreateOrderAsync(request);

        }
    }
}
