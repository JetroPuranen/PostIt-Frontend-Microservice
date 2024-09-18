using System.Threading.Tasks;
using PostIt.Domain.Interfaces;
using PostIt.Application.Dto;
using PostIt.Domain.Data;

namespace PostIt.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public LoginService(IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<bool> LoginUserAsync(string username, string password)
        {
            
            var hashedPassword = _passwordHasher.HashPassword(password);
            return await _userRepository.LoginUserInDatabase(username, hashedPassword);
        }
    }
}
