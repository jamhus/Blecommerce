namespace Blecommerce.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _auth;

        public AuthService(HttpClient http , AuthenticationStateProvider auth)
        {
            _http = http;
            _auth = auth;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(ChangePassword model)
        {
            var result = await _http.PostAsJsonAsync("api/auth/change-password", model.Password);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<bool> IsAuthenticated()
        {
            return (await _auth.GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
        }

        public async Task<ServiceResponse<string>> Login(UserLoginDto user)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", user);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(UserRegister user)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", user);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
