using PostIt.Domain.Interfaces;
using PostIt.Application.Interfaces;

namespace PostIt.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginService(IPasswordHasher passwordHasher, IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<(string Token, string UserId)> LoginUserAsync(string username, string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(password);
            var userId = await _userRepository.LoginUserInDatabase(username, hashedPassword);

            if (!string.IsNullOrEmpty(userId))
            {
                
                var token = _jwtTokenService.GenerateToken(username, userId);
                return (token, userId);
            }

            return (null, null);
        }
    }
}
