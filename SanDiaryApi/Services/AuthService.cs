using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SanDiaryApi.Data;
using SanDiaryApi.DTOs;
using SanDiaryApi.Models;
using SanDiaryApi.Common;
using Microsoft.EntityFrameworkCore.Update;

namespace SanDiaryApi.Services
{
    public class AuthService(AppDbContext context, IConfiguration configuration)
    {
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Result<User>> RegisterAsync(RegisterRequest req)
        {
            if (await _context.Users.AnyAsync(u => u.Email == req.Email))
            {
                return Result<User>.Fail("User already exists.");
            }

            var user = new User
            {
                Email = req.Email,
                Role = "User"
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, req.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Result<User>.Success(user);
        }

        public async Task<Result<string>> LoginAsync(LoginRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user == null)
            {
                return Result<string>.Fail("User does not exist.");
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Result<string>.Fail("Password hash verification failed");
            }

            return Result<string>.Success(CreateToken(user));
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
