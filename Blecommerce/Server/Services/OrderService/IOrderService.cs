namespace Blecommerce.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> AddOrder();
        Task<ServiceResponse<List<OrderOverViewDto>>> GetOrders();
    }
}
