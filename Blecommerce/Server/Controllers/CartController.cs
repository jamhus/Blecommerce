using Microsoft.AspNetCore.Mvc;

namespace Blecommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServiceBE _cartService;

        public CartController(ICartServiceBE cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("products")]

        public async Task<ActionResult<ServiceResponse<List<CartProductDto>>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = await _cartService.GetCartProducts(cartItems);
            return Ok(result);
        }
    }
}
