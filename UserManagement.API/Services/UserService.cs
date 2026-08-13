using Microsoft.AspNetCore.Identity;
using UserManagement.API.Data;
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
    }
}
