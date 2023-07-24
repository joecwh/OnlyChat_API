using API.Models;
using API.Responses;

namespace API.Services
{
    public interface IAuthService
    {
        Task<HttpResponse<string>> Login(LoginRequest request);
        Task<HttpResponse<UserResponse>> Signup(SignupRequest request);
        Task<string> GenerateToken(User user);
    }
}
