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
        public async Task<ServiceResponse<bool>> AddOrder(int userId)
        {
            var products = (await _cartService.GetDbCartProducts(userId)).Data;

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
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            _context.CartItems.RemoveRange(
                _context.CartItems.Where(x => x.UserId == userId)
            );

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<OrderDetailsDto>> GetOrdeDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsDto>();
            var order = await _context.Orders
                .Where(o => o.Id == orderId && o.UserId == _authService.GetUserId())
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .OrderByDescending(o=> o.OrderDate)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found";
                return response;
            }

            var orderDetailsResponse = new OrderDetailsDto
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductDto>()
            };

            order.OrderItems.ForEach(oi => orderDetailsResponse.Products.Add(new OrderDetailsProductDto
            {
                ProductId = oi.ProductId,
                ImageUrl = oi.Product.ImageUrl,
                ProductType = oi.ProductType.Name,
                Quantity = oi.Quantity,
                Title = oi.Product.Title,
                TotalPrice = oi.TotalPrice
            }));
            response.Data = orderDetailsResponse;
            return response;
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
