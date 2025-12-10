using SanDiaryApi.DTOs;
using SanDiaryApi.Models;
using SanDiaryApi.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SanDiaryApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController(UserService UserService) : ControllerBase
    {
        private readonly UserService _UserService = UserService;

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var result = await _UserService.GetUserByIdAsync(new GetUserByIdRequest(id));

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetAll()
        {
            var result = await _UserService.GetAllUsersAsync();

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _UserService.DeleteUserByIdAsync(new DeleteUserByIdRequest(id));

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Errors });

            return Ok();
        }
    }
}
