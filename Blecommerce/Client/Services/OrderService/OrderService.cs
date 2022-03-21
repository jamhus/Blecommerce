using Microsoft.AspNetCore.Components;

namespace Blecommerce.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly IAuthService _auth;
        private readonly NavigationManager _navigation;

        public OrderService(HttpClient http ,
            IAuthService auth,
            NavigationManager navigation
            )
        {
            _http = http;
            _auth = auth;
            _navigation = navigation;
        }
        public async Task<string> AddOrder()
        {
            if (await _auth.IsAuthenticated ())
            {
                var result = await _http.PostAsync("api/payment/checkout", null);
                var url = await result.Content.ReadAsStringAsync();
                return url;

            } else
            {
                return "login";
            }
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsDto>>($"api/order/{orderId}");
            return result!.Data!;
        }

        public async Task<List<OrderOverViewDto>> GetOrders()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverViewDto>>>("api/order");
            return result!.Data!;
        }
    }
}
