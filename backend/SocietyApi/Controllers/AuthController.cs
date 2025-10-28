using Microsoft.AspNetCore.Mvc;
using SocietyApi.Models;
using SocietyApi.Services;

namespace SocietyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!Enum.TryParse<Role>(dto.Role, true, out var role))
                return BadRequest("Invalid role");

            var res = await _auth.RegisterAsync(dto.Username, dto.Password, role, dto.FullName, dto.FlatNumber, dto.Phone);
            if (!res.Success) return BadRequest(res.Message);
            return Ok(res.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _auth.AuthenticateAsync(dto.Username, dto.Password);
            if (token == null) return Unauthorized("Invalid username or password");
            return Ok(new { token });
        }
    }

    public record RegisterDto(string Username, string Password, string Role, string? FullName, string? FlatNumber, string? Phone);
    public record LoginDto(string Username, string Password);
}