using Microsoft.AspNetCore.Http;
using PostIt.Application.Dto;
using PostIt.Application.Interfaces;
using PostIt.Domain.Data;
using PostIt.Domain.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace PostIt.Application.Services
{
    public class UserService : IUserService
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;  
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
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

        public async Task<bool> AddUserAsync(CreateUserDto userDto, IFormFile profilePicture)
        {
            string hashedPassword = _passwordHasher.HashPassword(userDto.Password);
            string imagedata = null;

            if (profilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    imagedata = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                _logger.LogWarning("Image not given");
            }

            var userEntity = new Users
            {
                Username = userDto.Username,
                Password = hashedPassword,
                FirstName = userDto.FirstName,
                SurName = userDto.SurName,
                EmailAddress = userDto.EmailAddress,
                HomeAddress = userDto.HomeAddress,
                BirthDay = userDto.BirthDay,
                ProfilePicture = imagedata,
            };

            return await _userRepository.AddUserToDatabase(userEntity);
        }

        public async Task<UserDetailDto?> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            byte[]? profilePictureBytes = null;
            if (!string.IsNullOrEmpty(user.ProfilePicture))
            {
                profilePictureBytes = Convert.FromBase64String(user.ProfilePicture);
            }

            return new UserDetailDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FirstName = user.FirstName,
                SurName = user.SurName,
                EmailAddress = user.EmailAddress,
                HomeAddress = user.HomeAddress,
                BirthDay = user.BirthDay,
                ProfilePictureBytes = profilePictureBytes,
                Followers = user.Followers.Select(f => new SimpleUserDto
                {
                    UserId = f.UserId.ToString(),
                    Username = f.Username
                }).ToList(),
                Following = user.Following.Select(f => new SimpleUserDto
                {
                    UserId = f.UserId.ToString(),
                    Username = f.Username
                }).ToList()
            };
        }
    }
}
