namespace Blecommerce.Server.Services.CartService
{
    public interface ICartServiceBE
    {
        Task<ServiceResponse<List<CartProductDto>>> GetCartProducts(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductDto>>> StoreCartItems(List<CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemsCount();
        Task<ServiceResponse<List<CartProductDto>>> GetDbCartProducts();
        Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);
        Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);


    }
}
