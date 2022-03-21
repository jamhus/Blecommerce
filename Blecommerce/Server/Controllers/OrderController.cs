using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blecommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverViewDto>>>> GetOders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }


        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<OrderDetailsDto>>> GetOrderDetails(int orderId)
        {
            var orders = await _orderService.GetOrdeDetails(orderId);
            return Ok(orders);
        }

    }
}
