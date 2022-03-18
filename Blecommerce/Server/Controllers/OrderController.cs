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

        [HttpPost]

        public async Task <ActionResult<ServiceResponse<bool>>> AddOrder()
        {
            var result = await _orderService.AddOrder();
            return Ok(result);
        }
    }
}
