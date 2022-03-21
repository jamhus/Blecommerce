namespace Blecommerce.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task AddOrder();
        Task<List<OrderOverViewDto>> GetOrders();   
    }
}
