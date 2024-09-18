using PostIt.Application.Dto;
using PostIt.Application.Interfaces;
using PostIt.Domain.Data;
using PostIt.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace PostIt.Application.Services
{
    public class UserService : IUserService
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public (bool IsValid, string Message) ValidateUser(CreateUserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Username))
                return (false, "Username is required.");

            if (string.IsNullOrEmpty(userDto.Password))
                return (false, "Password is required.");

            if (string.IsNullOrEmpty(userDto.EmailAddress) || !EmailRegex.IsMatch(userDto.EmailAddress))
                return (false, "Invalid email format.");

            if (userDto.BirthDay >= DateTime.Now)
                return (false, "Invalid birth date.");

            return (true, "Validation successful.");
        }

        public async Task<bool> AddUserAsync(CreateUserDto userDto)
        {
            string hashedPassword = _passwordHasher.HashPassword(userDto.Password);

            var userEntity = new Users
            {
                Username = userDto.Username,
                Password = hashedPassword,
                FirstName = userDto.FirstName,
                SurName = userDto.SurName,
                EmailAddress = userDto.EmailAddress,
                HomeAddress = userDto.HomeAddress,
                BirthDay = userDto.BirthDay,
                ProfilePicture = userDto.ProfilePicture 
            };

            return await _userRepository.AddUserToDatabase(userEntity);
        }
    }
}
