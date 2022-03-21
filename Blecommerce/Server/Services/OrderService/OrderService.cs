using System.Security.Claims;

namespace Blecommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext context,ICartService cartService, IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }
        public async Task<ServiceResponse<bool>> AddOrder()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;

            decimal totalPrice = products!.Sum(product => product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();

            products!.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity,
            }));

            var order = new Order
            {
                UserId = _authService.GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            _context.CartItems.RemoveRange(
                _context.CartItems.Where(x => x.UserId == _authService.GetUserId())
            );

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<OrderOverViewDto>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverViewDto>>();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderOverViewDto>();
            orders.ForEach(o => orderResponse.Add(new OrderOverViewDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ?
                $"{o.OrderItems.First().Product.Title} and {o.OrderItems.Count - 1} more..."
                : o.OrderItems.First().Product.Title,
                ImageUrl = o.OrderItems.First().Product.ImageUrl,
            }));

            response.Data = orderResponse;
            return response;
        }
    }
}
