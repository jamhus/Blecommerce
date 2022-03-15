using Blazored.LocalStorage;

namespace Blecommerce.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;

        public CartService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        public event Action OnChange = default!;

        public async Task AddToCart(CartItem item)
        {
            var cart = await CreateCart();
            cart.Add(item);
            await _localStorage.SetItemAsync("cart", cart);
            OnChange.Invoke();
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            var cart = await CreateCart();
              return cart;
        }

        private async Task<List<CartItem>> CreateCart()
        {
           var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return cart;
        }
    }
}
