using SanDiaryApi.Common;
using SanDiaryApi.Models;
using SanDiaryApi.Data;
using SanDiaryApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace SanDiaryApi.Services
{
    public class UserService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<Result<User>> GetUserByIdAsync(GetUserByIdRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == req.Id);
            if (user == null)
            {
                return Result<User>.Fail("User does not exist.");
            }
            return Result<User>.Success(user);
        }

        public async Task<Result<List<User>>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return Result<List<User>>.Success(users);
        }

        public async Task<Result<User>> GetUserByEmailAsync(GetUserByEmailRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user == null)
            {
                return Result<User>.Fail("User does not exist.");
            }
            return Result<User>.Success(user);
        }

        public async Task<Result<bool>> DeleteUserByIdAsync(DeleteUserByIdRequest req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == req.Id);
            if (user == null)
            {
                return Result<bool>.Fail("User does not exist.");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
