using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Models;

namespace UserManagement.API.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking().
                Select(user => new UserResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }).ToListAsync();
        }

        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().
                Where(user => user.Id == id).
                Select(user => new UserResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }).FirstOrDefaultAsync();
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var emailExists = await _context.Users.AnyAsync(user => user.Email == normalizedEmail);
            if (emailExists)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var user = new User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = normalizedEmail,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

        }

        public async Task<bool> UpdateAsync(int id, UpdateUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if(user is null)
            {
                return false;
            }

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            var emailExists = await _context.Users.AnyAsync(user => user.Email == normalizedEmail && user.Id != id);
            if (emailExists)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.Email = normalizedEmail;
            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

            if(user is null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
