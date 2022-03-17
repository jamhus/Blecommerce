namespace Blecommerce.Server.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem item);
        Task<List<CartItem>> GetCartItems();
        Task<List<CartProductDto>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeId);
        Task UpdateQuantity(CartProductDto cartProduct);
        Task StoreCartItems(bool emptyLocal);
        Task GetCartItemsCount();

    }
}
