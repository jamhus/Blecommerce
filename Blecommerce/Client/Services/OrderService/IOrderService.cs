namespace Blecommerce.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> AddOrder();
        Task<List<OrderOverViewDto>> GetOrders();
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}
