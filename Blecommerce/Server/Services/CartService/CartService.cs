using System.Security.Claims;

namespace Blecommerce.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public CartService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetUserId();

            var sameItem = await _context.CartItems.FirstOrDefaultAsync(
                x => x.ProductId == cartItem.ProductId
                && x.ProductTypeId == cartItem.ProductTypeId
                && x.UserId == cartItem.UserId);

            if(sameItem == null)
            {
                _context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await _context.CartItems
                .Where(ci => ci.UserId == _authService.GetUserId()).ToListAsync())
                .Sum(item => item.Quantity);
            return new ServiceResponse<int> { Data = count };

        }

        public async Task<ServiceResponse<List<CartProductDto>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductDto>>
            {
                Data = new List<CartProductDto>()
            };

            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products
                    .Where(p=> p.Id == cartItem.ProductId)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _context.ProductVariants
                    .Where(v => v.ProductId == cartItem.ProductId
                    && v.ProductTypeId == cartItem.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();

                if (productVariant == null)
                {
                    continue;
                }

                var cartProduct = new CartProductDto
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = cartItem.Quantity
                };

                result.Data.Add(cartProduct);
            }

            return result;
        }

        public async Task<ServiceResponse<List<CartProductDto>>> GetDbCartProducts()
        {
            var items = await _context.CartItems.Where(ci=>ci.UserId == _authService.GetUserId()).ToListAsync();
            return await GetCartProducts(items);
        }

        public async Task<ServiceResponse<bool>> RemoveFromCart(int productId, int productTypeId)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(
                x => x.ProductId == productId
                && x.ProductTypeId == productTypeId
                && x.UserId == _authService.GetUserId());

            if (item == null)
            {
                return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist", Success = false };
            }

            _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<CartProductDto>>> StoreCartItems(List<CartItem> cartItems)

        {
            int userId = _authService.GetUserId();
            cartItems.ForEach(cartItem => cartItem.UserId = userId);

            _context.CartItems.AddRange(cartItems);

            await _context.SaveChangesAsync();
            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(
                x => x.ProductId == cartItem.ProductId
                && x.ProductTypeId == cartItem.ProductTypeId
                && x.UserId == _authService.GetUserId());

            if (item == null)
            {
                return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist", Success = false };
            }
            item.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }
    }
}
