using PostIt.Application.Interfaces;
using PostIt.Domain.Data;
using PostIt.Domain.Interfaces;
using PostIt.Application.Dto;

namespace PostIt.Application.Services
{
    public class FollowerService : IFollowerService
    {
        private readonly IUserRepository _userRepository;

        public FollowerService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AddFollowerAsync(FollowerDto followerDto)
        {
            if (string.IsNullOrEmpty(followerDto.Username) || string.IsNullOrEmpty(followerDto.FollowerUsername))
            {
                throw new ArgumentException("Invalid follower data.");
            }

            
            var followerData = new FollowerData
            {
                Username = followerDto.Username,
                FollowerUsername = followerDto.FollowerUsername
            };

            
            return await _userRepository.AddFollowerToDatabase(followerData);
        }
    }
}
