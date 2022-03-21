namespace Blecommerce.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister user);
        Task<ServiceResponse<string>> Login(UserLoginDto user);
        Task<ServiceResponse<bool>> ChangePassword(ChangePassword model);
        Task<bool> IsAuthenticated();

    }
}
