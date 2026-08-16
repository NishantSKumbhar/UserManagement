using UserManagement.API.Models;

namespace UserManagement.API.Authentication
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user, IEnumerable<string> roles)
        {

        }
    }
}
