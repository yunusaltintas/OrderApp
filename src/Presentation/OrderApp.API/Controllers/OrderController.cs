using Microsoft.AspNetCore.Mvc;
using OrderApp.Application.Dtos.BaseResponse;
using OrderApp.Application.Dtos.Requests;
using OrderApp.Application.Filters;
using OrderApp.Application.Interfaces.IService;
using System.Net;

namespace OrderApp.API.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMessageProducer _messagePublisher;

        public OrderController(IOrderService orderService, IMessageProducer messagePublisher)
        {
            _orderService = orderService;
            _messagePublisher = messagePublisher;
        }

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<CustomResponseDto<OrderDto>>> CreateOrder(CreateOrderRequest request)
        {
            var id = await _orderService.CreateOrderAsync(request);

            _messagePublisher.SendMessage(request);

            return new CustomResponseDto<OrderDto>().Success((int)HttpStatusCode.OK, new OrderDto { Id = id });
        }
    }
}
