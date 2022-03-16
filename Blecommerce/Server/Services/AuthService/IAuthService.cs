namespace Blecommerce.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(UserLoginDto model);
        Task<bool> UserExist (string email);
    }
}
