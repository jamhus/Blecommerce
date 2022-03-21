namespace Blecommerce.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> AddOrder(int userId);
        Task<ServiceResponse<List<OrderOverViewDto>>> GetOrders();
        Task<ServiceResponse<OrderDetailsDto>> GetOrdeDetails(int orderId);
    }
}
