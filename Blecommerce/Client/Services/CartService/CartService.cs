using Blazored.LocalStorage;

namespace Blecommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly IAuthService _auth;

        public CartService(ILocalStorageService localStorage, HttpClient http, IAuthService auth)
        {
            _localStorage = localStorage;
            _http = http;
            _auth = auth;
        }
        public event Action OnChange = default!;

        public async Task AddToCart(CartItem item)
        {
            if (await _auth.IsAuthenticated())
            {
                await _http.PostAsJsonAsync("api/cart/add",item);
            }
            else
            {
                var cart = await FetchOrCreateCart();
                var sameItem = cart.Find(i => i.ProductId == item.ProductId && i.ProductTypeId == item.ProductTypeId);
                if (sameItem == null)
                {
                    cart.Add(item);
                }
                else
                {
                    sameItem.Quantity += item.Quantity;
                }
                await _localStorage.SetItemAsync("cart", cart);
            }
            
            await GetCartItemsCount();
        }


        public async Task GetCartItemsCount()
        {
            if (await _auth.IsAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result!.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }

            OnChange.Invoke();
        }

        public async Task<List<CartProductDto>> GetCartProducts()
        {
            if (await _auth.IsAuthenticated())
            {
                var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductDto>>>("api/cart");
                return response!.Data!;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                    return new List<CartProductDto>();
                var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
                var cartProducts =
                    await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductDto>>>();
                return cartProducts!.Data!;
            }

        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            if (await _auth.IsAuthenticated())
            {
                var payload = new CartItem
                {
                    ProductId = productId,
                    ProductTypeId = productTypeId,
                };

                await _http.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
            await GetCartItemsCount();
        }

        public async Task StoreCartItems(bool emptyLocal)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }

            await _http.PostAsJsonAsync("api/cart", cart);
            if (emptyLocal) await _localStorage.RemoveItemAsync("cart");
        }

        public async Task UpdateQuantity(CartProductDto cartProduct)
        {
            if (await _auth.IsAuthenticated())
            {
                var payload = new CartItem
                {
                    ProductId = cartProduct.ProductId,
                    Quantity = cartProduct.Quantity,
                    ProductTypeId = cartProduct.ProductTypeId,
                };

                await _http.PutAsJsonAsync("api/cart/update-quantity", payload);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == cartProduct.ProductId && x.ProductTypeId == cartProduct.ProductTypeId);
                if (cartItem != null)
                {
                    cartItem.Quantity = cartProduct.Quantity;
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
            await GetCartItemsCount();
        }

        private async Task<List<CartItem>> FetchOrCreateCart()
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
