using Microsoft.AspNetCore.Components;

namespace Blecommerce.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _auth;
        private readonly NavigationManager _navigation;

        public OrderService(HttpClient http ,
            AuthenticationStateProvider auth,
            NavigationManager navigation
            )
        {
            _http = http;
            _auth = auth;
            _navigation = navigation;
        }
        public async Task AddOrder()
        {
            if (await isAuthenticated ())
            {
                await _http.PostAsync("api/order", null);

            } else
            {
                _navigation.NavigateTo("login");
            }
        }

        private async Task<bool> isAuthenticated()
        {
            return (await _auth.GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
        }
    }
}
