using System.Security.Claims;

namespace Blecommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartServiceBE _cartService;
        private readonly IHttpContextAccessor _accessor;

        public OrderService(DataContext context,ICartServiceBE cartService,IHttpContextAccessor accessor)
        {
            _context = context;
            _cartService = cartService;
            _accessor = accessor;
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
                UserId = GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        private int GetUserId() => int.Parse(_accessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

    }
}
