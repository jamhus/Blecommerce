namespace Blecommerce.Server.Services.CartService
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductDto>>> GetCartProducts(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductDto>>> StoreCartItems(List<CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemsCount();
        Task<ServiceResponse<List<CartProductDto>>> GetDbCartProducts(int? userId = null);
        Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);
        Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);
        Task<ServiceResponse<bool>> RemoveFromCart(int productId, int productTypeId);


    }
}
