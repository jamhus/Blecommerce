namespace Blecommerce.Server.Services.CartService
{
    public interface ICartServiceBE
    {
        Task<ServiceResponse<List<CartProductDto>>> GetCartProducts(List<CartItem> cartItems);
    }
}
