namespace Blecommerce.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(UserLoginDto model);
        Task<ServiceResponse<bool>> ChangePassword(int userId, string password);
        Task<bool> UserExist (string email);
        int GetUserId();
    }
}
