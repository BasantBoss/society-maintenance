using SocietyApi.Models;

namespace SocietyApi.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(string username, string password, Role role, string? fullname = null, string? flat = null, string? phone = null);
        Task<string?> AuthenticateAsync(string username, string password);
    }
}