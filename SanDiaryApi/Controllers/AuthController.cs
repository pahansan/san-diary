using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SanDiaryApi.Services;
using SanDiaryApi.Models;
using SanDiaryApi.DTOs;

namespace SanDiaryApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService, UserService userService) : ControllerBase
    {
        private readonly AuthService _authService = authService;
        private readonly UserService _userService = userService;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest req)
        {
            var result = await _authService.RegisterAsync(req);
            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest req)
        {
            var loginResult = await _authService.LoginAsync(req);
            if (!loginResult.IsSuccess)
            {
                return BadRequest(new { errors = loginResult.Errors });
            }

            var getUserResult = await _userService.GetUserByEmailAsync(new GetUserByEmailRequest(req.Email));

            if (!getUserResult.IsSuccess)
            {
                return BadRequest(new { errors = getUserResult.Errors });
            }

            return Ok(new LoginResponse
            {
                Token = loginResult.Value!,
                Role = getUserResult.Value?.Role ?? "User",
                UserId = getUserResult.Value?.Id ?? 0
            });
        }
    }
}
