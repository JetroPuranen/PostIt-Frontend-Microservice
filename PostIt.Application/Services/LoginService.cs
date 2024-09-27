using PostIt.Domain.Interfaces;
using PostIt.Application.Interfaces;
using PostIt.Application.Dto;
using PostIt.Domain.Data;

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

        public async Task<(string Token, string UserId)> LoginUserAsync(LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
            {
                return (null, null);
            }

            
            var loginData = new LoginData
            {
                Username = loginDto.Username,
                Hashedpassword = _passwordHasher.HashPassword(loginDto.Password)
            };

            
            var userId = await _userRepository.LoginUserInDatabase(loginData);

            if (!string.IsNullOrEmpty(userId))
            {
                var token = _jwtTokenService.GenerateToken(loginDto.Username, userId);
                return (token, userId);
            }

            return (null, null);
        }

    }
}
