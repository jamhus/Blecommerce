namespace Blecommerce.Server.Services.CartService
{
    public interface ICartServiceBE
    {
        Task<ServiceResponse<List<CartProductDto>>> GetCartProducts(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductDto>>> StoreCartItems(List<CartItem> cartItems, int userId);
    }
}
